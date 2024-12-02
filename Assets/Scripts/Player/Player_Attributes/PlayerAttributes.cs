using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    [SerializeField] private PlayerCombat _currentPlayerClass;

    [Header("Class specific attributes")]
    [SerializeField] private ClassAttributeSO[] _classesAttributes;

    public float attackDamage { get; set; }
    public float maxHP { get; set; }
    public float movementSpeed { get; set; }
    public float mass { get; set; }
    public float knockBack { get; set; }
    public float attackSpeed { get; set; }

    private float _upgradeAttackDamage;
    private float _upgradeMaxHP;
    private float _upgradeMovementSpeed;
    private float _upgrademMass; 
    private float _upgradeKnockBack; 
    private float _upgradeAttackSpeed;

    // Base attributes for upgrade system.
    private float _baseAttackDamage;
    private float _baseMaxHP;
    private float _baseMovementSpeed;
    private float _baseAttackSpeed;

    #region Methods for the Upgrade System.
    public void UpgradeAttackDamage(float damageAmount) {
        _upgradeAttackDamage += damageAmount;
        attackDamage = Mathf.Max(0, _baseAttackDamage + _upgradeAttackDamage);
    }
    public void UpgradeMaxHealth(float healthAmount) {
        _upgradeMaxHP += healthAmount;
        maxHP = Mathf.Max(0, _baseMaxHP + _upgradeMaxHP);
    }
    public void UpgradeMovementSpeed(float movementSpeedAmount) {
        _upgradeMovementSpeed += movementSpeedAmount;
        movementSpeed = Mathf.Max(0, _baseMovementSpeed + _upgradeMovementSpeed);
    }
    public void UpgradeAttackSpeed(float attackSpeedAmount) {
        _upgradeAttackSpeed += attackSpeedAmount;
        attackSpeed = Mathf.Max(0, _baseAttackSpeed + _upgradeAttackSpeed);
    } 
    #endregion

    #region UPGRADE METHODS FOR PLAYER ATTRIBUTES

    public void AdjustAttackDamage(float plusMinusDamage)
    {
        _upgradeAttackDamage += plusMinusDamage; 
        attackDamage = Mathf.Max(0, attackDamage + _upgradeAttackDamage); 
    }

    public void AdjustMaxHP(float plusMinusHP)
    {
        _upgradeMaxHP += plusMinusHP;
        maxHP = Mathf.Max(0, maxHP + _upgradeMaxHP);
    }

    public void AdjustMovementSpeed(float plusMinusMovementSpeed)
    {
        _upgradeMovementSpeed += plusMinusMovementSpeed;
        movementSpeed = Mathf.Max(0, movementSpeed + _upgradeMovementSpeed);
    }

    public void AdjustKnocback(float plusMinusKnockback)
    {
        _upgradeKnockBack += plusMinusKnockback;
        knockBack = Mathf.Max(0, knockBack + _upgradeKnockBack);
    }
    public void AdjustAttackSpeed(float plusMinusAttackSpeed)
    {
        _upgradeAttackSpeed += plusMinusAttackSpeed;
        attackSpeed = Mathf.Max(0, attackSpeed + _upgradeAttackSpeed);
    }

    #endregion

    private void Start()
    {
        _currentPlayerClass.OnClassSwitched += PlayerCombatOnClassSwitched;

        //SetBaseValues(_currentPlayerClass.currentPlayerClass);

        #region Base Attributes for upgrade system.
        _baseAttackDamage = attackDamage;
        _baseMaxHP = maxHP;
        _baseMovementSpeed = movementSpeed;
        _baseAttackSpeed = attackSpeed;
        #endregion
    }
    private void OnDestroy()
    {
        _currentPlayerClass.OnClassSwitched -= PlayerCombatOnClassSwitched;
    }

    private void PlayerCombatOnClassSwitched(PlayerCombat.PlayerClass currentClass)
    {
        switch (currentClass)
        {
            case PlayerCombat.PlayerClass.Default:

                SetBaseValues(PlayerCombat.PlayerClass.Default);

                break;

            case PlayerCombat.PlayerClass.Tank:

                SetBaseValues(PlayerCombat.PlayerClass.Tank);

                break;

            case PlayerCombat.PlayerClass.Warrior:

                SetBaseValues(PlayerCombat.PlayerClass.Warrior);

                break;

            case PlayerCombat.PlayerClass.Ranged:

                SetBaseValues(PlayerCombat.PlayerClass.Ranged);

                break;

            case PlayerCombat.PlayerClass.Support:

                SetBaseValues(PlayerCombat.PlayerClass.Support);

                break;
        }
    }

    public void SetBaseValues(PlayerCombat.PlayerClass classToCheck)
    {

        foreach(ClassAttributeSO classAttribute in _classesAttributes)
        {
            if(classAttribute.thisClass == classToCheck)
            {
                attackDamage = classAttribute.attackDamage + _upgradeAttackDamage;
                maxHP = classAttribute.maxHP + _upgradeMaxHP;
                movementSpeed = classAttribute.movementSpeed + _upgradeMovementSpeed;
                mass = classAttribute.mass;
                knockBack = classAttribute.knockBack + _upgradeKnockBack;
                attackSpeed = classAttribute.attackSpeed + _upgradeAttackSpeed;
            }
        }        
    }
    
}
