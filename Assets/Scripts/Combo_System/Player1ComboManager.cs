using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1ComboManager : MonoBehaviour
{
    public static Player1ComboManager instance { get; private set; }

    #region BOTH

    [Header("Both")]
    [SerializeField] private UnarmedComboSOs _availableUnarmedCombos;

    [SerializeField] private PlayerCombat playerCombatScript;

    #endregion

    #region PLAYER1

    [Header("Player 1")]
    [SerializeField] private GameObject _currentPlayer1WeaponObject; // Weapon defines its own ComboAttackSO list

    [HideInInspector]
    public ComboAttackSO[] player1UnarmedCombos;

    public Animator player1DefaultAnimator;
    public Animator player1TankAnimator;
    public Animator player1WarriorAnimator;
    public Animator player1RangedAnimator;
    public Animator player1SupportAnimator;

    [HideInInspector]
    public bool bIsPlayer1Attacking = false;

    [HideInInspector]
    private List<ComboAttackSO> _player1ComboAttacks; // Current weapon's combos

    public Weapon currentPlayer1Weapon;

    public WeaponManager player1WeaponManager;

    [HideInInspector]
    public Weapon.WeaponType currentEquippedPlayer1WeaponType;

    [HideInInspector]
    public string currentPlayer1ComboSubstate = "";

    [HideInInspector]
    public string currentPlayer1ComboInSequence = "";

    private bool bIsPlayer1Unarmed = true;

    #endregion
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
                    enemyManager.DealDamageToEnemy(attackDamage);
                    Debug.Log("Hit enemy: " + enemy.name);

                    if (!durabilityReduced)
                    {
                        ReduceWeaponDurabilility();
                        durabilityReduced = true;
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

        if (playerCombatScript.currentPlayerClass == PlayerCombat.PlayerClass.Default)
        {       
            currentPlayer1ComboSubstate = ComboAnimationStatesData.unarmedSubStateDefault;
            player1DefaultAnimator.SetInteger("PlayerClass", (int)playerCombatScript.currentPlayerClass);
            player1UnarmedCombos = _availableUnarmedCombos.unarmedDefaultCombos;
        }
        else if (playerCombatScript.currentPlayerClass == PlayerCombat.PlayerClass.Tank)
        {
            currentPlayer1ComboSubstate = ComboAnimationStatesData.unarmedSubStateTank;
            player1TankAnimator.SetInteger("PlayerClass", (int)playerCombatScript.currentPlayerClass);
            player1UnarmedCombos = _availableUnarmedCombos.unarmedTankCombos;         
        }
        else if (playerCombatScript.currentPlayerClass == PlayerCombat.PlayerClass.Warrior)
        {
            currentPlayer1ComboSubstate = ComboAnimationStatesData.unarmedSubStateWarrior;
            player1WarriorAnimator.SetInteger("PlayerClass", (int)playerCombatScript.currentPlayerClass);
            player1UnarmedCombos = _availableUnarmedCombos.unarmedWarriorCombos;
        }
        else if (playerCombatScript.currentPlayerClass == PlayerCombat.PlayerClass.Ranged)
        {
            currentPlayer1ComboSubstate = ComboAnimationStatesData.unarmedSubStateRanged;
            player1RangedAnimator.SetInteger("PlayerClass", (int)playerCombatScript.currentPlayerClass);
            player1UnarmedCombos = _availableUnarmedCombos.unarmedRangedCombos;
        }
        else if (playerCombatScript.currentPlayerClass == PlayerCombat.PlayerClass.Support)
        {
            currentPlayer1ComboSubstate = ComboAnimationStatesData.unarmedSubStateSupport;
            player1SupportAnimator.SetInteger("PlayerClass", (int)playerCombatScript.currentPlayerClass);
            player1UnarmedCombos = _availableUnarmedCombos.unarmedSupportCombos;
        }

       
    }
}
