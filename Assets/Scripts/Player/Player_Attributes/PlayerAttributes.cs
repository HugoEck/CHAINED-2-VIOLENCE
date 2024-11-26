using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    [SerializeField] private PlayerCombat _currentPlayerClass;

    [Header("Class specific attributes")]
    [SerializeField] private ClassAttributeSO[] _classesAttributes;

    public float attackDamage { get; private set; }
    public float maxHP { get; private set; }
    public float movementSpeed { get; private set; }
    public float mass { get; private set; }
    public float knockBack { get; private set; }
    public float attackSpeed { get; private set; }

    private float _upgradeAttackDamage; 
    private float _upgradeMaxHP; 
    private float _upgradeMovementSpeed;
    private float _upgrademMass; 
    private float _upgradeKnockBack; 
    private float _upgradeAttackSpeed; 

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

        SetBaseValues(_currentPlayerClass.currentPlayerClass);
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

    private void SetBaseValues(PlayerCombat.PlayerClass classToCheck)
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
