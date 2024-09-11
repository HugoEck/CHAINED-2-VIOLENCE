using UnityEngine;

public class SimpleCombatController : MonoBehaviour
{
    public float attackCooldown = 1f; // Cooldown time between attacks
    public float abilityCooldown = 5f; // Cooldown time between abilities
    public float attackDamage = 10f; // Damage dealt per attack
    public float attackRange = 2f; // The range within which the attack can hit

    private float lastAttackTime;
    private float lastAbilityTime;

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        // Handle attack input
        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time > lastAttackTime + attackCooldown)
        {
            PerformAttack();
        }

        // Handle ability input
        if (Input.GetKeyDown(KeyCode.Mouse1) && Time.time > lastAbilityTime + abilityCooldown)
        {
            UseAbility();
        }
    }

    void PerformAttack()
    {
        lastAttackTime = Time.time;
        Debug.Log("Attacking!"); // Optional: Show debug message

        // Find all colliders within attack range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy")) // Check if it is an enemy
            {
                Enemy enemy = hitCollider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(attackDamage); // Call TakeDamage on the enemy
                    Debug.Log("Hit enemy: " + hitCollider.name); // Output debug message
                }
            }
        }
    }

    void UseAbility()
    {
        lastAbilityTime = Time.time;
        Debug.Log("Used ability!");
        // Implement ability logic here
    }

    // Visualize the attack range in the Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}