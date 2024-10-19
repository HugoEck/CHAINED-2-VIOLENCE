using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(SwingAbility))]
public class PlayerCombat : MonoBehaviour
{
    private enum PlayerClass
    {
        Tank,
        Meele,
        Support,
        Ranged
    };

    PlayerClass currentPlayerClass = PlayerClass.Tank;

    public float attackCooldown = 1f;  // Cooldown between attacks
    public float abilityCooldown = 5f; // Cooldown between abilities
    public float attackRange = 2f;     // Range of attack
    public float attackDamage = 10f;   // Damage per attack

    protected float lastAttackTime;
    protected float lastAbilityTime;

    #region Ability components

    private SwingAbility swingAbility;

    #endregion

    private void Start()
    {
        swingAbility = GetComponent<SwingAbility>();
    }

    /// <summary>
    /// This method is used for basic attacks (Called in Player script)
    /// </summary>
    public void UseBaseAttack()
    {       
        // Find all enemies within the attack range
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider enemy in hitEnemies)
        {
            BaseManager enemyManager = enemy.GetComponent<BaseManager>();
            if (enemyManager != null)
            {
                enemyManager.DealDamageToEnemy(attackDamage);
                Debug.Log("Hit enemy: " + enemy.name);
            }
        }

        Debug.Log("Base Attack triggered.");

    }

    /// <summary>
    /// This method uses the ability that the player has for its class (Called in Player script)
    /// </summary>
    public void UseAbility()
    {
        switch (currentPlayerClass)
        {

            case PlayerClass.Meele:            

                break;

            case PlayerClass.Support:

                break;

            case PlayerClass.Ranged:

                break;

            case PlayerClass.Tank:

                swingAbility.UseAbility();

                break;

        }
    }

    // Method to set the player's attack damage (used for upgrades)
    public void SetAttackDamage(float newAttackDamage)
    {
        attackDamage = newAttackDamage;
        Debug.Log("Player attack damage set to: " + attackDamage);
    }
}