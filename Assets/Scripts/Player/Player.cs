using Unity.Netcode;
using UnityEngine;

/// <summary>
/// This script handles updating all the player actions (i.e, movement, combat etc)
/// </summary>
public class Player : NetworkBehaviour 
{
    // References for all player functionality (components) here
    #region Player components

    private PlayerMovement _playerMovement;

    #endregion

    private bool _bIsPlayerMovementActivated = false; // Used to enable player movement when players spawn

    void Start()
    {
        #region Instantiate components

        _playerMovement = GetComponent<PlayerMovement>();

        #endregion
    }
    private void FixedUpdate()
    {
        // Only allow movement if this player is the owner
        if (!IsOwner) return;

        UpdatePlayerMovement();
    }

    #region PlayerMovement
    private void UpdatePlayerMovement()
    {
        // Allow movement only if player movement is activated
        if (_bIsPlayerMovementActivated)
        {
            _playerMovement.MovePlayer();
        }
    }

    #endregion

    #region Player Spawn

    /// <summary>
    /// Only activate player movement after ownership has been properly transfered to other client
    /// </summary>
    public override void OnGainedOwnership()
    {
        base.OnGainedOwnership();
        Debug.Log("Gained ownership, activating player movement.");
        ActivatePlayerMovement();
    }

    /// <summary>
    /// Since the host takes ownership of both playerObjects at spawn, this method needs to be called in the playerSpawner script
    /// to ensure that the host can only move the player 1 object until he looses ownership of player 2 when a client joins
    /// </summary>
    public void ActivatePlayerMovement()
    {
        // Only activate movement for the owner
        if (IsOwner)
        {
            _bIsPlayerMovementActivated = true;
        }
    }

    #endregion
}



