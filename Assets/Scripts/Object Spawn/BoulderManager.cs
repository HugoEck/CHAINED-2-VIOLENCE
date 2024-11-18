using System;
using System.Collections;
using UnityEngine;

public class BoulderManager : MonoBehaviour
{
    private float boulderDamage = 3f;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);

        // Check if the collided object is a player
        if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player != null)
            {
                Debug.Log("Player hit by boulder. Current health: " + player.currentHealth);

                // Apply damage to the player
                player.SetHealth(boulderDamage);

                // Attempt to apply ragdoll stun using chainEffects
                BaseManager baseManager = collision.gameObject.GetComponent<BaseManager>();
                if (baseManager != null && baseManager.chainEffects != null)
                {
                    Debug.Log("Activating ragdoll stun on: " + collision.gameObject.name);

                    // Activate the ragdoll stun for 4 seconds
                    baseManager.chainEffects.ActivateRagdollStun(4f);
                }
                else
                {
                    Debug.LogError("BaseManager or chainEffects not found on: " + collision.gameObject.name);
                }
            }
        }
    }

    private IEnumerator DeactivateRagdoll(BaseManager baseManager, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (baseManager != null && baseManager.behaviorMethods != null)
        {
            baseManager.behaviorMethods.ToggleRagdoll(false);
            Debug.Log("Ragdoll deactivated for: " + baseManager.gameObject.name);
        }
    }
}
