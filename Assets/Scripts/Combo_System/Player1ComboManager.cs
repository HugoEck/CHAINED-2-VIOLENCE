using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1ComboManager : MonoBehaviour
{
    public static Player1ComboManager instance { get; private set; }

    [Header("Combat related")]
    [SerializeField] private UnarmedComboSOs _availableUnarmedCombos;

    [SerializeField] private PlayerCombat player1Combat;
    [SerializeField] private PlayerAttributes _player1Attributes;
    [SerializeField] private WeaponManager player1WeaponManager;

    [Header("Unarmed slash effect")]
    [SerializeField] private GameObject[] _defaultSlashes;
    [SerializeField] private GameObject[] _tankSlashes;
    [SerializeField] private GameObject[] _warriorSlashes;
    [SerializeField] private GameObject[] _rangedSlashes;
    [SerializeField] private GameObject[] _supportSlashes;

    public PlayerCombat.PlayerClass currentPlayer1Class { get; private set; }

    private List<ComboAttackSO> _player1ComboAttacks; // Current weapon's combos

    [HideInInspector]
    public Weapon currentPlayer1Weapon;
    private GameObject _currentPlayer1WeaponObject; // Weapon defines its own ComboAttackSO list

    [HideInInspector]
    public Weapon.WeaponType currentEquippedPlayer1WeaponType;

    private GameObject[] weaponSlashEffects;

    [Header("Player animators")]
    [SerializeField] private Animator player1DefaultAnimator;
    [SerializeField] private Animator player1TankAnimator;
    [SerializeField] private Animator player1WarriorAnimator;
    [SerializeField] private Animator player1RangedAnimator;
    [SerializeField] private Animator player1SupportAnimator;

    private Animator currentAnimator;

    [HideInInspector]
    public ComboAttackSO[] player1UnarmedCombos;

    [HideInInspector]
    public bool bIsPlayer1Attacking = false;

    [HideInInspector]
    public string currentPlayer1ComboSubstate = "";

    [HideInInspector]
    public string currentPlayer1ComboInSequence = "";

    private float weaponSlashSize;

    private void Awake()
    {
        instance = this;

        player1WeaponManager.OnWeaponEquipped += WeaponManager_OnWeaponEquippedPlayer1;
        player1WeaponManager.OnWeaponBroken += WeaponManager_OnWeaponBrokenPlayer1;
        player1Combat.OnClassSwitched += PlayerCombatOnClassSwitched;

        if(ClassManager._currentPlayer1Class == PlayerCombat.PlayerClass.Default)
        {
            currentAnimator = player1DefaultAnimator;
            currentPlayer1Class = PlayerCombat.PlayerClass.Default;
        }
        else if (ClassManager._currentPlayer1Class == PlayerCombat.PlayerClass.Tank)
        {
            currentAnimator = player1TankAnimator;
            currentPlayer1Class = PlayerCombat.PlayerClass.Tank;
        }
        else if (ClassManager._currentPlayer1Class == PlayerCombat.PlayerClass.Warrior)
        {
            currentAnimator = player1WarriorAnimator;
            currentPlayer1Class = PlayerCombat.PlayerClass.Warrior;
        }
        else if (ClassManager._currentPlayer1Class == PlayerCombat.PlayerClass.Ranged)
        {
            currentAnimator = player1RangedAnimator;
            currentPlayer1Class = PlayerCombat.PlayerClass.Ranged;
        }
        else if (ClassManager._currentPlayer1Class == PlayerCombat.PlayerClass.Support)
        {
            currentAnimator = player1SupportAnimator;
            currentPlayer1Class = PlayerCombat.PlayerClass.Support;
        }
        
    }

    

    private void Start()
    {
        
        DefaultCombo();
    }

    private void OnDestroy()
    {
        player1WeaponManager.OnWeaponEquipped -= WeaponManager_OnWeaponEquippedPlayer1;
        player1WeaponManager.OnWeaponBroken -= WeaponManager_OnWeaponBrokenPlayer1;
        player1Combat.OnClassSwitched -= PlayerCombatOnClassSwitched;
    }

    public void Attack()
    {

        if (!bIsPlayer1Attacking)
        {         
            bIsPlayer1Attacking = true;
        }
    }

    public void DealDamageToEnemies(float attackRange, float attackDamage, float stunDuration, float knockbackForce, float maxAngle)
    {
        bool durabilityReduced = false;

        TriggerWeaponSlash();
        // Find all enemies within the attack range
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange + weaponSlashSize + 3);
        foreach (Collider enemy in hitEnemies)
        {
            float maxAngleCos = Mathf.Cos(maxAngle * Mathf.Deg2Rad);

            // Calculate direction to the enemy
            Vector3 directionToEnemy = (enemy.transform.position - transform.position).normalized;

            // Check if enemy is within the cone
            if (Vector3.Dot(transform.forward, directionToEnemy) >= maxAngleCos)
            {
                BaseManager enemyManager = enemy.GetComponent<BaseManager>();
                if (enemyManager != null)
                {
                    // Deal damage if within cone
                    if(currentPlayer1Weapon != null)
                    {
                        enemyManager.DealDamageToEnemy(attackDamage + _player1Attributes.attackDamage, BaseManager.DamageType.WeaponDamage);
                    }
                    else
                    {
                        enemyManager.DealDamageToEnemy(attackDamage + _player1Attributes.attackDamage, BaseManager.DamageType.UnarmedDamage);
                    }
                    
                    enemyManager.chainEffects.ActivateKnockbackStun(stunDuration, gameObject, knockbackForce);

                    if (_currentPlayer1WeaponObject != null)
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
        
        SetAttackSpeed();
    }

    private void ApplyWeaponSlashEffect(int comboIndex)
    {
        if (currentAnimator.GetBool("WeaponSlash"))
        {
            if(currentPlayer1Weapon != null)
            {
                weaponSlashEffects[comboIndex].gameObject.transform.position = currentPlayer1Weapon.playerPosition.position;

                ParticleSystem particle = weaponSlashEffects[comboIndex].GetComponent<ParticleSystem>();
                var mainModule = particle.main;
                mainModule.startSize = currentPlayer1Weapon.combos[comboIndex].attackRange;
                weaponSlashSize = mainModule.startSize.constant;

                particle.Play();
                currentAnimator.SetBool("WeaponSlash", false);
            }
            else
            {
                if(currentPlayer1Class != PlayerCombat.PlayerClass.Default)
                {
                    ParticleSystem particle = weaponSlashEffects[comboIndex].GetComponent<ParticleSystem>();
                    var mainModule = particle.main;
                    mainModule.startSize = player1UnarmedCombos[comboIndex].attackRange;
                    weaponSlashSize = mainModule.startSize.constant;

                    particle.Play();
                    currentAnimator.SetBool("WeaponSlash", false);
                }
                
            }
            
            
        }       
    }
    private void TriggerWeaponSlash()
    {
        if(currentAnimator.GetInteger("ComboIndex") == 1)
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

    private void WeaponManager_OnWeaponBrokenPlayer1(GameObject equippedWeapon)
    {
        currentPlayer1Weapon = null;
        _currentPlayer1WeaponObject = null;

        currentAnimator.SetInteger("ComboIndex", 0);
        DefaultCombo();
        
    }

    private void WeaponManager_OnWeaponEquippedPlayer1(GameObject equippedWeapon)
    {
        _currentPlayer1WeaponObject = equippedWeapon;

        currentAnimator.SetInteger("ComboIndex", 0);

        AssignWeaponCombos(_currentPlayer1WeaponObject.GetComponent<Weapon>());
    }
    private void PlayerCombatOnClassSwitched(PlayerCombat.PlayerClass newClass)
    {
       
        if (newClass == PlayerCombat.PlayerClass.Default)
        {
            currentAnimator = player1DefaultAnimator;
            currentPlayer1ComboSubstate = ComboAnimationStatesData.unarmedSubStateDefault;
            player1UnarmedCombos = _availableUnarmedCombos.unarmedDefaultCombos;
        }
        else if (newClass == PlayerCombat.PlayerClass.Tank)
        {
            currentAnimator = player1TankAnimator;
            currentPlayer1ComboSubstate = ComboAnimationStatesData.unarmedSubStateTank;
            player1UnarmedCombos = _availableUnarmedCombos.unarmedTankCombos;
        }
        else if (newClass == PlayerCombat.PlayerClass.Warrior)
        {
            currentAnimator = player1WarriorAnimator;
            currentPlayer1ComboSubstate = ComboAnimationStatesData.unarmedSubStateWarrior;
            player1UnarmedCombos = _availableUnarmedCombos.unarmedWarriorCombos;
        }
        else if (newClass == PlayerCombat.PlayerClass.Ranged)
        {
            currentAnimator = player1RangedAnimator;
            currentPlayer1ComboSubstate = ComboAnimationStatesData.unarmedSubStateRanged;
            player1UnarmedCombos = _availableUnarmedCombos.unarmedRangedCombos;
        }
        else if (newClass == PlayerCombat.PlayerClass.Support)
        {
            currentAnimator = player1SupportAnimator;
            currentPlayer1ComboSubstate = ComboAnimationStatesData.unarmedSubStateSupport;
            player1UnarmedCombos = _availableUnarmedCombos.unarmedSupportCombos;
        }
        currentPlayer1Class = newClass;
        DefaultCombo();
    }
    public void AssignWeaponCombos(Weapon weapon)
    {
        currentPlayer1Weapon = weapon;

        currentEquippedPlayer1WeaponType = currentPlayer1Weapon.currentWeaponType;

        weaponSlashEffects = currentPlayer1Weapon.weaponSlashEffects;

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

        if(currentPlayer1Class == PlayerCombat.PlayerClass.Default)
        {
            currentPlayer1ComboSubstate = ComboAnimationStatesData.unarmedSubStateDefault;
            weaponSlashEffects = _defaultSlashes;
            player1UnarmedCombos = _availableUnarmedCombos.unarmedDefaultCombos;
        }
        else if (currentPlayer1Class == PlayerCombat.PlayerClass.Tank)
        {
            currentPlayer1ComboSubstate = ComboAnimationStatesData.unarmedSubStateTank;
            weaponSlashEffects = _tankSlashes;
            player1UnarmedCombos = _availableUnarmedCombos.unarmedTankCombos;
        }
        else if (currentPlayer1Class == PlayerCombat.PlayerClass.Warrior)
        {
            currentPlayer1ComboSubstate = ComboAnimationStatesData.unarmedSubStateWarrior;
            weaponSlashEffects = _warriorSlashes;
            player1UnarmedCombos = _availableUnarmedCombos.unarmedWarriorCombos;
        }
        else if (currentPlayer1Class == PlayerCombat.PlayerClass.Ranged)
        {
            currentPlayer1ComboSubstate = ComboAnimationStatesData.unarmedSubStateRanged;
            weaponSlashEffects = _rangedSlashes;
            player1UnarmedCombos = _availableUnarmedCombos.unarmedRangedCombos;
        }
        else if (currentPlayer1Class == PlayerCombat.PlayerClass.Support)
        {
            currentPlayer1ComboSubstate = ComboAnimationStatesData.unarmedSubStateSupport;
            weaponSlashEffects = _supportSlashes;
            player1UnarmedCombos = _availableUnarmedCombos.unarmedSupportCombos;
        }
    }

    private void SetAttackSpeed()
    {
        currentAnimator.SetFloat("AttackSpeed", _player1Attributes.attackSpeed);
    }
}
