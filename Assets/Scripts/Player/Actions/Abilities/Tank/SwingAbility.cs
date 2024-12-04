using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwingAbility : MonoBehaviour, IAbility
{
    public static bool BIsPlayerCurrentlySwinging = false;

    public Transform otherPlayer;         // Reference to the other player being swung
    public float swingDuration = 3f;      // Duration of the swing
    public float swingSpeed = 200f;       // Speed of the swing (degrees per second)
    public float swingRadius = 5f;        // Fixed radius at which the other player should swing
    public float baseSwingDamage = 20f;       // Damage dealt to enemies during the swing
    public float cooldown = 5f;           // Cooldown duration for the ability
    private float lastSwingTime = -Mathf.Infinity; // Time when the ability was last used
    private float swingDamage;

    private bool isSwinging = false;      // Flag to track if the player is currently swinging
    public LayerMask enemyLayer;          // Layer for enemies
    public LayerMask playerLayer;         // Layer for players

    private Rigidbody otherPlayerRb;      // Rigidbody of the other player
    private Rigidbody anchorRb;           // Rigidbody of this (anchor) player

    public PlayerAttributes playerAttributes;
    public GameObject swingEffectPrefab;

    private HashSet<Collider> hitEnemies;

    private void Start()
    {
        if (otherPlayer != null)
        {
            otherPlayerRb = otherPlayer.GetComponent<Rigidbody>();
        }
        anchorRb = GetComponent<Rigidbody>(); // Get the Rigidbody of this player (anchor)
    }

    public void UseAbility()
    {
        // Check if the ability is ready (cooldown has elapsed)
        if (!isSwinging && otherPlayer != null && !BIsPlayerCurrentlySwinging && Time.time >= lastSwingTime + cooldown)
        {
            StartSwing();
            lastSwingTime = Time.time; // Update the last use time
        }
        else
        {
            Debug.Log("Swing ability is on cooldown.");
        }
    }

    void StartSwing()
    {
        swingDamage = baseSwingDamage + playerAttributes.attackDamage;

        GameObject swingEffect = Instantiate(swingEffectPrefab, transform.position, Quaternion.identity);

        BIsPlayerCurrentlySwinging = true;
        swingRadius = AdjustChainLength.currentChainLength;

        isSwinging = true;

        hitEnemies = new HashSet<Collider>();

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

        Vector3 initialPosition = otherPlayer.position;
        float offsetX = initialPosition.x - swingCenter.x;
        float offsetZ = initialPosition.z - swingCenter.z;
        currentAngle = Mathf.Atan2(offsetZ, offsetX) * Mathf.Rad2Deg; // Get angle in degrees

        while (elapsedTime < swingDuration)
        {
            elapsedTime += Time.fixedDeltaTime;

            currentAngle += swingSpeed * Time.fixedDeltaTime;
            if (currentAngle >= 360f) currentAngle -= 360f;
            float angleInRadians = currentAngle * Mathf.Deg2Rad;

            Vector3 newSwingPosition = new Vector3(
                swingCenter.x + swingRadius * Mathf.Cos(angleInRadians),
                initialPosition.y,
                swingCenter.z + swingRadius * Mathf.Sin(angleInRadians)
            );

            otherPlayer.position = newSwingPosition;
            otherPlayer.rotation = Quaternion.LookRotation(newSwingPosition - swingCenter);

            Collider[] hitEnemiesDuringFrame = Physics.OverlapSphere(swingCenter, swingRadius, enemyLayer);
            foreach (Collider enemy in hitEnemiesDuringFrame)
            {
                // Check if enemy has been hit
                if (hitEnemies.Contains(enemy)) continue;

                hitEnemies.Add(enemy);

                BaseManager enemyManager = enemy.GetComponent<BaseManager>();
                if (enemyManager != null)
                {
                    enemyManager.DealDamageToEnemy(baseSwingDamage, BaseManager.DamageType.AbilityDamage);
                    Debug.Log("Hit enemy during swing: " + enemy.name + swingDamage);

                    // Apply knockback force
                    Rigidbody enemyRb = enemy.GetComponent<Rigidbody>();
                    if (enemyRb != null)
                    {
                        Vector3 knockbackDirection = (enemy.transform.position - swingCenter).normalized;
                        //enemyRb.AddForce(knockbackDirection * 20f, ForceMode.Impulse); // Adjust force value as needed
                    }
                        enemyManager.chainEffects.ActivateRagdollStun(3, gameObject, 100);
                }
            }

            yield return new WaitForFixedUpdate();
        }

        isSwinging = false;
        Debug.Log("Swing ended.");

        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
        anchorRb.isKinematic = false;
        otherPlayerRb.isKinematic = false;

        BIsPlayerCurrentlySwinging = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, swingRadius);
    }
}
