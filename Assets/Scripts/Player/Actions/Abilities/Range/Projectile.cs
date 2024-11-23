using UnityEngine;

public class Projectile : MonoBehaviour, IAbility
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float cooldown;
    private float lastShootTime = -Mathf.Infinity;
    private float projectileSpeed = 20f;
    private float projectileLifeTime = 5f;

    public void UseAbility()
    {
        if (Time.time >= lastShootTime + cooldown)
        {
            Shoot();
            lastShootTime = Time.time;
        }
        else
        {
            Debug.Log("Projectile ability is on cooldown.");
        }
    }

    void Shoot()
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("Projectile prefab is not assigned.");
            return;
        }

        if (firePoint == null)
        {
            Debug.LogError("Fire point is not assigned.");
            return;
        }

        // Retrieve speed from the particle system.
        ParticleSystem particleSystem = projectilePrefab.GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            ParticleSystem.MainModule mainModule = particleSystem.main;
            projectileSpeed = mainModule.startSpeed.constant;
        }
        else
        {
            Debug.LogWarning("Projectile prefab is missing a ParticleSystem. Using default projectile speed.");
        }

        Vector3 direction = transform.forward.normalized;
        //Debug.Log("Spawning projectile with direction: " + direction);

        // Instantiate the projectile
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position + direction * 3f, Quaternion.LookRotation(direction));

        if (projectile != null)
        {
            Debug.Log("Projectile spawned: " + projectile.name);
        }
        else
        {
            Debug.LogError("Failed to spawn projectile.");
            return;
        }

        projectile.layer = LayerMask.NameToLayer("Projectile");

        // Ignore collision between the player and the projectile
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Projectile"));

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.velocity = direction * projectileSpeed;
        }
        else
        {
            Debug.LogError("Projectile is missing Rigidbody component.");
        }

        Destroy(projectile, projectileLifeTime);
    }
}
