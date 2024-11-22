using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ConeAbility : MonoBehaviour, IAbility
{
    public float coneRange = 10f;
    public float coneAngle = 90f;
    public float baseConeDamage = 50f;
    private float coneDamage;
    public GameObject coneEffectPrefab;
    public Transform coneAnchor;           // Anchor object for the effect to follow
    public PlayerCombat playerCombat;

    public float cooldown = 5f;            // Cooldown duration in seconds
    private float lastUseTime = -Mathf.Infinity;  // Time when the ability was last used

    private HashSet<Collider> hitEnemiesOnce;

    private void Start()
    {
        playerCombat = GetComponent<PlayerCombat>();
        hitEnemiesOnce = new HashSet<Collider>();
    }

    public void UseAbility()
    {
        // Check if the cooldown has elapsed
        if (Time.time >= lastUseTime + cooldown)
        {
            ActivateConeAbility();
            lastUseTime = Time.time; // Update the last use time
        }
        else
        {
            Debug.Log("Cone ability is on cooldown.");
        }
    }

    void ActivateConeAbility()
    {
        coneDamage = baseConeDamage + playerCombat.attackDamage;

        // Instantiate the visual effect at the anchor's position
        if (coneEffectPrefab != null && coneAnchor != null)
        {
            GameObject coneEffect = Instantiate(coneEffectPrefab, coneAnchor.position, Quaternion.identity);

            // Make the effect follow the cone anchor
            coneEffect.transform.SetParent(coneAnchor);

            // Align and scale the visual effect to match the cone
            coneEffect.transform.localRotation = Quaternion.identity; // Keep aligned with the anchor's forward direction
            coneEffect.transform.localScale = new Vector3(coneRange, coneRange, coneRange);

            Destroy(coneEffect, 2.0f); // Destroy the effect after some time if it's temporary
        }

        // Find all enemies within the range
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, coneRange);

        foreach (Collider enemy in hitEnemies)
        {
            // Check if enemy has been hit
            if (hitEnemiesOnce.Contains(enemy)) continue;

            hitEnemiesOnce.Add(enemy);

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
                   // enemyManager.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 150, ForceMode.Force);
                }
            }
        }

        Debug.Log("Cone Ability triggered.");
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, coneRange);

        Vector3 forwardDirection = transform.forward * coneRange;

        Quaternion leftRotation = Quaternion.AngleAxis(-coneAngle / 2, Vector3.up);
        Vector3 leftEdge = leftRotation * forwardDirection;

        Quaternion rightRotation = Quaternion.AngleAxis(coneAngle / 2, Vector3.up);
        Vector3 rightEdge = rightRotation * forwardDirection;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + leftEdge);
        Gizmos.DrawLine(transform.position, transform.position + rightEdge);

        Gizmos.color = Color.yellow;
        for (float i = -coneAngle / 2; i <= coneAngle / 2; i += coneAngle / 10)
        {
            Quaternion stepRotation = Quaternion.AngleAxis(i, Vector3.up);
            Vector3 edge = stepRotation * forwardDirection;
            Gizmos.DrawLine(transform.position, transform.position + edge);
        }
    }
}
