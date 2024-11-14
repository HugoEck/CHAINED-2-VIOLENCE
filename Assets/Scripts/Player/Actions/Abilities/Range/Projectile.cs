using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour, IAbility
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public Transform playerDirection;
    public float projectileSpeed = 20f;
    public float cooldown = 3f;        // Cooldown duration in seconds
    private float shootTimer;

    private bool bHasshot = false;

    void Start()
    {
        shootTimer = cooldown;
    }

    public void UseAbility()
    {      
        Shoot();    
    }
    private void Update()
    {
        if (bHasshot)
        {
            shootTimer -= Time.deltaTime;

            if (shootTimer <= 0)
            {
                bHasshot = false;
                shootTimer = cooldown;
            }
        }
    }

    void Shoot()
    {
        if (bHasshot) return;

        shootTimer = cooldown;
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
        bHasshot = true;
    }
}
