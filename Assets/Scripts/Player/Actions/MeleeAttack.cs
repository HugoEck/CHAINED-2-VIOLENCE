using UnityEngine;

public class MeleeAttack : PlayerCombat
{
    // Override the base class's Attack method to handle melee combat.
    public override void Attack()
    {
        Debug.Log("Performing Melee Attack!");

        // Find all enemies within the attack range
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider enemy in hitEnemies)
        {
            BaseManager enemyManager = enemy.GetComponent<BaseManager>();
            if (enemyManager != null)
            {
                enemyManager.SetHealth(attackDamage);
                Debug.Log("Hit enemy: " + enemy.name);
            }
        }
    }

    // Optional: Visualize the attack range in the scene view for debugging.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}