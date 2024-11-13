using UnityEngine;

public class Projectile : MonoBehaviour, IAbility
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public Transform playerDirection;
    public float projectileSpeed = 20f;

    public void UseAbility()
    {
        Shoot();
    }

    void Shoot()
    {
        Vector3 direction = playerDirection.forward.normalized;

        // Instantiate the projectile at a slight offset to avoid immediate collision
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position + direction * 1f, Quaternion.LookRotation(direction));

        projectile.layer = LayerMask.NameToLayer("Projectile");

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
