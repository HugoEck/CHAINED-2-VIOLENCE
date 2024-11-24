using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1ComboManager : MonoBehaviour
{
    public static Player1ComboManager instance { get; private set; }

    [Header("Combat related")]
    [SerializeField] private UnarmedComboSOs _availableUnarmedCombos;

    [SerializeField] private PlayerCombat player1CombatScript;
    [SerializeField] private PlayerAttributes _player1Attributes;
    [SerializeField] private WeaponManager player1WeaponManager;

    private List<ComboAttackSO> _player1ComboAttacks; // Current weapon's combos

    [HideInInspector]
    public Weapon currentPlayer1Weapon;
    private GameObject _currentPlayer1WeaponObject; // Weapon defines its own ComboAttackSO list

    [HideInInspector]
    public Weapon.WeaponType currentEquippedPlayer1WeaponType;

    [Header("Player animators")]
    [SerializeField] private Animator player1DefaultAnimator;
    [SerializeField] private Animator player1TankAnimator;
    [SerializeField] private Animator player1WarriorAnimator;
    [SerializeField] private Animator player1RangedAnimator;
    [SerializeField] private Animator player1SupportAnimator;
    

    [HideInInspector]
    public ComboAttackSO[] player1UnarmedCombos;

    [HideInInspector]
    public bool bIsPlayer1Attacking = false;

    [HideInInspector]
    public string currentPlayer1ComboSubstate = "";

    [HideInInspector]
    public string currentPlayer1ComboInSequence = "";

    private bool bIsPlayer1Unarmed = true;

    private void Awake()
    {
        instance = this;

        player1WeaponManager.OnWeaponEquipped += WeaponManager_OnWeaponEquippedPlayer1;
        player1WeaponManager.OnWeaponBroken += WeaponManager_OnWeaponBrokenPlayer1;
    }



    private void Start()
    {
        DefaultCombo();
    }

    private void OnDestroy()
    {
        player1WeaponManager.OnWeaponEquipped -= WeaponManager_OnWeaponEquippedPlayer1;
        player1WeaponManager.OnWeaponBroken -= WeaponManager_OnWeaponBrokenPlayer1;
    }

    public void Attack()
    {

        if (!bIsPlayer1Attacking)
        {
            bIsPlayer1Attacking = true;
        }

    }

    public void DealDamageToEnemies(float attackRange, float attackDamage)
    {
        bool durabilityReduced = false;
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
                    enemyManager.DealDamageToEnemy(attackDamage + _player1Attributes.attackDamage);
                    Debug.Log("Hit enemy: " + enemy.name);

                    if(_currentPlayer1WeaponObject != null)
                    {
                        if (!durabilityReduced)
                        {
                            ReduceWeaponDurabilility();
                            durabilityReduced = true;
                        }
                    }
                    
                }
            }
        }
    }

    private void ReduceWeaponDurabilility()
    {
        if (player1WeaponManager != null)
        {
            player1WeaponManager.ReduceWeaponDurability();

        }

    }


    private void Update()
    {
        
        SetUnarmedCombos();
    }

    private void WeaponManager_OnWeaponBrokenPlayer1(GameObject equippedWeapon)
    {
        currentPlayer1Weapon = null;
        _currentPlayer1WeaponObject = null;

        bIsPlayer1Unarmed = true;
        DefaultCombo();
    }

    private void WeaponManager_OnWeaponEquippedPlayer1(GameObject equippedWeapon)
    {
        bIsPlayer1Unarmed = false;

        _currentPlayer1WeaponObject = equippedWeapon;

        AssignWeaponCombos(_currentPlayer1WeaponObject.GetComponent<Weapon>());
    }

    public void AssignWeaponCombos(Weapon weapon)
    {

        currentPlayer1Weapon = weapon;

        currentEquippedPlayer1WeaponType = currentPlayer1Weapon.currentWeaponType;

        if (currentEquippedPlayer1WeaponType == Weapon.WeaponType.TwoHanded)
        {
            currentPlayer1ComboSubstate = ComboAnimationStatesData.twoHandedSubState;
        }
        else if(currentEquippedPlayer1WeaponType == Weapon.WeaponType.OneHanded)
        {
            currentPlayer1ComboSubstate = ComboAnimationStatesData.oneHandedSubState;
        }
        else if(currentEquippedPlayer1WeaponType == Weapon.WeaponType.ReallyBigTwoHanded)
        {
            currentPlayer1ComboSubstate = ComboAnimationStatesData.reallyBigTwoHandedSubState;
        }
        else if (currentEquippedPlayer1WeaponType == Weapon.WeaponType.Polearm)
        {
            currentPlayer1ComboSubstate = ComboAnimationStatesData.polearmSubState;
        }
        else if (currentEquippedPlayer1WeaponType == Weapon.WeaponType.Dagger)
        {
            currentPlayer1ComboSubstate = ComboAnimationStatesData.daggerSubState;
        }
        else if (currentEquippedPlayer1WeaponType == Weapon.WeaponType.BigPen)
        {
            currentPlayer1ComboSubstate = ComboAnimationStatesData.bigPenSubState;
        }
    }

    private void DefaultCombo()
    {       
        currentEquippedPlayer1WeaponType = Weapon.WeaponType.Unarmed;
    }

    private void SetUnarmedCombos()
    {
        if (!bIsPlayer1Unarmed) return;

        if (player1CombatScript.currentPlayerClass == PlayerCombat.PlayerClass.Default)
        {       
            currentPlayer1ComboSubstate = ComboAnimationStatesData.unarmedSubStateDefault;
            player1DefaultAnimator.SetInteger("PlayerClass", (int)player1CombatScript.currentPlayerClass);
            player1UnarmedCombos = _availableUnarmedCombos.unarmedDefaultCombos;
        }
        else if (player1CombatScript.currentPlayerClass == PlayerCombat.PlayerClass.Tank)
        {
            currentPlayer1ComboSubstate = ComboAnimationStatesData.unarmedSubStateTank;
            player1TankAnimator.SetInteger("PlayerClass", (int)player1CombatScript.currentPlayerClass);
            player1UnarmedCombos = _availableUnarmedCombos.unarmedTankCombos;         
        }
        else if (player1CombatScript.currentPlayerClass == PlayerCombat.PlayerClass.Warrior)
        {
            currentPlayer1ComboSubstate = ComboAnimationStatesData.unarmedSubStateWarrior;
            player1WarriorAnimator.SetInteger("PlayerClass", (int)player1CombatScript.currentPlayerClass);
            player1UnarmedCombos = _availableUnarmedCombos.unarmedWarriorCombos;
        }
        else if (player1CombatScript.currentPlayerClass == PlayerCombat.PlayerClass.Ranged)
        {
            currentPlayer1ComboSubstate = ComboAnimationStatesData.unarmedSubStateRanged;
            player1RangedAnimator.SetInteger("PlayerClass", (int)player1CombatScript.currentPlayerClass);
            player1UnarmedCombos = _availableUnarmedCombos.unarmedRangedCombos;
        }
        else if (player1CombatScript.currentPlayerClass == PlayerCombat.PlayerClass.Support)
        {
            currentPlayer1ComboSubstate = ComboAnimationStatesData.unarmedSubStateSupport;
            player1SupportAnimator.SetInteger("PlayerClass", (int)player1CombatScript.currentPlayerClass);
            player1UnarmedCombos = _availableUnarmedCombos.unarmedSupportCombos;
        }      
    }
}
