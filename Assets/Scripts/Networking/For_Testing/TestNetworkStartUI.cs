using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TestNetworkStartUI : MonoBehaviour
{
    [SerializeField] private Button _startHostButton;
    [SerializeField] private Button _startClientButton;

    [SerializeField] private GameObject _testGameManager;
    private void Start()
    {
        _startHostButton.onClick.AddListener(StartHost);
        _startClientButton.onClick.AddListener(StartClient);
    }

    private void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        _testGameManager.gameObject.SetActive(true);
        Debug.Log("Host Started");
        Hide();
    }

    private void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        Debug.Log("Client Started");
        Hide();
    }
    private void Hide() => gameObject.SetActive(false);
}
