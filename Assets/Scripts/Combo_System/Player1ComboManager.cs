using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player1ComboManager : MonoBehaviour
{
    public static Player1ComboManager instance { get; private set; }

    //[Header("Sound Effects")]
    //[SerializeField] private AudioClip[] unarmedSounds;
    //[SerializeField] private AudioClip[] twoHandedWeaponSounds;
    //[SerializeField] private AudioClip[] oneHandedWeaponSounds;
    //[SerializeField] private AudioClip[] reallyBigTwoHandedWeaponSounds;
    //[SerializeField] private AudioClip[] polearmWeaponSounds;
    //[SerializeField] private AudioClip[] daggerWeaponSounds;
    //[SerializeField] private AudioClip[] bigPenWeaponSounds;

    private AudioClipManager audioClipManager;

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

    public Animator currentAnimator { get; private set; }

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
        audioClipManager = FindObjectOfType<AudioClipManager>();

        if (ClassManager._currentPlayer1Class == PlayerCombat.PlayerClass.Default)
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

    private float _saveNormalWalkingSpeed;
    public void SetCombatWalkingSpeed()
    {
        _saveNormalWalkingSpeed = _player1Attributes.movementSpeed;

        _player1Attributes.movementSpeed = _player1Attributes.movementSpeed * 0.5f;
    }
    public void NormalWalkingSpeed()
    {
        if (_saveNormalWalkingSpeed <= 0) return;

        _player1Attributes.movementSpeed = _saveNormalWalkingSpeed;
    }
    public void DealDamageToEnemies(float attackRange, float attackDamage, float stunDuration, float knockbackForce, float maxAngle)
    {
        bool durabilityReduced = false;
        int comboIndex = currentAnimator.GetInteger("ComboIndex");

        TriggerWeaponSlash();
        // Find all enemies within the attack range

        float totalAttackRange = attackRange;
        if(totalAttackRange > 10)
        {
            totalAttackRange = 10;
        }
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, totalAttackRange);
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
                    if (currentPlayer1Weapon != null)
                    {
                        // Play corresponding sound for the combo
                        
                        enemyManager.DealDamageToEnemy(attackDamage + _player1Attributes.attackDamage, BaseManager.DamageType.WeaponDamage, true, false);
                    }
                    else
                    {
                        if(currentPlayer1Class != PlayerCombat.PlayerClass.Default)
                        {
                            enemyManager.DealDamageToEnemy(attackDamage + _player1Attributes.attackDamage, BaseManager.DamageType.UnarmedDamage, true, false);                 
                        }
                    }
                    PlayComboSound(comboIndex);
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
        currentAnimator.SetInteger("currentPlayerClass", (int)currentPlayer1Class);
        SetAttackSpeed();
    }

    private void ApplyWeaponSlashEffect(int comboIndex)
    {
        if (currentAnimator.GetBool("WeaponSlash"))
        {
            if(currentPlayer1Weapon != null)
            {
                weaponSlashEffects[comboIndex].gameObject.transform.position = currentPlayer1Weapon.slashEffectPositions[comboIndex].position;

                ParticleSystem particle = weaponSlashEffects[comboIndex].GetComponent<ParticleSystem>();
                var mainModule = particle.main;

                float attackRange = currentPlayer1Weapon.combos[comboIndex].attackRange;
                if (attackRange > 10)
                {
                    attackRange = 10;
                }
                float totalAttackRange = (attackRange / 10f) * 4f;

                mainModule.startSize = totalAttackRange;
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

                    float attackRange = player1UnarmedCombos[comboIndex].attackRange;
                    if (attackRange > 10)
                    {
                        attackRange = 10;
                    }
                    float totalAttackRange = (attackRange / 10f) * 4f;

                    mainModule.startSize = totalAttackRange;
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
        currentAnimator.SetInteger("currentWeapon", 0);
    }

    private void WeaponManager_OnWeaponEquippedPlayer1(GameObject equippedWeapon)
    {
        _currentPlayer1WeaponObject = equippedWeapon;

        currentAnimator.SetInteger("ComboIndex", 0);

        AssignWeaponCombos(_currentPlayer1WeaponObject.GetComponent<Weapon>());

        currentAnimator.SetInteger("currentWeapon", (int)currentPlayer1Weapon.currentWeaponType);
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

    private void PlayComboSound(int comboIndex)
    {
        if (audioClipManager == null)
        {
            Debug.LogWarning("AudioClipManager is not initialized.");
            return;
        }

        // Adjust for 0-based indexing if comboIndex starts at 1
        int adjustedIndex = comboIndex - 1;

        AudioClip[] selectedSounds = null;

        if (currentPlayer1Weapon != null)
        {
            // Determine sound by weapon type
            switch (currentEquippedPlayer1WeaponType)
            {
                case Weapon.WeaponType.TwoHanded:
                    selectedSounds = audioClipManager.twoHandedWeaponSounds;
                    break;

                case Weapon.WeaponType.OneHanded:
                    selectedSounds = audioClipManager.oneHandedWeaponSounds;
                    break;

                case Weapon.WeaponType.ReallyBigTwoHanded:
                    selectedSounds = audioClipManager.reallyBigTwoHandedWeaponSounds;
                    break;

                case Weapon.WeaponType.Polearm:
                    selectedSounds = audioClipManager.polearmWeaponSounds;
                    break;

                case Weapon.WeaponType.Dagger:
                    selectedSounds = audioClipManager.daggerWeaponSounds;
                    break;

                case Weapon.WeaponType.BigPen:
                    selectedSounds = audioClipManager.bigPenWeaponSounds;
                    break;

                default:
                    Debug.LogWarning($"No sound defined for weapon type: {currentEquippedPlayer1WeaponType}");
                    break;
            }
        }
        else
        {
            if (currentPlayer1Class == PlayerCombat.PlayerClass.Default)
            {
                selectedSounds = audioClipManager.tankUnarmedSounds;
            }
            else if (currentPlayer1Class == PlayerCombat.PlayerClass.Tank)
            {
                selectedSounds = audioClipManager.tankUnarmedSounds;
            }
            else if (currentPlayer1Class == PlayerCombat.PlayerClass.Warrior)
            {
                selectedSounds = audioClipManager.warriorUnarmedSounds;
            }
            else if (currentPlayer1Class == PlayerCombat.PlayerClass.Ranged)
            {
                selectedSounds = audioClipManager.rangedUnarmedSounds;
            }
            else if (currentPlayer1Class == PlayerCombat.PlayerClass.Support)
            {
                selectedSounds = audioClipManager.supportUnarmedSounds;
            }
        }

        // Play the selected sound if available
        if (selectedSounds != null && adjustedIndex >= 0 && adjustedIndex < selectedSounds.Length)
        {
            SFXManager.instance.PlaySFXClip(selectedSounds[adjustedIndex], transform, 1.0f);
        }
    }
}
