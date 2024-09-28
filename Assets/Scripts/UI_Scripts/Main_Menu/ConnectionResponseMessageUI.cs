using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionResponseMessageUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private Button _closeButton;

    private void Awake()
    {
        _closeButton.onClick.AddListener(Hide);
    }
    private void Start()
    {
        SteamLobby.Instance.OnFailedToJoinGame += Chained2ViolenceMultiplayer_OnFailedToJoinGame;

        Hide();
    }

    private void Chained2ViolenceMultiplayer_OnFailedToJoinGame(object sender, EventArgs e)
    {
        Show();

        _messageText.text = NetworkManager.Singleton.DisconnectReason; 

        if(_messageText.text == "")
        {
            _messageText.text = "Failed to connect";
        }
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
        SteamLobby.Instance.OnFailedToJoinGame -= Chained2ViolenceMultiplayer_OnFailedToJoinGame;
    }
}
