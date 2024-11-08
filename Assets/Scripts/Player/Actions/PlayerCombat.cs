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
        Default,
        Tank,
        Warrior,
        Support,
        Ranged
    };

    [Header("Class reference objects")]
    [SerializeField] private GameObject _defaultObject;
    [SerializeField] private GameObject _tankObject;
    [SerializeField] private GameObject _supportObject;
    [SerializeField] private GameObject _rangedObject;
    [SerializeField] private GameObject _warriorObject;


    public PlayerClass currentPlayerClass;   

    public float attackCooldown = 1f;  // Cooldown between attacks
    public float abilityCooldown = 5f; // Cooldown between abilities
    public float attackRange = 2f;     // Range of attack
    public float attackDamage = 10f;   // Damage per attack

    protected float lastAttackTime;
    protected float lastAbilityTime;

    private int playerId;

    
    #region Ability components

    private SwingAbility swingAbility;
    private Projectile projectile;
    private ShieldAbility shieldAbility;
    private ConeAbility coneAbility;

    #endregion

    private void Start()
    {
        playerId = gameObject.GetComponent<Player>()._playerId;

        // Set the player classes to the saved player class in the class manager. This is because player objects are destroyed between scenes
        if (playerId == 1)
        {
            currentPlayerClass = ClassManager._currentPlayer1Class;
            SetActiveClassModel(currentPlayerClass);
        }
        else if (playerId == 2)
        {
            currentPlayerClass = ClassManager._currentPlayer2Class;
            SetActiveClassModel(currentPlayerClass);
        }

        swingAbility = GetComponent<SwingAbility>();
        projectile = GetComponent<Projectile>();
        shieldAbility = GetComponent<ShieldAbility>();
        coneAbility = GetComponent<ConeAbility>();
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

            case PlayerClass.Warrior:

                coneAbility.UseAbility();

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
        if (playerId == 1)
        {
            if (newPlayerClass == ClassManager._currentPlayer2Class) return;
        }
        else if (playerId == 2)
        {
            if (newPlayerClass == ClassManager._currentPlayer1Class) return;
        }
       
        currentPlayerClass = newPlayerClass;
       
        if (playerId == 1)
        {
            ClassManager._currentPlayer1Class = newPlayerClass;
        }
        else if (playerId == 2)
        {
            ClassManager._currentPlayer2Class = newPlayerClass;
        }
            
    }
    private void SetActiveClassModel(PlayerClass currentPlayerClass)
    {
        
        if (currentPlayerClass == PlayerClass.Tank)
        {
            _defaultObject.SetActive(false);

            _rangedObject.SetActive(false);
            _supportObject.SetActive(false);
            _warriorObject.SetActive(false);

            _tankObject.SetActive(true);
        }
        else if(currentPlayerClass == PlayerClass.Ranged)
        {
            _defaultObject.SetActive(false);

            _supportObject.SetActive(false);
            _warriorObject.SetActive(false);
            _tankObject.SetActive(false);

            _rangedObject.SetActive(true);
        }
        else if(currentPlayerClass == PlayerClass.Warrior)
        {
            _defaultObject.SetActive(false);

            _supportObject.SetActive(false);
            _tankObject.SetActive(false);
            _rangedObject.SetActive(false);

            _warriorObject.SetActive(true);
        }
        else if(currentPlayerClass == PlayerClass.Support)
        {
            _defaultObject.SetActive(false);

            _tankObject.SetActive(false);
            _rangedObject.SetActive(false);
            _warriorObject.SetActive(false);

            _supportObject.SetActive(true);
        }
    }

}