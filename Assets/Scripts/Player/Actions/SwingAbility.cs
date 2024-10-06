using UnityEngine;
using System.Collections;

public class SwingAbility : PlayerCombat
{
    public Transform otherPlayer;      // Reference to the other player being swung
    public float swingDuration = 3f;   // Duration of the swing
    public float swingForce = 500f;    // Force applied during the swing
    public float swingDamage = 20f;    // Damage dealt to enemies during the swing
    public float swingRadius = 5f;     // Radius within which enemies can be hit by the swing
    private bool isSwinging = false;   // Flag to track if the player is currently swinging
    private Rigidbody otherPlayerRb;   // Rigidbody of the other player
    private Rigidbody anchorRb;        // Rigidbody of this (anchor) player

    void Start()
    {
        if (otherPlayer != null)
        {
            otherPlayerRb = otherPlayer.GetComponent<Rigidbody>();
        }

        // Get the Rigidbody of this player (anchor)
        anchorRb = GetComponent<Rigidbody>();

        // Freeze rotation for stability
        anchorRb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    // Override the UseAbility method to implement the swinging ability
    public override void UseAbility()
    {
        if (!isSwinging && otherPlayer != null)
        {
            StartSwing();
        }
    }

    // Start the tethered swing (swinging the other player)
    void StartSwing()
    {
        isSwinging = true;

        // Freeze the anchor player's position during the swing
        anchorRb.isKinematic = true;

        StartCoroutine(SwingOtherPlayer());
    }

    // Coroutine to swing the other player around the anchor
    IEnumerator SwingOtherPlayer()
    {
        float elapsedTime = 0f;
        Vector3 swingCenter = transform.position; // The anchor player's position

        while (elapsedTime < swingDuration)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the direction to the anchor (this player)
            Vector3 directionToAnchor = (otherPlayer.position - swingCenter).normalized;

            // Calculate the perpendicular force direction (tangential to the swing)
            Vector3 perpendicularForce = Vector3.Cross(directionToAnchor, Vector3.up).normalized;

            // Apply force to the other player to simulate swinging
            otherPlayerRb.AddForce(perpendicularForce * swingForce * Time.deltaTime, ForceMode.VelocityChange);

            // Detect enemies in the swing radius and apply damage
            Collider[] hitEnemies = Physics.OverlapSphere(transform.position, swingRadius);
            foreach (Collider enemy in hitEnemies)
            {
                BaseManager enemyManager = enemy.GetComponent<BaseManager>();
                if (enemyManager != null)
                {
                    enemyManager.SetHealth(swingDamage);
                    Debug.Log("Hit enemy during swing: " + enemy.name);
                }
            }

            yield return null;
        }

        isSwinging = false;
        Debug.Log("Swing ended.");

        // Unfreeze the anchor player's position after the swing
        anchorRb.isKinematic = false;
    }

    // Optional: Visualize the swing radius in the scene view for debugging.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, swingRadius);
    }
}