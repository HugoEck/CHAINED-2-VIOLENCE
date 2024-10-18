using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    private enum PlayerClass
    {
        Tank,
        Meele,
        Support,
        Ranged
    };

    

    public float attackCooldown = 1f;  // Cooldown between attacks
    public float abilityCooldown = 5f; // Cooldown between abilities
    public float attackRange = 2f;     // Range of attack
    public float attackDamage = 10f;   // Damage per attack

    protected float lastAttackTime;
    protected float lastAbilityTime;

    private SwingAbility swingAbility;

    PlayerClass currentPlayerClass = PlayerClass.Meele;

    private void Start()
    {
        swingAbility = GetComponent<SwingAbility>();
    }

    // Virtual method for handling attacks, to be overridden by subclasses.
    public virtual void Attack()
    {
        Debug.Log("Base Attack triggered.");
    }

    // Virtual method for handling abilities, to be overridden by subclasses.
    public virtual void UseAbility()
    {
        switch (currentPlayerClass)
        {

            case PlayerClass.Meele:            

                break;

            case PlayerClass.Support:

                break;

            case PlayerClass.Ranged:

                break;

            case PlayerClass.Tank:

                swingAbility.UseAbility();

                break;

        }

        swingAbility.UseAbility();
        Debug.Log("Base Ability triggered.");
    }


    void Update()
    {
      //  HandleInput();
    }

    // Method to set the player's attack damage (used for upgrades)
    public void SetAttackDamage(float newAttackDamage)
    {
        attackDamage = newAttackDamage;
        Debug.Log("Player attack damage set to: " + attackDamage);
    }
}