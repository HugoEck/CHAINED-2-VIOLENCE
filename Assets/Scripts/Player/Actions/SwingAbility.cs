using UnityEngine;
using System.Collections;

public class SwingAbility : PlayerCombat
{
    public Transform otherPlayer;      // Reference to the other player being swung
    public float swingDuration = 3f;   // Duration of the swing
    public float swingSpeed = 200f;    // Speed of the swing (degrees per second)
    public float swingRadius = 5f;     // Fixed radius at which the other player should swing
    public float swingDamage = 20f;    // Damage dealt to enemies during the swing
    private bool isSwinging = false;   // Flag to track if the player is currently swinging

    public LayerMask enemyLayer;       // Layer for enemies (make sure enemies are on this layer)
    public LayerMask playerLayer;      // Layer for players (including the swung player)

    private Rigidbody otherPlayerRb;   // Rigidbody of the other player
    private Rigidbody anchorRb;        // Rigidbody of this (anchor) player

    private void Start()
    {
        if (otherPlayer != null)
        {
            otherPlayerRb = otherPlayer.GetComponent<Rigidbody>();
        }

        // Get the Rigidbody of this player (anchor)
        anchorRb = GetComponent<Rigidbody>();
    }

    public override void UseAbility()
    {
        if (!isSwinging && otherPlayer != null)
        {
            StartSwing();
        }
    }

    void StartSwing()
    {
        isSwinging = true;

        // Set the anchor player to be kinematic to prevent movement during the swing
        anchorRb.isKinematic = true;

        // Disable collisions between the swung player and enemies
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);

        StartCoroutine(SwingOtherPlayer());
    }

    IEnumerator SwingOtherPlayer()
    {
        float elapsedTime = 0f;
        float currentAngle = 0f;  // Angle to control the swing position

        Vector3 swingCenter = transform.position; // The anchor player's position

        while (elapsedTime < swingDuration)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the angle to move the swung player in a circular path
            currentAngle += swingSpeed * Time.deltaTime;

            // Ensure the angle stays within 360 degrees
            if (currentAngle >= 360f) currentAngle -= 360f;

            // Convert the angle to radians for calculations
            float angleInRadians = currentAngle * Mathf.Deg2Rad;

            // Calculate the new position based on the angle and fixed radius
            Vector3 newSwingPosition = new Vector3(
                swingCenter.x + swingRadius * Mathf.Cos(angleInRadians),
                otherPlayer.position.y,  // Keep the player's current height
                swingCenter.z + swingRadius * Mathf.Sin(angleInRadians)
            );

            // Move the swung player to the new calculated position
            otherPlayerRb.MovePosition(newSwingPosition);

            // Detect enemies in the swing radius and apply damage
            Collider[] hitEnemies = Physics.OverlapSphere(transform.position, swingRadius);
            foreach (Collider enemy in hitEnemies)
            {
                BaseManager enemyManager = enemy.GetComponent<BaseManager>();
                if (enemyManager != null)
                {
                    enemyManager.DealDamageToEnemy(swingDamage);
                    Debug.Log("Hit enemy during swing: " + enemy.name);
                }
            }

            yield return null;
        }

        isSwinging = false;
        Debug.Log("Swing ended.");

        // Re-enable collisions between the swung player and enemies
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);

        // Unset kinematic mode for the anchor player so they can move again
        anchorRb.isKinematic = false;
    }

    // Optional: Visualize the swing radius in the scene view for debugging.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, swingRadius);
    }
}
