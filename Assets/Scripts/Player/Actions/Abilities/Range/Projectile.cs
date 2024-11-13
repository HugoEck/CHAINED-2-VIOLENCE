using UnityEngine;

public class Projectile : MonoBehaviour, IAbility
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public Transform playerDirection;
    public float projectileSpeed = 20f;
    public float cooldown = 2f;        // Cooldown duration in seconds
    private float lastShootTime = -Mathf.Infinity; // Time when the ability was last used

    public void UseAbility()
    {
        // Check if the ability is ready (cooldown has elapsed)
        if (Time.time >= lastShootTime + cooldown)
        {
            Shoot();
            lastShootTime = Time.time; // Update the last use time
        }
        else
        {
            Debug.Log("Projectile ability is on cooldown.");
        }
    }

    void Shoot()
    {
        Vector3 direction = playerDirection.forward.normalized;

        // Instantiate the projectile at a slight offset to avoid immediate collision
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position + direction * 1f, Quaternion.LookRotation(direction));

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
    }
}
