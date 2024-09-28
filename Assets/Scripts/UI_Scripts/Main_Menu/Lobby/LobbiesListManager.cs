using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using Steamworks.Data;

/// <summary>
/// This script manages all of the found steam lobbies and controls main menu UI elements
/// </summary>
public class LobbiesListManager : MonoBehaviour
{
    public static LobbiesListManager Instance;

    [Header("Lobby UI")]
    [SerializeField] private GameObject _lobbyUI;
    [SerializeField] private GameObject _lobbyPrefab;
    [SerializeField] private GameObject _lobbyListContent;

    [Header("Menu buttons")]
    [SerializeField] private GameObject _createGameButton;
    [SerializeField] private GameObject _joinGameButton;

    public List<GameObject> _listOfLobbies { get; set; } = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    /// <summary>
    /// Close Lobby UI and activate menu buttons
    /// </summary>
    public void CloseListOfLobbies()
    {
        _lobbyUI.SetActive(false);

        _joinGameButton.SetActive(true);
        _createGameButton.SetActive(true);
    }

    /// <summary>
    /// Hides Menu buttons and displays lobbies
    /// </summary>
    public void GetListOfLobbies()
    {
        _joinGameButton.SetActive(false);
        _createGameButton.SetActive(false);

        _lobbyUI.SetActive(true);

        SteamLobby.Instance.GetLobbiesList();
    }

    /// <summary>
    /// Instantiate lobby prefab and set its data to found steam lobby
    /// </summary>
    /// <param name="lobbies"></param>
    public void DisplayLobbies(Lobby[] lobbies)
    {
        // Clear existing lobby UI
        if (_listOfLobbies.Count > 0)
        {
            DestroyLobbies();
        }

        // Loop through all lobbies passed in
        foreach (var lobby in lobbies)
        {
            GameObject createdLobby = Instantiate(_lobbyPrefab);

            createdLobby.GetComponent<LobbyDataEntry>().LobbyId = lobby.Id.Value; // Set Lobby ID
            createdLobby.GetComponent<LobbyDataEntry>().LobbyName = lobby.GetData("name"); // Get Lobby Name
            createdLobby.GetComponent<LobbyDataEntry>().AmountOfPlayers = lobby.MaxMembers;
            createdLobby.GetComponent<LobbyDataEntry>().PlayerJoined = lobby.MemberCount;
            createdLobby.GetComponent<LobbyDataEntry>().SetLobbyData(); // Set any additional lobby data if needed

            createdLobby.transform.SetParent(_lobbyListContent.transform);
            createdLobby.transform.localScale = Vector3.one;

            _listOfLobbies.Add(createdLobby);
        }
    }

    /// <summary>
    /// Destroy all the lobby objects to prevent duplicates in the Lobby UI
    /// </summary>
    public void DestroyLobbies()
    {
        foreach (GameObject lobby in _listOfLobbies)
        {
            Destroy(lobby);
        }
        _listOfLobbies.Clear();
    }
}

