using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour ////// THIS SCRIPT IS FOR TESTING PURPOSES ONLY
{
    [Header("Buttons")]
    [SerializeField] private Button _serverButton;
    [SerializeField] private Button _hostButton;
    [SerializeField] private Button _clientButton;

    private void Awake()
    {
        // Listen for which button is pressed and start eiter server, host or client 
        _serverButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
        });

        _hostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });

        _clientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
    }
}
