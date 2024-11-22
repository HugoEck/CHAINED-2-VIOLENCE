using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1ComboManager : MonoBehaviour
{
    //public static Player1ComboManager instance { get; private set; }

    //#region BOTH

    //[Header("Both")]
    //[SerializeField] private UnarmedComboSOs _availableUnarmedCombos;

    //[SerializeField] private PlayerCombat playerCombatScript;

    //#endregion

    //#region PLAYER1

    //[Header("Player 1")]
    //[SerializeField] private GameObject _currentPlayer1WeaponObject; // Weapon defines its own ComboAttackSO list

    //[HideInInspector]
    //public ComboAttackSO[] player1UnarmedCombos;

    //public Animator player1Animator;

    //[HideInInspector]
    //public bool bIsPlayer1Attacking = false;

    //[HideInInspector]
    //private List<ComboAttackSO> _player1ComboAttacks; // Current weapon's combos

    //public Weapon currentPlayer1Weapon;

    //public WeaponManager player1WeaponManager;

    //[HideInInspector]
    //public Weapon.WeaponType currentEquippedPlayer1WeaponType;

    //[HideInInspector]
    //public string currentPlayer1ComboSubstate = "";

    //[HideInInspector]
    //public string currentPlayer1ComboInSequence = "";

    //#endregion
    //private void Awake()
    //{
    //    instance = this;

    //    player1WeaponManager.OnWeaponEquipped += WeaponManager_OnWeaponEquippedPlayer1;
    //    player1WeaponManager.OnWeaponBroken += WeaponManager_OnWeaponBrokenPlayer1;
    //}



    //private void Start()
    //{
    //    DefaultCombo();
    //}

    //private void OnDestroy()
    //{
    //    player1WeaponManager.OnWeaponEquipped -= WeaponManager_OnWeaponEquippedPlayer1;
    //    player1WeaponManager.OnWeaponBroken -= WeaponManager_OnWeaponBrokenPlayer1;
    //}

    //public void Attack()
    //{

    //    if (!bIsPlayer1Attacking)
    //    {
    //        bIsPlayer1Attacking = true;
    //        //player1Animator.SetBool("NextAttack", true);
    //    }

    //}

    //public void DealDamageToEnemies(float attackRange, float attackDamage)
    //{
    //    //bool durabilityReduced = false;
    //    // Find all enemies within the attack range
    //    Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange);
    //    foreach (Collider enemy in hitEnemies)
    //    {
    //        // Calculate the direction to the enemy
    //        Vector3 directionToEnemy = (enemy.transform.position - transform.position).normalized;

    //        // Check if the enemy is within the 120-degree cone
    //        if (Vector3.Angle(transform.forward, directionToEnemy) <= 60) // 60 degrees on each side
    //        {
    //            BaseManager enemyManager = enemy.GetComponent<BaseManager>();
    //            if (enemyManager != null)
    //            {
    //                // Deal damage if within cone
    //                enemyManager.DealDamageToEnemy(attackDamage);
    //                Debug.Log("Hit enemy: " + enemy.name);

    //                //if (!durabilityReduced)
    //                //{
    //                //    ReduceWeaponDurabilility();
    //                //    durabilityReduced = true;
    //                //}
    //            }
    //        }
    //    }
    //}

    //private void Update()
    //{
    //    player1Animator.SetInteger("PlayerClass", (int)playerCombatScript.currentPlayerClass);

    //    SetUnarmedCombos();
    //}

    //private void WeaponManager_OnWeaponBrokenPlayer1(GameObject equippedWeapon)
    //{
    //    currentPlayer1Weapon = null;
    //    _currentPlayer1WeaponObject = null;

    //    DefaultCombo();
    //}

    //private void WeaponManager_OnWeaponEquippedPlayer1(GameObject equippedWeapon)
    //{
    //    _currentPlayer1WeaponObject = equippedWeapon;

    //    AssignWeaponCombos(_currentPlayer1WeaponObject.GetComponent<Weapon>());
    //}

    //public void AssignWeaponCombos(Weapon weapon)
    //{

    //    currentPlayer1Weapon = weapon;

    //    currentEquippedPlayer1WeaponType = currentPlayer1Weapon.currentWeaponType;

    //    if (currentEquippedPlayer1WeaponType == Weapon.WeaponType.TwoHanded)
    //    {
    //        currentPlayer1ComboSubstate = "Base Layer.Attack Combos.Two Handed";
    //    }
    //}

    //private void DefaultCombo()
    //{
    //    currentPlayer1ComboSubstate = "Base Layer.Attack Combos.Unarmed Default";
    //    currentEquippedPlayer1WeaponType = Weapon.WeaponType.Unarmed;
    //}

    //private void SetUnarmedCombos()
    //{
    //    if (playerCombatScript.currentPlayerClass == PlayerCombat.PlayerClass.Default)
    //    {
    //        player1UnarmedCombos = _availableUnarmedCombos.unarmedDefaultCombos;
    //    }
    //    else if (playerCombatScript.currentPlayerClass == PlayerCombat.PlayerClass.Tank)
    //    {
    //        player1UnarmedCombos = _availableUnarmedCombos.unarmedTankCombos;
    //    }
    //    else if (playerCombatScript.currentPlayerClass == PlayerCombat.PlayerClass.Warrior)
    //    {
    //        player1UnarmedCombos = _availableUnarmedCombos.unarmedWarriorCombos;
    //    }
    //    else if (playerCombatScript.currentPlayerClass == PlayerCombat.PlayerClass.Ranged)
    //    {
    //        player1UnarmedCombos = _availableUnarmedCombos.unarmedRangedCombos;
    //    }
    //    else if (playerCombatScript.currentPlayerClass == PlayerCombat.PlayerClass.Support)
    //    {
    //        player1UnarmedCombos = _availableUnarmedCombos.unarmedSupportCombos;
    //    }
    //}
}
