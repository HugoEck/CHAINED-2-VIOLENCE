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

    // PRIVATE
    private float boulderDamage = 3f;
    private float dustSpawnTimer = 0f;
    private float dustSpawnInterval = 0.4f;

    private Vector3 moveDirection;
    private Vector3 targetPosition;

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
        moveDirection = (targetPosition - transform.position).normalized; // Properly calculate direction
    }

    private void FixedUpdate()
    {
        if (rb == null) return;

        // Apply force to move the boulder
        rb.velocity = moveDirection * boulderSpeed;

        // Apply torque to make it roll
        ApplyRollingTorque();
        SpawnDustParticles();
    }

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
                Debug.Log("Player hit by boulder. Current health: " + player.currentHealth);

                // Apply damage to the player
                player.SetHealth(boulderDamage);

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

    private IEnumerator DestroyBoulderWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    #region TARGET & TORQUE
    private Vector3 DetermineTargetPosition(Vector3 spawnPosition)
    {
        // Determine the target position based on the spawn position
        if (Mathf.Approximately(spawnPosition.x, 45)) // Spawned on Right Edge
            return new Vector3(-45, spawnPosition.y, spawnPosition.z);
        else if (Mathf.Approximately(spawnPosition.x, -45)) // Spawned on Left Edge
            return new Vector3(45, spawnPosition.y, spawnPosition.z);
        else if (Mathf.Approximately(spawnPosition.z, 45)) // Spawned on Top Edge
            return new Vector3(spawnPosition.x, spawnPosition.y, -45);
        else if (Mathf.Approximately(spawnPosition.z, -45)) // Spawned on Bottom Edge
            return new Vector3(spawnPosition.x, spawnPosition.y, 45);

        // Default target position if no match
        return Vector3.zero;
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
