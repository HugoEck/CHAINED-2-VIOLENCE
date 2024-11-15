using System;
using System.Collections;
using System.Collections.Generic;
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

    private WeaponManager _weaponManager;

    public PlayerClass currentPlayerClass;   

    public float attackCooldown = 1f;  // Cooldown between attacks
    public float abilityCooldown = 5f; // Cooldown between abilities
    public float attackRange = 2f;     // Range of attack
    public float attackDamage = 10f;   // Damage per attack
    public float InitialAttackDamage { get; private set; }

    protected float lastAttackTime;
    protected float lastAbilityTime;

    ///Variables for allowing attack
    private float _lastAttackedTime = 0f;
    private float _attackSpeed = 1f;

    private int playerId;

    private ClassSelector classSelector;
    
    #region Ability components

    private SwingAbility swingAbility;
    private Projectile projectile;
    private ShieldAbility shieldAbility;
    private ConeAbility coneAbility;

    #endregion
   
private void Awake()
{
    classSelector = GetComponent<ClassSelector>();
    if (classSelector == null)
    {
        Debug.LogError("ClassSelector not found in the scene.");
        return;
    }

    Debug.Log($"{gameObject.name} is subscribing to OnClassSwitched");
    classSelector.OnClassSwitched += ClassSelectorOnClassSwitched;
}


    private void Start()
    {
        playerId = gameObject.GetComponent<Player>()._playerId;
        InitialAttackDamage = attackDamage;
        // Set the player classes to the saved player class in the class manager. This is because player objects are destroyed between scenes
        if (playerId == 1)
        {
            attackDamage = StatsTransfer.Player1AttackDamage > 0 ? StatsTransfer.Player1AttackDamage : attackDamage;
            currentPlayerClass = ClassManager._currentPlayer1Class;
            SetActiveClassModel(currentPlayerClass);
        }
        else if (playerId == 2)
        {
            attackDamage = StatsTransfer.Player2AttackDamage > 0 ? StatsTransfer.Player2AttackDamage : attackDamage;
            currentPlayerClass = ClassManager._currentPlayer2Class;
            SetActiveClassModel(currentPlayerClass);
        }

        swingAbility = GetComponent<SwingAbility>();
        projectile = GetComponent<Projectile>();
        shieldAbility = GetComponent<ShieldAbility>();
        coneAbility = GetComponent<ConeAbility>();
    }
    private void OnDestroy()
    {
        classSelector.OnClassSwitched -= ClassSelectorOnClassSwitched;
    }

    private void ClassSelectorOnClassSwitched(GameObject player, PlayerClass targetClass)
    {
        string activeClass = targetClass.ToString();

        AnimationStateController animator = player.GetComponent<AnimationStateController>();
        WeaponManager _weaponManager = player.GetComponent<WeaponManager>();

        Transform findActiveClass = player.transform.Find("Classes:").Find(activeClass);

        if (animator != null && findActiveClass != null)
        {
            animator._animator = findActiveClass.GetComponentInChildren<Animator>();
            animator._animator.Rebind();
        }

        if (_weaponManager != null)
        {
            _weaponManager.OnClassSwitch(targetClass);
        }

        Debug.Log($"Class switched to {targetClass} for player: {player.name}");
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
            // Calculate the direction to the enemy
            Vector3 directionToEnemy = (enemy.transform.position - transform.position).normalized;

            // Check if the enemy is within the 120-degree cone
            if (Vector3.Angle(transform.forward, directionToEnemy) <= 60) // 60 degrees on each side
            {
                BaseManager enemyManager = enemy.GetComponent<BaseManager>();
                if (enemyManager != null)
                {
                    // Deal damage if within cone
                    enemyManager.DealDamageToEnemy(attackDamage);
                    Debug.Log("Hit enemy: " + enemy.name);
                }
            }
        }

        Debug.Log("Base Attack triggered in 120-degree cone.");
    }
    public bool IsAttackAllowed()
    {
        if (Time.time > _lastAttackedTime + _attackSpeed)
        {
            _lastAttackedTime = Time.time;
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// This method uses the ability that the player has for its class (Called in Player script)
    /// </summary>
    public void UseAbility()
    {
        float cooldown = 0f;

        switch (currentPlayerClass)
        {

            case PlayerClass.Warrior:

                coneAbility.UseAbility();
                cooldown = coneAbility.cooldown;
                break;

            case PlayerClass.Support:

                shieldAbility.UseAbility();

                if (shieldAbility.IsShieldActive() == false)
                {
                    cooldown = shieldAbility.cooldown;
                }

                break;

            case PlayerClass.Ranged:

                projectile.UseAbility();
                cooldown = projectile.cooldown;

                break;

            case PlayerClass.Tank:

                swingAbility.UseAbility();
                cooldown = swingAbility.cooldown;
                break;

        }
        AbilityCdEventsUI.AbilityUsed(playerId, currentPlayerClass, cooldown);
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
           // _weaponManager.OnClassSwitch(PlayerCombat.PlayerClass.Tank);


        }
        else if(currentPlayerClass == PlayerClass.Ranged)
        {
            _defaultObject.SetActive(false);

            _supportObject.SetActive(false);
            _warriorObject.SetActive(false);
            _tankObject.SetActive(false);

            _rangedObject.SetActive(true);
  //     _weaponManager.OnClassSwitch(PlayerCombat.PlayerClass.Ranged);
        }
        else if(currentPlayerClass == PlayerClass.Warrior)
        {
            _defaultObject.SetActive(false);

            _supportObject.SetActive(false);
            _tankObject.SetActive(false);
            _rangedObject.SetActive(false);

            _warriorObject.SetActive(true);
        //    _weaponManager.OnClassSwitch(PlayerCombat.PlayerClass.Warrior);
        }
        else if(currentPlayerClass == PlayerClass.Support)
        {
            _defaultObject.SetActive(false);

            _tankObject.SetActive(false);
            _rangedObject.SetActive(false);
            _warriorObject.SetActive(false);

            _supportObject.SetActive(true);
         //   _weaponManager.OnClassSwitch(PlayerCombat.PlayerClass.Support);
        }



    }

}