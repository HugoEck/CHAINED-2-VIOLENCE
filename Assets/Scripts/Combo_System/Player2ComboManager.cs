using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2ComboManager : MonoBehaviour
{
    public static Player2ComboManager instance { get; private set; }

    [Header("Combat related")]
    [SerializeField] private UnarmedComboSOs _availableUnarmedCombos;

    [SerializeField] private PlayerCombat player2CombatScript;
    [SerializeField] private PlayerAttributes _player2Attributes;
    [SerializeField] private WeaponManager player2WeaponManager;

    private List<ComboAttackSO> _player2ComboAttacks; // Current weapon's combos

    [HideInInspector]
    public Weapon currentPlayer2Weapon;
    private GameObject _currentPlayer2WeaponObject; // Weapon defines its own ComboAttackSO list

    [HideInInspector]
    public Weapon.WeaponType currentEquippedPlayer2WeaponType;

    private GameObject[] weaponSlashEffects;

    [Header("Player animators")]
    [SerializeField] private Animator player2DefaultAnimator;
    [SerializeField] private Animator player2TankAnimator;
    [SerializeField] private Animator player2WarriorAnimator;
    [SerializeField] private Animator player2RangedAnimator;
    [SerializeField] private Animator player2SupportAnimator;

    private Animator currentAnimator;

    [HideInInspector]
    public ComboAttackSO[] player2UnarmedCombos;

    [HideInInspector]
    public bool bIsPlayer2Attacking = false;

    [HideInInspector]
    public string currentPlayer2ComboSubstate = "";

    [HideInInspector]
    public string currentPlayer2ComboInSequence = "";

    private bool bIsPlayer2Unarmed = true;

    private float weaponSlashSize;

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

        TriggerWeaponSlash();
        // Find all enemies within the attack range
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange + weaponSlashSize + 3);
        foreach (Collider enemy in hitEnemies)
        {
            float maxAngleCos = Mathf.Cos(90 * Mathf.Deg2Rad);

            // Calculate direction to the enemy
            Vector3 directionToEnemy = (enemy.transform.position - transform.position).normalized;

            // Check if enemy is within the cone
            if (Vector3.Dot(transform.forward, directionToEnemy) >= maxAngleCos)
            {
                BaseManager enemyManager = enemy.GetComponent<BaseManager>();
                if (enemyManager != null)
                {
                    // Deal damage if within cone
                    enemyManager.DealDamageToEnemy(attackDamage + _player2Attributes.attackDamage);
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

    private void ApplyWeaponSlashEffect(int comboIndex)
    {
        if (currentAnimator.GetBool("WeaponSlash"))
        {
            weaponSlashEffects[comboIndex].gameObject.transform.position = currentPlayer2Weapon.handPosition.position;
            ParticleSystem particle = weaponSlashEffects[comboIndex].GetComponent<ParticleSystem>();
            var mainModule = particle.main;
            mainModule.startSize = currentPlayer2Weapon.combos[comboIndex].attackRange;
            weaponSlashSize = mainModule.startSize.constant;

            particle.Play();
            player2DefaultAnimator.SetBool("WeaponSlash", false);
        }
    }
    private void TriggerWeaponSlash()
    {
        if (currentAnimator.GetInteger("ComboIndex") == 1)
        {
            ApplyWeaponSlashEffect(0);
        }
        else if (currentAnimator.GetInteger("ComboIndex") == 2)
        {
            ApplyWeaponSlashEffect(1);
        }
        if (currentAnimator.GetInteger("ComboIndex") == 3)
        {
            ApplyWeaponSlashEffect(2);
        }
        else if (currentAnimator.GetInteger("ComboIndex") == 4)
        {
            ApplyWeaponSlashEffect(3);
        }
        if (currentAnimator.GetInteger("ComboIndex") == 5)
        {
            ApplyWeaponSlashEffect(4);
        }
        if (currentAnimator.GetInteger("ComboIndex") == 6)
        {
            ApplyWeaponSlashEffect(5);
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
        SetAttackSpeed();
        SetUnarmedCombos();
    }

    private void WeaponManager_OnWeaponBrokenPlayer2(GameObject equippedWeapon)
    {
        currentPlayer2Weapon = null;
        _currentPlayer2WeaponObject = null;

        bIsPlayer2Unarmed = true;
        currentAnimator.SetInteger("ComboIndex", 0);
        DefaultCombo();
    }

    private void WeaponManager_OnWeaponEquippedPlayer2(GameObject equippedWeapon)
    {
        bIsPlayer2Unarmed = false;

        _currentPlayer2WeaponObject = equippedWeapon;
        currentAnimator.SetInteger("ComboIndex", 0);
        AssignWeaponCombos(_currentPlayer2WeaponObject.GetComponent<Weapon>());
    }

    public void AssignWeaponCombos(Weapon weapon)
    {

        currentPlayer2Weapon = weapon;

        currentEquippedPlayer2WeaponType = currentPlayer2Weapon.currentWeaponType;

        weaponSlashEffects = currentPlayer2Weapon.weaponSlashEffects;

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

        if (player2CombatScript.currentPlayerClass == PlayerCombat.PlayerClass.Default)
        {
            currentAnimator = player2DefaultAnimator;
            currentPlayer2ComboSubstate = ComboAnimationStatesData.unarmedSubStateDefault;
            player2DefaultAnimator.SetInteger("PlayerClass", (int)player2CombatScript.currentPlayerClass);
            player2UnarmedCombos = _availableUnarmedCombos.unarmedDefaultCombos;
        }
        else if (player2CombatScript.currentPlayerClass == PlayerCombat.PlayerClass.Tank)
        {
            currentAnimator = player2TankAnimator;
            currentPlayer2ComboSubstate = ComboAnimationStatesData.unarmedSubStateTank;
            player2TankAnimator.SetInteger("PlayerClass", (int)player2CombatScript.currentPlayerClass);
            player2UnarmedCombos = _availableUnarmedCombos.unarmedTankCombos;
        }
        else if (player2CombatScript.currentPlayerClass == PlayerCombat.PlayerClass.Warrior)
        {
            currentAnimator = player2WarriorAnimator;
            currentPlayer2ComboSubstate = ComboAnimationStatesData.unarmedSubStateWarrior;
            player2WarriorAnimator.SetInteger("PlayerClass", (int)player2CombatScript.currentPlayerClass);
            player2UnarmedCombos = _availableUnarmedCombos.unarmedWarriorCombos;
        }
        else if (player2CombatScript.currentPlayerClass == PlayerCombat.PlayerClass.Ranged)
        {
            currentAnimator = player2RangedAnimator;
            currentPlayer2ComboSubstate = ComboAnimationStatesData.unarmedSubStateRanged;
            player2RangedAnimator.SetInteger("PlayerClass", (int)player2CombatScript.currentPlayerClass);
            player2UnarmedCombos = _availableUnarmedCombos.unarmedRangedCombos;
        }
        else if (player2CombatScript.currentPlayerClass == PlayerCombat.PlayerClass.Support)
        {
            currentAnimator = player2SupportAnimator;
            currentPlayer2ComboSubstate = ComboAnimationStatesData.unarmedSubStateSupport;
            player2SupportAnimator.SetInteger("PlayerClass", (int)player2CombatScript.currentPlayerClass);
            player2UnarmedCombos = _availableUnarmedCombos.unarmedSupportCombos;
        }
    }
    private void SetAttackSpeed()
    {
        player2DefaultAnimator.SetFloat("AttackSpeed", _player2Attributes.attackSpeed);
        player2TankAnimator.SetFloat("AttackSpeed", _player2Attributes.attackSpeed);
        player2WarriorAnimator.SetFloat("AttackSpeed", _player2Attributes.attackSpeed);
        player2RangedAnimator.SetFloat("AttackSpeed", _player2Attributes.attackSpeed);
        player2SupportAnimator.SetFloat("AttackSpeed", _player2Attributes.attackSpeed);
    }
}
