using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Chained2ViolenceGameManager;


[RequireComponent(typeof(SwingAbility))]
public class PlayerCombat : MonoBehaviour
{
    public enum PlayerClass
    {
        Tank,
        Melee,
        Support,
        Ranged
    };

    public PlayerClass currentPlayerClass;
    public event Action<PlayerClass> OnPlayerClassChanged;

    public float attackCooldown = 1f;  // Cooldown between attacks
    public float abilityCooldown = 5f; // Cooldown between abilities
    public float attackRange = 2f;     // Range of attack
    public float attackDamage = 10f;   // Damage per attack

    protected float lastAttackTime;
    protected float lastAbilityTime;

    #region Ability components

    private SwingAbility swingAbility;
    private Projectile projectile;
    private ShieldAbility shieldAbility;

    #endregion

    private void Start()
    {
        int playerId = gameObject.GetComponent<Player>()._playerId;

        // Set the player classes to the saved player class in the class manager. This is because player objects are destroyed between scenes
        if (playerId == 1)
        {
            currentPlayerClass = ClassManager._currentPlayer1Class;
        }
        else if (playerId == 2)
        {
            currentPlayerClass = ClassManager._currentPlayer2Class;
        }

        swingAbility = GetComponent<SwingAbility>();
        projectile = GetComponent<Projectile>();
        shieldAbility = GetComponent<ShieldAbility>();
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

            case PlayerClass.Melee:            

                break;

            case PlayerClass.Support:

                shieldAbility.UseAbility();

                break;

            case PlayerClass.Ranged:

                projectile.UseAbility();

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

    /// <summary>
    /// This method is used for setting the player class 
    /// </summary>
    /// <param name="newPlayerClass"></param>
    public void SetCurrentPlayerClass(PlayerClass newPlayerClass)
    {
        currentPlayerClass = newPlayerClass;

        OnPlayerClassChanged?.Invoke(newPlayerClass);
    }
}