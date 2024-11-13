using UnityEngine;

public class ConeAbility : MonoBehaviour, IAbility
{
    public float coneRange = 10f;
    public float coneAngle = 90f;
    public float coneDamage = 50f;
    public GameObject coneEffectPrefab;
    public Transform coneAnchor;            // Anchor object for the effect to follow

    public void UseAbility()
    {
        // Check if the cone anchor and effect prefab are assigned
        if (coneEffectPrefab != null && coneAnchor != null)
        {
            // Instantiate the visual effect at the anchor's position
            GameObject coneEffect = Instantiate(coneEffectPrefab, coneAnchor.position, Quaternion.identity);

            // Make the effect follow the cone anchor
            coneEffect.transform.SetParent(coneAnchor);

            // Align and scale the visual effect to match the cone
            coneEffect.transform.localRotation = Quaternion.identity; // Keep aligned with the anchor's forward direction
            coneEffect.transform.localScale = new Vector3(coneRange, coneRange, coneRange); // Scale as needed

            // Optionally destroy the effect after some time if it's temporary
            Destroy(coneEffect, 2.0f);
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
