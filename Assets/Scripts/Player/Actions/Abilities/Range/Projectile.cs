using UnityEngine;

public class Projectile : MonoBehaviour, IAbility
{
    public GameObject projectilePrefab;  // Reference to the projectile prefab
    public Transform firePoint;          // The point from where the projectile is shot (usually the player's position)
    public float projectileSpeed = 20f;  // Speed of the projectile

    public void UseAbility()
    {
        Shoot();
    }

    void Shoot()
    {
        // Shooting logic
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        Vector3 targetPosition;
        if (Physics.Raycast(ray, out hit))
        {
            targetPosition = hit.point;  // Get the hit point
        }
        else
        {
            targetPosition = ray.GetPoint(1000);  // Shoot towards a distant point
        }

        Vector3 direction = targetPosition - firePoint.position;
        direction.Normalize();  // Normalize the direction to make it a unit vector

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
