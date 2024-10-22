using UnityEngine;

public class Projectile : MonoBehaviour, IAbility
{
    public GameObject projectilePrefab;  // Reference to the projectile prefab
    public Transform firePoint;          // The point from where the projectile is shot (usually the player's position)
    public Transform playerDirection;    // A reference to the transform representing the player's forward direction (e.g., the cube in front of the player)
    public float projectileSpeed = 20f;  // Speed of the projectile

    public void UseAbility()
    {
        Shoot();
    }

    void Shoot()
    {
        // Use the forward direction of the player or the cube object to determine the shooting direction
        Vector3 direction = playerDirection.forward;
        direction.Normalize();  // Ensure the direction is a unit vector

        // Instantiate the projectile at the fire point's position
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position + direction * 1.0f, Quaternion.identity);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.velocity = direction * projectileSpeed;  // Apply velocity to the projectile
        }
        else
        {
            Debug.LogError("Projectile is missing Rigidbody component.");
        }
    }
}
