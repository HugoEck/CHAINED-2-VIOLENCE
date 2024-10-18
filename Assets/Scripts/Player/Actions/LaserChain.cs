using UnityEngine;

public class LaserChain : MonoBehaviour
{
    public bool isLaserActive = false;  // Flag to activate the laser
    public float laserDuration = 3.0f;  // Duration for how long the laser stays active
    public float laserDamage = 10f;     // Amount of damage dealt to enemies
    public float laserRange = 2.0f;     // Range of the laser (like a melee attack range)

    private float laserTimer;

    private void Update()
    {
        if (isLaserActive)
        {
            // Handle laser timer
            laserTimer -= Time.deltaTime;
            if (laserTimer <= 0)
            {
                DeactivateLaser();
            }
            else
            {
                PerformLaserAttack();
            }
        }
    }

    // Call this method to activate the laser on this chain segment
    public void ActivateLaser()
    {
        isLaserActive = true;
        laserTimer = laserDuration;
        Debug.Log("Laser Activated on Chain Segment!");
        SpawnAbilityChainSegments.instance.SpawnLaserChainSegments();
    }

    // Call this method to deactivate the laser
    private void DeactivateLaser()
    {
        isLaserActive = false;
        Debug.Log("Laser Deactivated on Chain Segment.");
        SpawnAbilityChainSegments.instance.DeactivateLaserChainSegments();
    }

    // Similar to the Melee Attack, find all enemies in range and deal damage
    private void PerformLaserAttack()
    {
        Debug.Log("Performing Laser Attack!");

        SpawnAbilityChainSegments.instance.UpdateLaserChainSegments();
        // Find all enemies within the laser's range
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, laserRange);
        foreach (Collider enemy in hitEnemies)
        {
            BaseManager enemyManager = enemy.GetComponent<BaseManager>();
            if (enemyManager != null)
            {
                enemyManager.DealDamageToEnemy(laserDamage);
                Debug.Log("Hit enemy with laser: " + enemy.name);
            }
        }
    }

    // Optional: Visualize the laser's attack range in the scene view for debugging.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, laserRange);
    }
}
