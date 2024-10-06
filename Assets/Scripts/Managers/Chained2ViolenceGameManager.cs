using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script handles all the game states, only persists in lobby and in game
/// </summary>
public class Chained2ViolenceGameManager : NetworkBehaviour
{
    public static Chained2ViolenceGameManager Instance { get; private set; }

    // Track if Player 1 has been assigned
    private static bool isPlayer1Assigned = false;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;

            // Immediately assign host to Player1
            AssignHostToPlayer1();
        }

    }
    public override void OnDestroy()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= NetworkManager_OnClientConnectedCallback;
        }
    }

    private void NetworkManager_OnClientConnectedCallback(ulong clientId)
    {
        // Check if the new client is not the host
        if (clientId != NetworkManager.Singleton.LocalClientId)
        {
            AssignClientToPlayer2(clientId);
        }
    }

    /// <summary>
    /// Assign host to player 1 object
    /// </summary>
    private void AssignHostToPlayer1()
    {
        if (!isPlayer1Assigned)
        {
            Player player1 = GameObject.FindGameObjectWithTag("Player1")?.GetComponent<Player>();

            if (player1 != null)
            {
                player1.OnGainedOwnership();
                isPlayer1Assigned = true; // Mark Player 1 as assigned
                Debug.Log("Player1 (host) has been assigned.");
            }
            else
            {
                Debug.LogError("Player1 not found in the scene!");
            }
        }
    }

    /// <summary>
    /// Assign joined client to the player 2 object
    /// </summary>
    /// <param name="clientId"></param>
    private void AssignClientToPlayer2(ulong clientId)
    {
        Player player2 = GameObject.FindGameObjectWithTag("Player2")?.GetComponent<Player>();
        
        if (player2 != null)
        {
            NetworkObject player2Object = player2.GetComponent<NetworkObject>();

            // Change ownership of Player2 to the client
            player2Object.ChangeOwnership(clientId);

            // Activate player movement for the newly assigned client
            player2.OnGainedOwnership();

            Debug.Log($"Player2 has been assigned to client {clientId}.");
        }
        else
        {
            Debug.LogError("Player2 not found in the scene!");
        }
    }
}


