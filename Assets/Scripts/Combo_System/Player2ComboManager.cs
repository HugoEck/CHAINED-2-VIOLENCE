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

    public Animator player2Animator;

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
            player2Animator.SetBool("NextAttack", true);
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
        if (player2WeaponManager != null)
        {
            player2WeaponManager.ReduceWeaponDurability();

        }

    }


    private void Update()
    {
        player2Animator.SetInteger("PlayerClass", (int)playerCombatScript.currentPlayerClass);

        SetUnarmedCombos();
    }

    private void WeaponManager_OnWeaponBrokenPlayer2(GameObject equippedWeapon)
    {
        currentPlayer2Weapon = null;
        _currentPlayer2WeaponObject = null;

        DefaultCombo();
    }

    private void WeaponManager_OnWeaponEquippedPlayer2(GameObject equippedWeapon)
    {
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

        currentPlayer2ComboSubstate = "Base Layer.Attack Combos.Unarmed Default";
        currentEquippedPlayer2WeaponType = Weapon.WeaponType.Unarmed;

    }

    private void SetUnarmedCombos()
    {

        if (playerCombatScript.currentPlayerClass == PlayerCombat.PlayerClass.Default)
        {
            player2UnarmedCombos = _availableUnarmedCombos.unarmedDefaultCombos;
        }
        else if (playerCombatScript.currentPlayerClass == PlayerCombat.PlayerClass.Tank)
        {
            player2UnarmedCombos = _availableUnarmedCombos.unarmedTankCombos;
        }
        else if (playerCombatScript.currentPlayerClass == PlayerCombat.PlayerClass.Warrior)
        {
            player2UnarmedCombos = _availableUnarmedCombos.unarmedWarriorCombos;
        }
        else if (playerCombatScript.currentPlayerClass == PlayerCombat.PlayerClass.Ranged)
        {
            player2UnarmedCombos = _availableUnarmedCombos.unarmedRangedCombos;
        }
        else if (playerCombatScript.currentPlayerClass == PlayerCombat.PlayerClass.Support)
        {
            player2UnarmedCombos = _availableUnarmedCombos.unarmedSupportCombos;
        }
    }
}
