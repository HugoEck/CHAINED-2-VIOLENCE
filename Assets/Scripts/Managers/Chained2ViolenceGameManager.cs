using UnityEngine;


/// <summary>
/// This script handles all the game states, only persists in lobby and in game
/// </summary>
public class Chained2ViolenceGameManager : MonoBehaviour
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
        
    }
    public void OnDestroy()
    {
        
    }

}


