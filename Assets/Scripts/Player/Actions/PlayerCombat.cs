using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Chained2ViolenceGameManager;


[RequireComponent(typeof(SwingAbility))]
public class PlayerCombat : MonoBehaviour
{
    public enum PlayerClass
    {
        Tank,
        Melee,
        Support,
        Ranged
    };

    public PlayerClass currentPlayerClass;
    public event Action<PlayerClass> OnPlayerClassChanged;

    public float attackCooldown = 1f;  // Cooldown between attacks
    public float abilityCooldown = 5f; // Cooldown between abilities
    public float attackRange = 2f;     // Range of attack
    public float attackDamage = 10f;   // Damage per attack
    public float coneAngle = 120f; // Set the cone angle (in degrees) for the attack


    protected float lastAttackTime;
    protected float lastAbilityTime;

    #region Ability components

    private SwingAbility swingAbility;
    private Projectile projectile;
    private ShieldAbility shieldAbility;
    private ConeAbility coneAbility;

    #endregion

    private void Start()
    {
        int playerId = gameObject.GetComponent<Player>()._playerId;

        // Set the player classes to the saved player class in the class manager. This is because player objects are destroyed between scenes
        if (playerId == 1)
        {
            currentPlayerClass = ClassManager._currentPlayer1Class;
        }
        else if (playerId == 2)
        {
            currentPlayerClass = ClassManager._currentPlayer2Class;
        }

        swingAbility = GetComponent<SwingAbility>();
        projectile = GetComponent<Projectile>();
        shieldAbility = GetComponent<ShieldAbility>();
        coneAbility = GetComponent<ConeAbility>();
    }

    /// <summary>
    /// This method is used for basic attacks (Called in Player script)
    /// </summary>
    public void UseBaseAttack()
    {
        // Find all potential targets within the attack range
        Collider[] potentialTargets = Physics.OverlapSphere(transform.position, attackRange);

        foreach (Collider target in potentialTargets)
        {
            // Check if the target is within the cone angle
            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            if (angleToTarget <= coneAngle / 2) // Check if within half the cone angle
            {
                // Check if target has a BaseManager component
                BaseManager targetManager = target.GetComponent<BaseManager>();
                if (targetManager != null)
                {
                    targetManager.DealDamageToEnemy(attackDamage);
                    Debug.Log("Hit enemy: " + target.name);
                }
            }
        }

        Debug.Log("Base Attack triggered.");
    }


    /// <summary>
    /// This method uses the ability that the player has for its class (Called in Player script)
    /// </summary>
    public void UseAbility()
    {
        switch (currentPlayerClass)
        {

            case PlayerClass.Melee:

                coneAbility.UseAbility();

                break;

            case PlayerClass.Support:

                shieldAbility.UseAbility();

                break;

            case PlayerClass.Ranged:

                projectile.UseAbility();

                break;

            case PlayerClass.Tank:

                swingAbility.UseAbility();

                break;

        }
    }

    // Method to set the player's attack damage (used for upgrades)
    public void SetAttackDamage(float newAttackDamage)
    {
        attackDamage = newAttackDamage;
        Debug.Log("Player attack damage set to: " + attackDamage);
    }

    /// <summary>
    /// This method is used for setting the player class 
    /// </summary>
    /// <param name="newPlayerClass"></param>
    public void SetCurrentPlayerClass(PlayerClass newPlayerClass)
    {
        currentPlayerClass = newPlayerClass;

        OnPlayerClassChanged?.Invoke(newPlayerClass);
    }

    // Visualize the cone in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        // Draw the attack range as a sphere
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Draw the cone visualization
        Vector3 forward = transform.forward * attackRange;
        Quaternion leftRotation = Quaternion.Euler(0, -coneAngle / 2, 0);
        Quaternion rightRotation = Quaternion.Euler(0, coneAngle / 2, 0);

        Vector3 leftDirection = leftRotation * forward;
        Vector3 rightDirection = rightRotation * forward;

        Gizmos.DrawLine(transform.position, transform.position + leftDirection);
        Gizmos.DrawLine(transform.position, transform.position + rightDirection);

        // Optionally, draw additional lines for better cone visualization
        int lineCount = 10; // Number of lines to draw within the cone for visualization
        for (int i = 0; i <= lineCount; i++)
        {
            float t = i / (float)lineCount;
            Quaternion rotation = Quaternion.Euler(0, Mathf.Lerp(-coneAngle / 2, coneAngle / 2, t), 0);
            Vector3 direction = rotation * forward;
            Gizmos.DrawLine(transform.position, transform.position + direction);
        }
    }
}