using UnityEngine;

/// <summary>
/// This is a script that manages the chain joint rotation
/// </summary>
public class ChainJointRotation : MonoBehaviour /////// FIX JITTERING FOR CLIENT CHAIN JOINT ROTATION
{
    [SerializeField] private Transform _otherPlayerChainJointTransform;

    private void FixedUpdate()
    {
        RotateChainJointTowardsPlayer();
    }

    /// <summary>
    /// Calculate the direction toward the other player and rotate there
    /// </summary>
    private void RotateChainJointTowardsPlayer()
    {
        if (_otherPlayerChainJointTransform == null)
        {
            Debug.LogWarning("Other player transform is not assigned!");
            return;
        }

        // Calculate the direction from the chain joint to the other player
        Vector3 directionToOtherPlayer = _otherPlayerChainJointTransform.position - transform.position;

        // Rotation should only happen in the XZ plane (the chain joint can't rotate upward or downard)
        directionToOtherPlayer.y = 0;

        // Rotate if there has been a change in direction
        if (directionToOtherPlayer != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToOtherPlayer);

            transform.rotation = targetRotation;

        }
    }
}
