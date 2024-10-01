using Netcode.Transports.Facepunch;
using Steamworks.Data;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using System;
using UnityEngine.UI;

/// <summary>
/// This script handles the steam connections using facepunch API (creating a game, joining a game etc)
/// </summary>
public class SteamLobby : MonoBehaviour
{
    public static SteamLobby Instance { get; private set; }

    #region ONLY FOR TESTING
    [SerializeField] private Chained2ViolenceGameManager _testGameManager; // THIS IS ONLY USED FOR TESTING 
    [SerializeField] private Canvas _createGameCanvasTest; // THIS IS ONLY USED FOR TESTING (DISABLING THE TESTING CANVAS IN THE TestStartHost method) 

    #endregion
    /// <summary>
    /// This is only used during testing, pick which scene should be loaded when game is started (The loaded scene only works if there are already player objects instantiated)
    /// </summary>
    [SerializeField] Loader.Scene _loadSceneForTesting; 

    private FacepunchTransport _transport; // Reference to facepunch transport which handles the netoworking packages
    public Lobby? CurrentLobby { get; private set; } // The current lobby that the player is currently in

    private const int MAX_NUMBER_OF_PLAYERS = 2;

    /// <summary>
    /// These events will trigger when client tries to join game (such as showing a text that says "Failed to join game" or "Connecting..."
    /// </summary>
    #region Connection events 

    public event EventHandler OnTryingToJoinGame;
    public event EventHandler OnFailedToJoinGame;

    #endregion

    public List<SteamId> LobbyIDs = new List<SteamId>();  

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    private void Start()
    {
        _transport = GetComponent<FacepunchTransport>();

        SteamMatchmaking.OnLobbyCreated += SteamMatchmaking_OnLobbyCreated;
        SteamMatchmaking.OnLobbyEntered += SteamMatchmaking_OnLobbyEntered;
        SteamMatchmaking.OnLobbyMemberJoined += SteamMatchmaking_OnLobbyMemberJoined;
        SteamMatchmaking.OnLobbyMemberLeave += SteamMatchmaking_OnLobbyMemberLeave;
        SteamMatchmaking.OnLobbyInvite += SteamMatchmaking_OnLObbyInvite;
        SteamMatchmaking.OnLobbyGameCreated += SteamMatchmaking_OnLobbyGameCreated;
        SteamFriends.OnGameLobbyJoinRequested += SteamFriends_OnGameLobbyJoinRequested;

        SteamMatchmaking.OnLobbyDataChanged += SteamMatchmaking_OnLobbyDataChanged;
    }

    /// <summary>
    /// Triggers when you accept an invite from a friend
    /// </summary>
    /// <param name="lobby"></param>
    /// <param name="id"></param>
    private async void SteamFriends_OnGameLobbyJoinRequested(Lobby lobby, SteamId id)
    {
        RoomEnter joinedLobby = await lobby.Join();

        if (joinedLobby != RoomEnter.Success)
        {
            Debug.Log("Failed to join lobby");
        }
        else
        {
            CurrentLobby = lobby;
            Debug.Log("Joined lobby");
        }
    }

    /// <summary>
    /// Triggers when you create a lobby
    /// </summary>
    /// <param name="lobby"></param>
    /// <param name="ip"></param>
    /// <param name="port"></param>
    /// <param name="id"></param>
    private void SteamMatchmaking_OnLobbyGameCreated(Lobby lobby, uint ip, ushort port, SteamId id)
    {
        Debug.Log("Lobby was created");
    }

    /// <summary>
    /// Triggers when you recieve an invite to the lobby from a friend
    /// </summary>
    /// <param name="friend"></param>
    /// <param name="lobby"></param>
    private void SteamMatchmaking_OnLObbyInvite(Friend friend, Lobby lobby)
    {
        Debug.Log($"Invite from {friend.Name}");
    }

    /// <summary>
    /// Triggers when a player leaves the lobby
    /// </summary>
    /// <param name="lobby"></param>
    /// <param name="friend"></param>
    private void SteamMatchmaking_OnLobbyMemberLeave(Lobby lobby, Friend friend)
    {
        Debug.Log("Player left lobby");
    }

    /// <summary>
    /// Triggers when a player joins the lobby
    /// </summary>
    /// <param name="lobby"></param>
    /// <param name="friend"></param>
    private void SteamMatchmaking_OnLobbyMemberJoined(Lobby lobby, Friend friend)
    {
        Debug.Log("Player joined lobby");
    }

    /// <summary>
    /// Triggers when a player successfully joins a lobby and starts their client
    /// </summary>
    /// <param name="lobby"></param>
    private void SteamMatchmaking_OnLobbyEntered(Lobby lobby)
    {
        if (NetworkManager.Singleton.IsHost)
        {
            return;
        }
        StartClient(CurrentLobby.Value.Owner.Id);
    }

    /// <summary>
    /// Triggers when a lobby is created and sets the lobby setting
    /// </summary>
    /// <param name="result"></param>
    /// <param name="lobby"></param>
    private void SteamMatchmaking_OnLobbyCreated(Result result, Lobby lobby)
    {
        if (result != Result.OK)
        {
            Debug.Log("Lobby was not created");
        }
        lobby.SetPublic();
        lobby.SetJoinable(true);
        lobby.SetGameServer(lobby.Owner.Id);
        lobby.SetData("name", lobby.Owner.Name);
        Debug.Log($"Lobby created {lobby.Owner.Name}");
    }

    /// <summary>
    /// Starts the host, creates a lobby and loads the game lobby scene
    /// </summary>
    public async void StartHost()
    {
        NetworkManager.Singleton.OnServerStarted += NetworkManager_OnServerStarted;

        NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_ConnectionApprovalCallback;

        NetworkManager.Singleton.StartHost();

        CurrentLobby = await SteamMatchmaking.CreateLobbyAsync(MAX_NUMBER_OF_PLAYERS);

        Loader.LoadNetwork(_loadSceneForTesting); // SWITCH OUT _loadSceneForTesting FOR PRODUCTION WITH PRE DETERMINED SCENE TO LOAD
    }
    /// <summary>
    /// THIS METHOD IS ONLY USED FOR TESTING; IT USES THE _testGameManager reference
    /// </summary>
    public async void TestStartHost()
    {
        
        NetworkManager.Singleton.OnServerStarted += NetworkManager_OnServerStarted;

        NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_ConnectionApprovalCallback;
        
        NetworkManager.Singleton.StartHost();

        _testGameManager.gameObject.SetActive(true);
        CurrentLobby = await SteamMatchmaking.CreateLobbyAsync(MAX_NUMBER_OF_PLAYERS);

        _createGameCanvasTest.gameObject.SetActive(false);
    }
    private void NetworkManager_OnServerStarted()
    {
       
    }

    /// <summary>
    /// Starts the client
    /// </summary>
    /// <param name="steamId"></param>
    public void StartClient(SteamId steamId)
    {
        OnTryingToJoinGame?.Invoke(this, EventArgs.Empty);

        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        _transport.targetSteamId = steamId;

        if (NetworkManager.Singleton.StartClient())
        {
            Debug.Log("Client has started");

            // Check if the client has joined a lobby
            if (CurrentLobby != null)
            {
                Debug.Log($"Client has joined lobby with ID: {CurrentLobby.Value.Id.Value}, Name: {CurrentLobby.Value.GetData("name")}");
            }
            else
            {
                Debug.Log("No lobby information available");
            }
        }
    }

    /// <summary>
    /// Handles successful client connection
    /// </summary>
    /// <param name="obj"></param>
    private void NetworkManager_OnClientConnectedCallback(ulong obj)
    {
        
    }

    /// <summary>
    /// Handles client disconnections
    /// </summary>
    /// <param name="clientId"></param>
    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        OnFailedToJoinGame?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Used to reject or approve client connections to server, examples of this is rejecting client connection when lobby is full
    /// </summary>
    /// <param name="connectionApprovalRequest"></param>
    /// <param name="connectionApprovalResponse"></param>
    private void NetworkManager_ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest, NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
    {

        //////////////////////////// USE THIS WHEN YOU CAN PLAY THE GAME TO NOT ALLOW PLAYERS TO JOIN AFTER GAME HAS STARTED
        //if(SceneManager.GetActiveScene().name != Loader.Scene.ArnTestStartGameScene.ToString())
        //{
        //    connectionApprovalResponse.Approved = false;
        //    connectionApprovalResponse.Reason = "Game has already started";
        //    return;
        //}

        if (NetworkManager.Singleton.ConnectedClientsIds.Count >= MAX_NUMBER_OF_PLAYERS)
        {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Game is full";
            return;
        }
        ///// CHECK IF GAME HAS ALREADY STARTED OR IF LOBBY IS FULL IN THE FUTURE TO REJECT CONNECTION TO GAME
        connectionApprovalResponse.Approved = true;
    }

    /// <summary>
    /// Called when the player disconnects from the game
    /// </summary>
    public void Disconnected()
    {
        CurrentLobby?.Leave();
        if (NetworkManager.Singleton == null)
        {
            return;
        }

        if(NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.OnServerStarted -= NetworkManager_OnServerStarted;
        }
        else
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= NetworkManager_OnClientConnectedCallback;
        }

        NetworkManager.Singleton.Shutdown(true);
        Debug.Log("Disconnected");
    }
    private void OnApplicationQuit()
    {
        Disconnected();
    }

    /// <summary>
    /// Unsubscribe from all the events to prevent memory leaks
    /// </summary>
    private void OnDestroy()
    {
        SteamMatchmaking.OnLobbyCreated -= SteamMatchmaking_OnLobbyCreated;
        SteamMatchmaking.OnLobbyEntered -= SteamMatchmaking_OnLobbyEntered;
        SteamMatchmaking.OnLobbyMemberJoined -= SteamMatchmaking_OnLobbyMemberJoined;
        SteamMatchmaking.OnLobbyMemberLeave -= SteamMatchmaking_OnLobbyMemberLeave;
        SteamMatchmaking.OnLobbyInvite -= SteamMatchmaking_OnLObbyInvite;
        SteamMatchmaking.OnLobbyGameCreated -= SteamMatchmaking_OnLobbyGameCreated;
        SteamFriends.OnGameLobbyJoinRequested -= SteamFriends_OnGameLobbyJoinRequested;

        if(NetworkManager.Singleton == null)
        {
            return;
        }
        NetworkManager.Singleton.OnServerStarted -= NetworkManager_OnServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback -= NetworkManager_OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback -= NetworkManager_OnClientDisconnectCallback;
    }

    /// <summary>
    /// Searches for all the active lobbies (up to 50) and displays them in the lobby UI
    /// </summary>
    public async void GetLobbiesList()
    {
        // Clear any existing lobby IDs
        if (LobbyIDs.Count > 0)
        {
            LobbyIDs.Clear();
        }

        // Create a query for lobby list with a maximum result count
        var query = SteamMatchmaking.LobbyList
            .WithMaxResults(50); // Limit to 50 results

        // Request the lobby list asynchronously
        var lobbies = await query.RequestAsync();

        OnGetLobbiesList(lobbies);
    }
    private void OnGetLobbiesList(Lobby[] lobbies)
    {
        Debug.Log($"Total Lobbies Found: {lobbies.Length}");

        // Call DisplayLobbies and pass all the retrieved lobbies
        LobbiesListManager.Instance.DisplayLobbies(lobbies);
    }

    /// <summary>
    /// Triggers when any data associated with the lobby is changed, such as player count or lobby name
    /// </summary>
    /// <param name="lobby"></param>
    private void SteamMatchmaking_OnLobbyDataChanged(Lobby lobby)
    {
        
    }
}
