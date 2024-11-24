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


    #region UPGRADE METHODS FOR PLAYER ATTRIBUTES

    
    public void AdjustAttackDamage(float plusMinusDamage)
    {
        attackDamage += plusMinusDamage; 
        attackDamage = Mathf.Max(0, attackDamage); 
    }

    public void AdjustMaxHP(float plusMinusHP)
    {
        maxHP += plusMinusHP;
        maxHP = Mathf.Max(0, maxHP);
    }

    public void AdjustMovementSpeed(float plusMinusMovementSpeed)
    {
        movementSpeed += plusMinusMovementSpeed;
        movementSpeed = Mathf.Max(0, movementSpeed);
    }

    public void AdjustKnocback(float plusMinusKnockback)
    {
        knockBack += plusMinusKnockback;
        knockBack = Mathf.Max(0, knockBack);
    }

    #endregion

    private void Start()
    {
        _currentPlayerClass.OnClassSwitched += PlayerCombatOnClassSwitched;
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
                attackDamage = classAttribute.attackDamage + attackDamage;
                maxHP = classAttribute.maxHP + maxHP;
                movementSpeed = classAttribute.movementSpeed + movementSpeed;
                mass = classAttribute.mass;
                knockBack = classAttribute.knockBack + knockBack;
            }
        }        
    }
}
