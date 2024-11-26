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

    // PRIVATE
    private float boulderDamage = 3f;

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
    }

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
                enemy.chainEffects.ActivateRagdollStun(4, gameObject);

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
    private IEnumerator DestroyBoulderWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
    //private IEnumerator DeactivateRagdoll(BaseManager baseManager, float delay)
    //{
    //    yield return new WaitForSeconds(delay);
    //    if (baseManager != null && baseManager.behaviorMethods != null)
    //    {
    //        baseManager.behaviorMethods.ToggleRagdoll(false);
    //        Debug.Log("Ragdoll deactivated for: " + baseManager.gameObject.name);
    //    }
    //}

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
