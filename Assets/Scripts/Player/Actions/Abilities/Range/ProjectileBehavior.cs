using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float explosionRadius = 5f;
    public float baseExplosionDamage = 50f;
    public float explosionDamage;
    public LayerMask enemyLayer;
    public GameObject explosionEffectPrefab;

    private HashSet<Collider> hitEnemiesOnce;
    public PlayerAttributes playerAttributes;
    private bool hasExploded = false;

    private void Start()
    {
        hitEnemiesOnce = new HashSet<Collider>();
    }

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
        explosionDamage = baseExplosionDamage + playerAttributes.attackDamage;

        Debug.Log("Projectile exploded!");

        // Find all colliders within the explosion radius that are on the enemy layer
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            // Check if enemy has been hit
            if (hitEnemiesOnce.Contains(enemy)) continue;

            hitEnemiesOnce.Add(enemy);

            // Apply damage to each enemy
            BaseManager enemyManager = enemy.GetComponent<BaseManager>();
            if (enemyManager != null)
            {
                enemyManager.DealDamageToEnemy(explosionDamage,BaseManager.DamageType.AbilityDamage);
                Debug.Log("Damaged enemy: " + enemy.name + explosionDamage);
            }
        }
    }
}
