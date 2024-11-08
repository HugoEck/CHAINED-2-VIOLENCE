using UnityEngine;
using static Chained2ViolenceGameManager;


public class ClassManager : MonoBehaviour
{
    public static ClassManager Instance { get; private set; }

    private PlayerCombat _player1;
    private PlayerCombat _player2;

    public static PlayerCombat.PlayerClass _currentPlayer1Class;
    public static PlayerCombat.PlayerClass _currentPlayer2Class;

    private void Awake()
    {      
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializePlayers();
    }   

    private void InitializePlayers()
    {
        _player1 = GameObject.FindGameObjectWithTag("Player1")?.GetComponent<PlayerCombat>();
        if (_player1 != null)
        {
            _currentPlayer1Class = _player1.currentPlayerClass;        
        }

        _player2 = GameObject.FindGameObjectWithTag("Player2")?.GetComponent<PlayerCombat>();
        if (_player2 != null)
        {
            _currentPlayer2Class = _player2.currentPlayerClass;       
        }
    }

    private void Update()
    {
        if (_player1 == null || _player2 == null)
        {
            InitializePlayers(); // Reinitialize the players between scenes so that the current players are retrieved
        }
    }
}

