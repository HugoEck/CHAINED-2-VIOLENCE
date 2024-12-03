using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderManager : MonoBehaviour
{
    #region VARIABLES
    // PUBLIC
    public float despawnTime = 5f;
    public float boulderSpeed = 10f;

    public List<GameObject> objectsToRemove = new List<GameObject>();
    public itemAreaSpawner spawner;

    [SerializeField] public GameObject destructionParticle;
    [SerializeField] public GameObject portalParticle;
    [SerializeField] public GameObject dustParticle;
    public GameObject pathParticle; // Particle prefab for the path trail

    // PRIVATE
    private float boulderDamage = 3f;
    private float dustSpawnTimer = 0f;
    private float dustSpawnInterval = 0.4f;
    private float damageCooldownDuration = 3f; // Cooldown duration in seconds

    private Vector3 moveDirection;
    private Vector3 targetPosition;

    private GameObject currentPathParticle; // Reference to the active path particle
    private Dictionary<GameObject, float> damageCooldowns = new Dictionary<GameObject, float>();
    private Rigidbody rb;
    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody not found on the boulder! Please add a Rigidbody component.");
            return;
        }

        targetPosition = DetermineTargetPosition(transform.position);
        moveDirection = (targetPosition - transform.position).normalized;

        SpawnPathParticle();
    }

    private void FixedUpdate()
    {
        if (rb == null) return;

        // Apply force to move the boulder
        rb.velocity = moveDirection * boulderSpeed;

        // Apply torque to make it roll
        ApplyRollingTorque();
        UpdatePathParticle();
        SpawnDustParticles();
    }

    #region Path Particle
    private void SpawnPathParticle()
    {
        if (pathParticle != null)
        {
            float yOffset = 1.0f; // Adjust this value to set the desired height
            Vector3 particlePosition = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);
            Quaternion particleRotation = Quaternion.LookRotation(moveDirection);

            currentPathParticle = Instantiate(pathParticle, particlePosition, particleRotation);
            Debug.Log($"Path particle spawned at: {particlePosition}");
        }
        else
        {
            Debug.LogWarning("Path particle prefab is not assigned.");
        }
    }
    private void UpdatePathParticle()
    {
        if (currentPathParticle != null)
        {
            float yOffset = 1.0f; // Same Y offset as in SpawnPathParticle
            Vector3 targetPositionWithOffset = new Vector3(targetPosition.x, targetPosition.y + yOffset, targetPosition.z);

            currentPathParticle.transform.position = Vector3.MoveTowards(
                currentPathParticle.transform.position,
                targetPositionWithOffset,
                (boulderSpeed * 1.5f) * Time.fixedDeltaTime
            );

            // Update the rotation to always face the travel direction
            currentPathParticle.transform.rotation = Quaternion.LookRotation(moveDirection);

            // Destroy the path particle if it reaches the target position
            if (Vector3.Distance(currentPathParticle.transform.position, targetPosition) < 0.1f)
            {
                Destroy(currentPathParticle);
                currentPathParticle = null;
            }
        }
    }

    private void OnDestroy()
    {
        // Destroy the path particle when the boulder is destroyed
        if (currentPathParticle != null)
        {
            Destroy(currentPathParticle);
        }
    }
    #endregion

    #region DAMAGE AND COLLIDE LOGIC
    private void OnCollisionEnter(Collision collision)
    {
        // Check for specific collision tags
        if (collision.gameObject.CompareTag("Misc"))
        {
            if (destructionParticle != null && collision.contacts.Length > 0)
            {
                Vector3 collisionPoint = collision.contacts[0].point;

                collisionPoint.y = 0;

                GameObject particlesGo = Instantiate(destructionParticle, collisionPoint, Quaternion.identity);
                Destroy(particlesGo, 2f); // Destroy the particle system after 2 seconds
            }
            else
            {
                Debug.LogWarning("Particles prefab is not assigned or no collision contacts available.");
            }

            Destroy(collision.gameObject);

            // Skip further processing to ensure the boulder keeps its direction
            //return;

            if (spawner != null)
            {
                spawner.RemoveObjectFromCollision(collision.gameObject);
            }
            objectsToRemove.Add(collision.gameObject);


            Destroy(collision.gameObject);

            // Skip further processing to ensure the boulder keeps its direction
            return;
        }

        // Check if the collided object is a player
        if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player != null)
            {
                // Check if the player is on cooldown
                if (damageCooldowns.TryGetValue(collision.gameObject, out float lastDamageTime))
                {
                    // Skip if still on cooldown
                    if (Time.time - lastDamageTime < damageCooldownDuration)
                        return;
                }

                // Apply damage to the player
                Debug.Log($"Player hit by boulder. Current health: {player.currentHealth}");
                player.SetHealth(boulderDamage);

                // Update the cooldown time for this player
                damageCooldowns[collision.gameObject] = Time.time;

                // Skip further processing to ensure the boulder keeps its direction
                return;
            }
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            BaseManager enemy = collision.gameObject.GetComponent<BaseManager>();

            if (enemy != null)
            {
                enemy.chainEffects.ActivateRagdollStun(4, gameObject, 100);

                // Skip further processing to ensure the boulder keeps its direction
                return;
            }
        }

        if (collision.gameObject.CompareTag("Map"))
        {
            if (portalParticle != null && collision.contacts.Length > 0)
            {
                Vector3 collisionPoint = collision.contacts[0].point;
                Vector3 collisionNormal = collision.contacts[0].normal;
                Quaternion portalRotation = Quaternion.LookRotation(-collisionNormal); // Rotate to face the boulder
                GameObject particlesGo = Instantiate(portalParticle, collisionPoint, portalRotation);
                particlesGo.transform.Rotate(0, 180, 0);
                Destroy(particlesGo, 2f); // Destroy the particle system after 2 seconds
            }
            else
            {
                Debug.LogWarning("Particles prefab is not assigned or no collision contacts available.");
            }

            #region REMOVE COLLIDERS AT COLLISION
            Collider boulderCollider = GetComponent<Collider>();
            if (boulderCollider != null)
            {
                boulderCollider.enabled = false;
            }
            Obi.ObiCollider obiCollider = GetComponent<Obi.ObiCollider>();
            if (obiCollider != null)
            {
                obiCollider.enabled = false;
            }
            #endregion

            if (spawner != null)
            {
                spawner.RemoveObjectFromCollision(gameObject);
            }

            // Destroy the boulder
            StartCoroutine(DestroyBoulderWithDelay(0.3f));
        }
    }
    #endregion

    #region PARTICLES
    private void SpawnDustParticles()
    {
        // Update the dust spawn timer
        dustSpawnTimer += Time.fixedDeltaTime;

        // Check if it's time to spawn a new particle
        if (dustSpawnTimer >= dustSpawnInterval)
        {
            dustSpawnTimer = 0f; // Reset the timer

            if (dustParticle != null)
            {
                // Ensure the spawn position is at the center of the boulder's X and Z axes
                Vector3 spawnPosition = new Vector3(
                    transform.position.x,
                    transform.position.y - (GetComponent<Collider>().bounds.extents.y + 0.2f), // Slightly below the boulder
                    transform.position.z
                );

                Quaternion spawnRotation = Quaternion.identity;

                // Adjust rotation to face the opposite of the boulder's velocity
                if (rb != null && rb.velocity.magnitude > 0.1f)
                {
                    spawnRotation = Quaternion.LookRotation(-rb.velocity.normalized, Vector3.up);
                }

                // Instantiate the dust particle
                GameObject dust = Instantiate(dustParticle, spawnPosition, spawnRotation);

                // Destroy the particle after a short duration
                Destroy(dust, 1f);
            }
            else
            {
                Debug.LogWarning("Dust particle prefab is not assigned.");
            }
        }
    }
    #endregion

    #region DESTROY BOULDER
    private IEnumerator DestroyBoulderWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(pathParticle);
        Destroy(gameObject);
    }
    #endregion

    #region TARGET & TORQUE
    private Vector3 DetermineTargetPosition(Vector3 spawnPosition)
    {
        // Handle edges
        if (Mathf.Approximately(spawnPosition.x, 45)) // Spawned on Right Edge
            return new Vector3(-45, spawnPosition.y, spawnPosition.z);
        else if (Mathf.Approximately(spawnPosition.x, -45)) // Spawned on Left Edge
            return new Vector3(45, spawnPosition.y, spawnPosition.z);
        else if (Mathf.Approximately(spawnPosition.z, 45)) // Spawned on Top Edge
            return new Vector3(spawnPosition.x, spawnPosition.y, -45);
        else if (Mathf.Approximately(spawnPosition.z, -45)) // Spawned on Bottom Edge
            return new Vector3(spawnPosition.x, spawnPosition.y, 45);

        // Handle specific corner cases
        if (Mathf.Approximately(spawnPosition.x, -35) && Mathf.Approximately(spawnPosition.z, -35))
            return new Vector3(35, spawnPosition.y, 35); // Target Right
        if (Mathf.Approximately(spawnPosition.x, -35) && Mathf.Approximately(spawnPosition.z, 35))
            return new Vector3(35, spawnPosition.y, -35); // Target Bottom

        // Log a warning for unexpected spawn positions
        Debug.LogWarning($"Unexpected spawn position: {spawnPosition}. Defaulting to Vector3.zero.");
        return Vector3.zero; // Default target position if no match
    }
    public void SetMovementDirection(Vector3 direction)
    {
        moveDirection = direction.normalized;
    }

    private void ApplyRollingTorque()
    {
        // Calculate torque direction (cross product with movement and up axis)
        Vector3 rollingAxis = Vector3.Cross(Vector3.up, moveDirection).normalized;

        // Calculate torque magnitude based on speed and mass
        float torqueMagnitude = boulderSpeed * rb.mass;

        // Apply torque
        rb.AddTorque(rollingAxis * torqueMagnitude, ForceMode.Acceleration);
    }
    #endregion
}
