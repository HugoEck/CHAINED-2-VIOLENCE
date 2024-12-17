using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    [SerializeField] private PlayerCombat _currentPlayerClass;

    [Header("Class specific attributes")]
    [SerializeField] private ClassAttributeSO[] _classesAttributes;
    private int _playerId;

    public float attackDamage { get; set; }
    public float maxHP { get; set; }
    public float movementSpeed { get; set; }
    public float mass { get; set; }
    public float knockBack { get; set; }
    public float attackSpeed { get; set; }

    public float _upgradeAttackDamage;
    public float _upgradeMaxHP;
    public float _upgradeMovementSpeed;
    public float _upgrademMass;
    public float _upgradeKnockBack;
    public float _upgradeAttackSpeed;

    private void Start()
    {
        _currentPlayerClass.OnClassSwitched += PlayerCombatOnClassSwitched;
        //SetBaseValues(_currentPlayerClass.currentPlayerClass);

        // Apply correct upgraded stats upon loading back into lobby.
        _upgradeMaxHP = StatsTransfer.Player1UpgradedMaxHP;
        _upgradeAttackDamage = StatsTransfer.Player1UpgradedAttackDamage;
        _upgradeMovementSpeed = StatsTransfer.Player1UpgradedSpeed;
        RecalculateMaxHP();
        RecalculateAttackDamage();
        RecalculateMovementSpeed();

    }

    private void OnDestroy()
    {
        _currentPlayerClass.OnClassSwitched -= PlayerCombatOnClassSwitched;
    }

    public void SetPlayerId(int playerId)
    {
        _playerId = playerId;
    }

    #region Recalculation Methods
    public void RecalculateAttackDamage()
    {
        foreach (ClassAttributeSO classAttribute in _classesAttributes)
        {
            if (classAttribute.thisClass == _currentPlayerClass.currentPlayerClass)
            {
                attackDamage = classAttribute.attackDamage + _upgradeAttackDamage;
                break;
            }
        }
    }

    public void RecalculateMaxHP()
    {
        foreach (ClassAttributeSO classAttribute in _classesAttributes)
        {
            if (classAttribute.thisClass == _currentPlayerClass.currentPlayerClass)
            {
                maxHP = classAttribute.maxHP + _upgradeMaxHP;
                break;
            }
        }
    }

    public void RecalculateMovementSpeed()
    {
        foreach (ClassAttributeSO classAttribute in _classesAttributes)
        {
            if (classAttribute.thisClass == _currentPlayerClass.currentPlayerClass)
            {
                movementSpeed = classAttribute.movementSpeed + _upgradeMovementSpeed;
                break;
            }
        }
    }

    private void RecalculateKnockBack()
    {
        foreach (ClassAttributeSO classAttribute in _classesAttributes)
        {
            if (classAttribute.thisClass == _currentPlayerClass.currentPlayerClass)
            {
                knockBack = classAttribute.knockBack + _upgradeKnockBack;
                break;
            }
        }
    }

    private void RecalculateAttackSpeed()
    {
        foreach (ClassAttributeSO classAttribute in _classesAttributes)
        {
            if (classAttribute.thisClass == _currentPlayerClass.currentPlayerClass)
            {
                attackSpeed = classAttribute.attackSpeed + _upgradeAttackSpeed;
                break;
            }
        }
    }
    #endregion

    #region Methods for the Upgrade System.
    public void UpgradeAttackDamage(float damageAmount)
    {
        _upgradeAttackDamage += damageAmount;
        RecalculateAttackDamage();
    }

    public void UpgradeMaxHealth(float healthAmount)
    {
        _upgradeMaxHP += healthAmount;
        RecalculateMaxHP();
    }

    public void UpgradeMovementSpeed(float movementSpeedAmount)
    {
        _upgradeMovementSpeed += movementSpeedAmount;
        RecalculateMovementSpeed();
    }

    public void UpgradeKnockBack(float knockBackAmount)
    {
        _upgradeKnockBack += knockBackAmount;
        RecalculateKnockBack();
    }

    public void UpgradeAttackSpeed(float attackSpeedAmount)
    {
        _upgradeAttackSpeed += attackSpeedAmount;
        RecalculateAttackSpeed();
    }
    #endregion

    #region UPGRADE METHODS FOR PLAYER ATTRIBUTES USED FOR ITEM SYSTEM.

    public void AdjustAttackDamage(float plusMinusDamage)
    {
        _upgradeAttackDamage = 0;
        _upgradeAttackDamage += plusMinusDamage; 
        attackDamage = Mathf.Max(0, attackDamage + _upgradeAttackDamage);
    }

    public void AdjustMaxHP(float plusMinusHP)
    {
        _upgradeMaxHP = 0;
        _upgradeMaxHP += plusMinusHP;
        maxHP = Mathf.Max(0, maxHP + _upgradeMaxHP);
    }

    public void AdjustMovementSpeed(float plusMinusMovementSpeed)
    {
        _upgradeMovementSpeed = 0;
        _upgradeMovementSpeed += plusMinusMovementSpeed;
        movementSpeed = Mathf.Max(0, movementSpeed + _upgradeMovementSpeed);
    }

    public void AdjustKnocback(float plusMinusKnockback)
    {
        _upgradeKnockBack = 0;
        _upgradeKnockBack += plusMinusKnockback;
        knockBack = Mathf.Max(0, knockBack + _upgradeKnockBack);
    }
    public void AdjustAttackSpeed(float plusMinusAttackSpeed)
    {

        _upgradeAttackSpeed = 0;
        _upgradeAttackSpeed += plusMinusAttackSpeed;
        attackSpeed = Mathf.Max(0, attackSpeed + _upgradeAttackSpeed);
    }

    #endregion

    //private void PlayerCombatOnClassSwitched(PlayerCombat.PlayerClass currentClass)
    //{
    //    switch (currentClass)
    //    {
    //        case PlayerCombat.PlayerClass.Default:

    //            SetBaseValues(PlayerCombat.PlayerClass.Default);

    //            break;

    //        case PlayerCombat.PlayerClass.Tank:

    //            SetBaseValues(PlayerCombat.PlayerClass.Tank);

    //            break;

    //        case PlayerCombat.PlayerClass.Warrior:

    //            SetBaseValues(PlayerCombat.PlayerClass.Warrior);

    //            break;

    //        case PlayerCombat.PlayerClass.Ranged:

    //            SetBaseValues(PlayerCombat.PlayerClass.Ranged);

    //            break;

    //        case PlayerCombat.PlayerClass.Support:

    //            SetBaseValues(PlayerCombat.PlayerClass.Support);

    //            break;
    //    }
    //}

    private void PlayerCombatOnClassSwitched(PlayerCombat.PlayerClass currentClass)
    {
        SetBaseValues(currentClass);
        RecalculateAttackDamage();
        RecalculateMaxHP();
        RecalculateMovementSpeed();
        RecalculateKnockBack();
        RecalculateAttackSpeed();
    }

    //public void SetBaseValues(PlayerCombat.PlayerClass classToCheck)
    //{

    //    foreach(ClassAttributeSO classAttribute in _classesAttributes)
    //    {
    //        if(classAttribute.thisClass == classToCheck)
    //        {
    //            attackDamage = classAttribute.attackDamage + _upgradeAttackDamage;
    //            maxHP = classAttribute.maxHP + _upgradeMaxHP;
    //            movementSpeed = classAttribute.movementSpeed + _upgradeMovementSpeed;
    //            mass = classAttribute.mass;
    //            knockBack = classAttribute.knockBack + _upgradeKnockBack;
    //            attackSpeed = classAttribute.attackSpeed + _upgradeAttackSpeed;
    //        }
    //    }        
    //}

    public void SetBaseValues(PlayerCombat.PlayerClass classToCheck)
    {
        foreach (ClassAttributeSO classAttribute in _classesAttributes)
        {
            if (classAttribute.thisClass == classToCheck)
            {
                // Base stats and upgrades based on player ID
                if (_playerId == 1)
                {
                    attackDamage = classAttribute.attackDamage + StatsTransfer.Player1UpgradedAttackDamage;// + _upgradeAttackDamage;
                    maxHP = classAttribute.maxHP + StatsTransfer.Player1UpgradedMaxHP;// + _upgradeMaxHP;
                    movementSpeed = classAttribute.movementSpeed + StatsTransfer.Player1UpgradedSpeed;// + _upgradeMovementSpeed;
                }
                else if (_playerId == 2)
                {
                    attackDamage = classAttribute.attackDamage + StatsTransfer.Player2UpgradedAttackDamage;// + _upgradeAttackDamage;
                    maxHP = classAttribute.maxHP + StatsTransfer.Player2UpgradedMaxHP;// + _upgradeMaxHP;
                    movementSpeed = classAttribute.movementSpeed + StatsTransfer.Player2UpgradedSpeed;// + _upgradeMovementSpeed;
                }

                // Common attributes
                mass = classAttribute.mass;
                knockBack = classAttribute.knockBack + _upgradeKnockBack;
                attackSpeed = classAttribute.attackSpeed + _upgradeAttackSpeed;
            }
        }
    }
}
