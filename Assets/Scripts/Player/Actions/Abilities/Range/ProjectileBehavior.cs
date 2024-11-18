using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float explosionRadius = 5f;
    public float explosionDamage = 50f;
    public LayerMask enemyLayer;
    public GameObject explosionEffectPrefab;

    private bool hasExploded = false;
    void OnCollisionEnter(Collision collision)
    {
        // Ensure the explosion happens only once
        if (!hasExploded && collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            hasExploded = true;  // Set the flag so this block only runs once

            // Disable the projectile's collider to prevent further collisions
            GetComponent<Collider>().enabled = false;

            // Trigger explosion effect and logic
            Explode();

            // Instantiate the visual explosion effect
            if (explosionEffectPrefab != null)
            {
                GameObject explosionInstance = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);

                Destroy(explosionInstance, 2f);  // Destroy the explosion effect after 2 seconds
            }

            // Destroy the projectile immediately after the explosion effect is instantiated
            Destroy(gameObject); // This ensures the projectile is removed right away
        }
    }

    // Method to handle the explosion and damage enemies in the radius
    void Explode()
    {
        Debug.Log("Projectile exploded!");

        // Find all colliders within the explosion radius that are on the enemy layer
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            // Apply damage to each enemy
            BaseManager enemyManager = enemy.GetComponent<BaseManager>();
            if (enemyManager != null)
            {
                enemyManager.DealDamageToEnemy(explosionDamage);
                Debug.Log("Damaged enemy: " + enemy.name);
            }
        }
    }
}
