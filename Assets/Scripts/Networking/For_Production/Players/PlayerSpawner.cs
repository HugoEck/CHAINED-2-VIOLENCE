using Unity.Netcode;
using UnityEngine;

/// <summary>
/// This script handles assigning ownership of the player objects to the clients that join the game
/// </summary>
public class PlayerSpawner : NetworkBehaviour //// CAN BE IMPROVED (PROBABLY)
{
    [Header("Pre-spawned players")]
    [SerializeField] private NetworkObject[] _players;

    private int _currentPlayerIndex = 0;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected; // Listen for clients that connect to the game
    }

    // Event handler for client connection
    private void OnClientConnected(ulong clientId)
    {
        Debug.Log($"Client {clientId} connected.");
        
        // Only the server should handle assigning players to clients
        if (NetworkManager.Singleton.IsServer)
        {
            AssignPlayersToPlayerObjects(clientId);
        }
        
    }
    // Assign pre-spawned player object to a specific client
    private void AssignPlayersToPlayerObjects(ulong clientId)
    {
        if (_currentPlayerIndex >= _players.Length)
        {
            Debug.Log("No more player objects to assign.");
            return;
        }

        NetworkObject playerObject = _players[_currentPlayerIndex];
        
        // Avoid assigning ownership if client already is an owner of the object
        if(playerObject.OwnerClientId != clientId)
        {
            playerObject.ChangeOwnership(clientId);
        }
       
        // Activate player movement for the newly assigned client
        playerObject.GetComponent<Player>().ActivatePlayerMovement();

        // Increment to the next player object for the next client
        Debug.Log($"Assigned player object {_currentPlayerIndex} to client {clientId}.");
        
        _currentPlayerIndex++;
    }
}




