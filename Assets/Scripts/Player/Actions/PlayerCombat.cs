using UnityEngine;

public abstract class PlayerCombat : MonoBehaviour
{
    public float attackCooldown = 1f;  // Cooldown between attacks
    public float abilityCooldown = 5f; // Cooldown between abilities
    public float attackRange = 2f;     // Range of attack
    public float attackDamage = 10f;   // Damage per attack

    protected float lastAttackTime;
    protected float lastAbilityTime;

    // Virtual method for handling attacks, to be overridden by subclasses.
    public virtual void Attack()
    {
        Debug.Log("Base Attack triggered.");
    }

    // Virtual method for handling abilities, to be overridden by subclasses.
    public virtual void UseAbility()
    {
        Debug.Log("Base Ability triggered.");
    }

    // Method to handle player input for attacks and abilities.
    public void HandleInput()
    {
        // Handle attack input (left mouse button)
        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time > lastAttackTime + attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }

        // Handle ability input (right mouse button)
        if (Input.GetKeyDown(KeyCode.Mouse1) && Time.time > lastAbilityTime + abilityCooldown)
        {
            UseAbility();
            lastAbilityTime = Time.time;
        }
    }

    void Update()
    {
        HandleInput();
    }
}