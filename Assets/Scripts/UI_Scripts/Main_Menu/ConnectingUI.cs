using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectingUI : MonoBehaviour
{
    private void Start()
    {
        SteamLobby.Instance.OnTryingToJoinGame += Chained2ViolenceMultiplayer_OnTryingToJoinGame;
        SteamLobby.Instance.OnFailedToJoinGame += Chained2ViolenceMultiplayer_OnFailedToJoinGame;

        Hide();
    }

    private void Chained2ViolenceMultiplayer_OnFailedToJoinGame(object sender, EventArgs e)
    {
        Hide();
    }

    private void Chained2ViolenceMultiplayer_OnTryingToJoinGame(object sender, EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        SteamLobby.Instance.OnTryingToJoinGame -= Chained2ViolenceMultiplayer_OnTryingToJoinGame;
        SteamLobby.Instance.OnFailedToJoinGame -= Chained2ViolenceMultiplayer_OnFailedToJoinGame;
    }
}
