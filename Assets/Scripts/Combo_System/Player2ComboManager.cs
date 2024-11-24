using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2ComboManager : MonoBehaviour
{
    public static Player2ComboManager instance { get; private set; }

    #region BOTH

    [Header("Both")]
    [SerializeField] private UnarmedComboSOs _availableUnarmedCombos;

    [SerializeField] private PlayerCombat playerCombatScript;

    #endregion

    #region PLAYER2

    [Header("Player 2")]
    [SerializeField] private GameObject _currentPlayer2WeaponObject; // Weapon defines its own ComboAttackSO list

    [HideInInspector]
    public ComboAttackSO[] player2UnarmedCombos;

    public Animator player2DefaultAnimator;
    public Animator player2TankAnimator;
    public Animator player2WarriorAnimator;
    public Animator player2RangedAnimator;
    public Animator player2SupportAnimator;


    [HideInInspector]
    public bool bIsPlayer2Attacking = false;

    [HideInInspector]
    private List<ComboAttackSO> _player2ComboAttacks; // Current weapon's combos

    public Weapon currentPlayer2Weapon;

    public WeaponManager player2WeaponManager;

    [HideInInspector]
    public Weapon.WeaponType currentEquippedPlayer2WeaponType;

    [HideInInspector]
    public string currentPlayer2ComboSubstate = "";
    [HideInInspector]
    public string currentPlayer2ComboInSequence = "";

    private bool bIsPlayer2Unarmed = true;
    #endregion
    private void Awake()
    {
        instance = this;

        player2WeaponManager.OnWeaponEquipped += WeaponManager_OnWeaponEquippedPlayer2;
        player2WeaponManager.OnWeaponBroken += WeaponManager_OnWeaponBrokenPlayer2;
    }



    private void Start()
    {
        DefaultCombo();
    }

    private void OnDestroy()
    {

        player2WeaponManager.OnWeaponEquipped -= WeaponManager_OnWeaponEquippedPlayer2;
        player2WeaponManager.OnWeaponBroken -= WeaponManager_OnWeaponBrokenPlayer2;
    }

    public void Attack()
    {

        if (!bIsPlayer2Attacking)
        {
            bIsPlayer2Attacking = true;
            player2DefaultAnimator.SetBool("NextAttack", true);
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
                    enemyManager.DealDamageToEnemy(attackDamage + playerCombatScript.attackDamage);
                    Debug.Log("Hit enemy: " + enemy.name);

                    if (_currentPlayer2WeaponObject != null)
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
        if (player2WeaponManager != null)
        {
            player2WeaponManager.ReduceWeaponDurability();

        }

    }


    private void Update()
    {
        player2DefaultAnimator.SetInteger("PlayerClass", (int)playerCombatScript.currentPlayerClass);

        SetUnarmedCombos();
    }

    private void WeaponManager_OnWeaponBrokenPlayer2(GameObject equippedWeapon)
    {
        currentPlayer2Weapon = null;
        _currentPlayer2WeaponObject = null;

        bIsPlayer2Unarmed = true;
        DefaultCombo();
    }

    private void WeaponManager_OnWeaponEquippedPlayer2(GameObject equippedWeapon)
    {
        bIsPlayer2Unarmed = false;

        _currentPlayer2WeaponObject = equippedWeapon;

        AssignWeaponCombos(_currentPlayer2WeaponObject.GetComponent<Weapon>());
    }

    public void AssignWeaponCombos(Weapon weapon)
    {

        currentPlayer2Weapon = weapon;

        currentEquippedPlayer2WeaponType = currentPlayer2Weapon.currentWeaponType;

        if (currentEquippedPlayer2WeaponType == Weapon.WeaponType.TwoHanded)
        {
            currentPlayer2ComboSubstate = ComboAnimationStatesData.twoHandedSubState;
        }
        else if (currentEquippedPlayer2WeaponType == Weapon.WeaponType.OneHanded)
        {
            currentPlayer2ComboSubstate = ComboAnimationStatesData.oneHandedSubState;
        }
        else if (currentEquippedPlayer2WeaponType == Weapon.WeaponType.ReallyBigTwoHanded)
        {
            currentPlayer2ComboSubstate = ComboAnimationStatesData.reallyBigTwoHandedSubState;
        }
        else if (currentEquippedPlayer2WeaponType == Weapon.WeaponType.Polearm)
        {
            currentPlayer2ComboSubstate = ComboAnimationStatesData.polearmSubState;
        }
        else if (currentEquippedPlayer2WeaponType == Weapon.WeaponType.Dagger)
        {
            currentPlayer2ComboSubstate = ComboAnimationStatesData.daggerSubState;
        }
        else if (currentEquippedPlayer2WeaponType == Weapon.WeaponType.BigPen)
        {
            currentPlayer2ComboSubstate = ComboAnimationStatesData.bigPenSubState;
        }
    }

    private void DefaultCombo()
    {
        currentEquippedPlayer2WeaponType = Weapon.WeaponType.Unarmed;

    }

    private void SetUnarmedCombos()
    {
        if (!bIsPlayer2Unarmed) return;

        if (playerCombatScript.currentPlayerClass == PlayerCombat.PlayerClass.Default)
        {
            currentPlayer2ComboSubstate = ComboAnimationStatesData.unarmedSubStateDefault;
            player2DefaultAnimator.SetInteger("PlayerClass", (int)playerCombatScript.currentPlayerClass);
            player2UnarmedCombos = _availableUnarmedCombos.unarmedDefaultCombos;
        }
        else if (playerCombatScript.currentPlayerClass == PlayerCombat.PlayerClass.Tank)
        {
            currentPlayer2ComboSubstate = ComboAnimationStatesData.unarmedSubStateTank;
            player2TankAnimator.SetInteger("PlayerClass", (int)playerCombatScript.currentPlayerClass);
            player2UnarmedCombos = _availableUnarmedCombos.unarmedTankCombos;
        }
        else if (playerCombatScript.currentPlayerClass == PlayerCombat.PlayerClass.Warrior)
        {
            currentPlayer2ComboSubstate = ComboAnimationStatesData.unarmedSubStateWarrior;
            player2WarriorAnimator.SetInteger("PlayerClass", (int)playerCombatScript.currentPlayerClass);
            player2UnarmedCombos = _availableUnarmedCombos.unarmedWarriorCombos;
        }
        else if (playerCombatScript.currentPlayerClass == PlayerCombat.PlayerClass.Ranged)
        {
            currentPlayer2ComboSubstate = ComboAnimationStatesData.unarmedSubStateRanged;
            player2RangedAnimator.SetInteger("PlayerClass", (int)playerCombatScript.currentPlayerClass);
            player2UnarmedCombos = _availableUnarmedCombos.unarmedRangedCombos;
        }
        else if (playerCombatScript.currentPlayerClass == PlayerCombat.PlayerClass.Support)
        {
            currentPlayer2ComboSubstate = ComboAnimationStatesData.unarmedSubStateSupport;
            player2SupportAnimator.SetInteger("PlayerClass", (int)playerCombatScript.currentPlayerClass);
            player2UnarmedCombos = _availableUnarmedCombos.unarmedSupportCombos;
        }
    }
}
