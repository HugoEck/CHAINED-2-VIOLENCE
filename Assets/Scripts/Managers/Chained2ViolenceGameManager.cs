using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


/// <summary>
/// This script handles all the game states, only persists in lobby and in game
/// </summary>
public class Chained2ViolenceGameManager : MonoBehaviour
{
    public static Chained2ViolenceGameManager Instance { get; private set; }

    private static bool bIsPlayer2Assigned = false;

    private GameObject _player2GameObject;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    private void Start()
    {
        _player2GameObject = GameObject.FindGameObjectWithTag("Player2");
        _player2GameObject.GetComponent<Player>().enabled = false;
    }
    private void Update()
    {
        AssignPlayer2();
    }
    public void OnDestroy()
    {
        
    }
    public void AssignPlayer2()
    {
        if (bIsPlayer2Assigned) return;

        bIsPlayer2Assigned = InputManager.Instance.AssignPlayer2();
        if (bIsPlayer2Assigned)
        {
            _player2GameObject.GetComponent<Player>().enabled = true;
            Debug.Log("Player 2 joined the game");
        }
    }
    public bool BIsPlayer2Assigned
    {
        get
        {
            return bIsPlayer2Assigned;
        }
    }

}


