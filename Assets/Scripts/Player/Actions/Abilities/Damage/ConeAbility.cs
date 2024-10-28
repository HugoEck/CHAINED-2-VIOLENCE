using UnityEngine;

public class ConeAbility : MonoBehaviour, IAbility
{
    public float coneRange = 10f;          // Range of the cone
    public float coneAngle = 90f;         // Angle of the cone in degrees
    public float coneDamage = 50f;        // Damage dealt to enemies in the cone

    public GameObject attackVisualPrefab; // Reference to the visual effect asset

    public void UseAbility()
    {
        // Play the visual effect
        if (attackVisualPrefab != null)
        {
            GameObject attackVisual = Instantiate(
                attackVisualPrefab,
                transform.position,
                Quaternion.LookRotation(transform.forward)
            );

            // Optional: Destroy the effect after a short duration to clean up
            Destroy(attackVisual, 2f);  // Adjust time as needed for effect duration
        }

        // Find all enemies within the range
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, coneRange);

        foreach (Collider enemy in hitEnemies)
        {
            Vector3 directionToEnemy = (enemy.transform.position - transform.position).normalized;
            float angleToEnemy = Vector3.Angle(transform.forward, directionToEnemy);

            // Check if the enemy is within the cone's angle
            if (angleToEnemy <= coneAngle / 2)
            {
                BaseManager enemyManager = enemy.GetComponent<BaseManager>();
                if (enemyManager != null)
                {
                    enemyManager.DealDamageToEnemy(coneDamage);
                    Debug.Log("Cone hit enemy: " + enemy.name);
                }
            }
        }

        Debug.Log("Cone Ability triggered.");
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return; // Optional: Only show in Play Mode

        // Draw the cone range as a sphere
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, coneRange);

        // Draw lines representing the cone edges
        Vector3 forwardDirection = transform.forward * coneRange;

        // Calculate the left edge of the cone
        Quaternion leftRotation = Quaternion.AngleAxis(-coneAngle / 2, Vector3.up);
        Vector3 leftEdge = leftRotation * forwardDirection;

        // Calculate the right edge of the cone
        Quaternion rightRotation = Quaternion.AngleAxis(coneAngle / 2, Vector3.up);
        Vector3 rightEdge = rightRotation * forwardDirection;

        // Draw cone edges
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + leftEdge);
        Gizmos.DrawLine(transform.position, transform.position + rightEdge);

        // Optionally, draw arc lines to better visualize the cone
        Gizmos.color = Color.yellow;
        for (float i = -coneAngle / 2; i <= coneAngle / 2; i += coneAngle / 10)
        {
            Quaternion stepRotation = Quaternion.AngleAxis(i, Vector3.up);
            Vector3 edge = stepRotation * forwardDirection;
            Gizmos.DrawLine(transform.position, transform.position + edge);
        }
    }
}
