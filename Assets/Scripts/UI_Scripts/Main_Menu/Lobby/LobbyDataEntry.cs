using Steamworks;
using Steamworks.Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class LobbyDataEntry : MonoBehaviour
{
    [Header("Lobby Data")]
    [SerializeField] private TextMeshProUGUI _lobbyNameText;
    [SerializeField] private TextMeshProUGUI _membersInLobby;
    public SteamId LobbyId { get; set; }

    public string LobbyName { get; set; }

    public int AmountOfPlayers { get; set; }

    public int PlayerJoined { get; set; }

    public void SetLobbyData()
    {
        if(LobbyName == "")
        {
            _lobbyNameText.text = "Empty";
        }
        else
        {
            _lobbyNameText.text = LobbyName;
        }      
        _membersInLobby.text = PlayerJoined.ToString() + " / " + AmountOfPlayers.ToString();
    }

    public async void JoinLobby()
    {
        // Join the lobby before starting the client
        var lobby = new Lobby(LobbyId);
        var result = await lobby.Join();

        if (result == RoomEnter.Success)
        {
            
            Debug.Log($"Successfully joined lobby: {lobby.Id}");

            SteamLobby.Instance.StartClient(lobby.Owner.Id);  
        }
        else
        {
            Debug.LogError("Failed to join the lobby.");
        }
    }

}
