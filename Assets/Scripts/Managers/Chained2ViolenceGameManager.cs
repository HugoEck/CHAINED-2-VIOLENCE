using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


/// <summary>
/// This script handles all the game states, only persists in lobby and in game
/// </summary>

public class Chained2ViolenceGameManager : MonoBehaviour
{
    public enum LobbyState
    {
        Playing,
        Paused,

        Upgrading
    };

    public enum SceneState
    {
        LobbyScene,
        ArenaScene
    };

    public enum GameState
    {
        Playing,
        Paused,

        NextWave,
        AllWavesDefeated,
        BossDefeated,

        GameOver
    };

    public static Chained2ViolenceGameManager Instance { get; private set; }

    [HideInInspector]
    public SceneState currentSceneState = SceneState.LobbyScene;

    [HideInInspector]
    public GameState currentGameState = GameState.Playing;

    [HideInInspector]
    public LobbyState currentLobbyState = LobbyState.Playing;

    #region State events

    public event Action<GameState> OnGameStateChanged;
    public event Action<SceneState> OnSceneStateChanged;
    public event Action<LobbyState> OnLobbyStateChanged;

    #endregion

    private static bool bIsPlayer2Assigned = false;

    private GameObject _player2GameObject;
    DissolveManager arenaShader;
    private float _gameDurationTimer;

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
        }


    }

    private void Start()
    {
        _player2GameObject = GameObject.FindGameObjectWithTag("Player2");
        _player2GameObject.GetComponent<Player>().enabled = false;



    }
    private void Update()
    {
        AssignPlayer2();
        if (arenaShader == null && currentSceneState == SceneState.ArenaScene)
        {
            arenaShader = GameObject.FindAnyObjectByType<DissolveManager>();
        }

        if (!bIsPlayer2Assigned)
        {
            _player2GameObject = GameObject.FindGameObjectWithTag("Player2");
           
            _player2GameObject.GetComponentInChildren<Animator>(false).enabled = false;
            _player2GameObject.GetComponent<CapsuleCollider>().enabled = false;
        }
    }

    #region State management

    public void UpdateGamestate(GameState newGameState)
    {
        if (currentSceneState == SceneState.LobbyScene)
        {
            Debug.Log("You can't call UpdateGameState from the Lobby Scene");

            return;
        }

        currentGameState = newGameState;

        switch (currentGameState)
        {
            case GameState.Playing:

                // Special handling for playing state

                HandlePlayingState();

                break;

            case GameState.Paused:

                // Special handling for paused state

                HandlePausedState();

                break;

            case GameState.NextWave:

                // Special handling for Next wave state

                break;

            case GameState.AllWavesDefeated:

                // Special handling for AllWavesDefeated state

                break;

            case GameState.BossDefeated:

                // Special handling for BossDefeated state

                break;

            case GameState.GameOver:

                // Special handling for when the players have lost the game 

                HandleGameOverState();

                break;
        }

        OnGameStateChanged?.Invoke(newGameState);
    }



    public void UpdateSceneState(SceneState newSceneState)
    {
        currentSceneState = newSceneState;

        switch (currentSceneState)
        {
            case SceneState.LobbyScene:

                // Special handling for when the players are in the lobby scene

                break;

            case SceneState.ArenaScene:

                // Special handling for when the players are in the arena
                SetArenaStartTime();
                break;
        }

        OnSceneStateChanged?.Invoke(newSceneState);
    }
    public void UpdateLobbyState(LobbyState newLobbyState)
    {
        if (currentSceneState == SceneState.ArenaScene)
        {
            Debug.Log("You can't call UpdateLobbyState from the Arena scene");
        }

        currentLobbyState = newLobbyState;

        switch (currentLobbyState)
        {
            case LobbyState.Playing:

                // Should call the same handling method as in UpdateGameState "Playing"

                break;

            case LobbyState.Paused:

                // Should call the same handling method as in UpdateGameState "Paused"

                break;


            case LobbyState.Upgrading:

                // Special handling for when the players are upgrading 

                break;
        }

        OnLobbyStateChanged?.Invoke(newLobbyState);
    }


    #region State Handling

    private void HandlePlayingState()
    {
        Time.timeScale = 1;

        Debug.Log("Game is now in play state");

        // Show playing UI
    }

    private void HandlePausedState()
    {
        Time.timeScale = 0;

        Debug.Log("Game is now in pause state");

        // Show paused UI
    }

    private void HandleGameOverState()
    {

        Debug.Log("Game over");
        // SOME KIND OF WAITING MECHANISM OR BUTTONPRESS SO THAT THE PLAYERS AREN'T IMMEDIATELY TRANSFERRED TO LOBBY SCENE

        arenaShader.ResetMaterials();

        Loader.LoadScene(Loader.Scene.LobbyScene);

        UpdateSceneState(SceneState.LobbyScene);
    }

    #endregion


    #endregion

    public void OnDestroy()
    {

    }

    #region Assign player 2
    public void AssignPlayer2()
    {
        if (bIsPlayer2Assigned || currentSceneState == SceneState.ArenaScene) return;

        bIsPlayer2Assigned = InputManager.Instance.AssignPlayer2();
        if (bIsPlayer2Assigned)
        {
            _player2GameObject.GetComponent<Player>().enabled = true;
            _player2GameObject.GetComponentInChildren<Animator>(false).enabled = true;
            _player2GameObject.GetComponent<CapsuleCollider>().enabled = true;

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

    #endregion

    #region Player UI Stuff
    public void SetArenaStartTime()
    {
        _gameDurationTimer = Time.time;
    }

    public float GetElapsedTimeInArena()
    {
        return Time.time - _gameDurationTimer;
    }
    #endregion

}


