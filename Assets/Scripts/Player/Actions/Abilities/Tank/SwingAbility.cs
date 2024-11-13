using UnityEngine;
using System.Collections;

public class SwingAbility : MonoBehaviour, IAbility
{
    public static bool BIsPlayerCurrentlySwinging = false;

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

    public GameObject swingEffectPrefab;

    private void Start()
    {
        if (otherPlayer != null)
        {
            otherPlayerRb = otherPlayer.GetComponent<Rigidbody>();
        }

        // Get the Rigidbody of this player (anchor)
        anchorRb = GetComponent<Rigidbody>();
    }

    public void UseAbility()
    {
        if (!isSwinging && otherPlayer != null && !BIsPlayerCurrentlySwinging)
        {
            StartSwing();
            
        }
    }

    void StartSwing()
    {
        // Instantiate and store the effect instance
        GameObject swingEffect = Instantiate(swingEffectPrefab, transform.position, Quaternion.identity);

        BIsPlayerCurrentlySwinging = true;
        swingRadius = AdjustChainLength.currentChainLength;

        isSwinging = true;

        // Set both players to be kinematic to prevent normal physics movement during the swing
        anchorRb.isKinematic = true;
        otherPlayerRb.isKinematic = true;

        // Disable collisions between the swung player and enemies
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);

        StartCoroutine(SwingOtherPlayer());
        Destroy(swingEffect, swingDuration);
    }

    IEnumerator SwingOtherPlayer()
    {
        float elapsedTime = 0f;
        float currentAngle = 0f;  // Angle to control the swing position
        Vector3 swingCenter = transform.position; // The anchor player's position

        // Get the current position of the other player
        Vector3 initialPosition = otherPlayer.position;

        // Calculate the initial angle based on the current position of the swung player
        float offsetX = initialPosition.x - swingCenter.x;
        float offsetZ = initialPosition.z - swingCenter.z;
        currentAngle = Mathf.Atan2(offsetZ, offsetX) * Mathf.Rad2Deg; // Get angle in degrees

        while (elapsedTime < swingDuration)
        {
            elapsedTime += Time.fixedDeltaTime;  // Use FixedUpdate timing for smooth physics movement

            // Calculate the angle to move the swung player in a circular path
            currentAngle += swingSpeed * Time.fixedDeltaTime;

            // Ensure the angle stays within 360 degrees
            if (currentAngle >= 360f) currentAngle -= 360f;

            // Convert the angle to radians for calculations
            float angleInRadians = currentAngle * Mathf.Deg2Rad;

            // Calculate the new position based on the angle and fixed radius
            Vector3 newSwingPosition = new Vector3(
                swingCenter.x + swingRadius * Mathf.Cos(angleInRadians),
                initialPosition.y,  // Keep the player's current height
                swingCenter.z + swingRadius * Mathf.Sin(angleInRadians)
            );

            // Update the position of the swung player directly
            otherPlayer.position = newSwingPosition;  // Directly set the Rigidbody's position

            // Optional: Rotate the swung player to face the direction of the swing
            otherPlayer.rotation = Quaternion.LookRotation(newSwingPosition - swingCenter);

            // Detect enemies in the swing radius and apply damage
            Collider[] hitEnemies = Physics.OverlapSphere(swingCenter, swingRadius);
            foreach (Collider enemy in hitEnemies)
            {
                BaseManager enemyManager = enemy.GetComponent<BaseManager>();
                if (enemyManager != null)
                {
                    enemyManager.DealDamageToEnemy(swingDamage);
                    Debug.Log("Hit enemy during swing: " + enemy.name);
                }
            }

            yield return new WaitForFixedUpdate();  // Use FixedUpdate timing
        }

        isSwinging = false;
        Debug.Log("Swing ended.");

        // Re-enable collisions between the swung player and enemies
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);

        // Unset kinematic mode for both players so they can move again
        anchorRb.isKinematic = false;
        otherPlayerRb.isKinematic = false;

        BIsPlayerCurrentlySwinging = false;
    }

    // Optional: Visualize the swing radius in the scene view for debugging.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, swingRadius);
    }
}

