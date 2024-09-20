using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public float attackRange = 1.5f; // Range of the basic attack
    public int attackDamage = 10;    // Damage of the basic attack
    public float attackCooldown = 1.0f; // Cooldown between basic attacks
    public float abilityCooldown = 5.0f; // Cooldown between ability usage

    public Transform attackPoint;    // Point from where the attack is initiated
    public float abilityRange = 3.0f; // Range of the ability
    public int abilityDamage = 25;   // Damage of the ability

    private float nextAttackTime = 0f;
    private float nextAbilityTime = 0f;

    void Update()
    {
        // Check for basic attack input and cooldown
        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time >= nextAttackTime)
        {
            BasicAttack();
            nextAttackTime = Time.time + attackCooldown; // Set the next attack time
            Debug.Log("ATTACKED");
        }

        // Check for ability input and cooldown
        if (Input.GetKeyDown(KeyCode.Q) && Time.time >= nextAbilityTime)
        {
            UseAbility();
            nextAbilityTime = Time.time + abilityCooldown; // Set the next ability time
        }
    }

    void BasicAttack()
    {
        // Detect enemies in the attack range
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange);

        // Damage each enemy detected
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
                Debug.Log("Enemy hit with basic attack!");
            }
        }
    }

    void UseAbility()
    {
        // Detect enemies in the ability range
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, abilityRange);

        // Damage each enemy detected
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                enemy.GetComponent<Enemy>().TakeDamage(abilityDamage);
                Debug.Log("Enemy hit with ability!");
            }
        }
    }

    // Visualize the attack and ability range in the editor
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint.position, abilityRange);
    }
}