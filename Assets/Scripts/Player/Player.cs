using System;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// This script handles updating all the player actions (i.e, movement, combat etc)
/// </summary>
public class Player : MonoBehaviour 
{
    // References for all player functionality (components) here
    #region Player components

    private PlayerMovement _playerMovement;
    private PlayerCombat _playerCombat;

    #endregion

    #region Player attributes

    [Header("Player attributes")]
    [SerializeField] private float _maxHealth = 10.0f;
    public float currentHealth { get; private set; }

    public int _playerId {  get; private set; }

    

    #endregion

    #region Inputs

    private Vector2 _movementInput = Vector2.zero;
    private Vector2 _rotationInput = Vector2.zero;
    private bool _bIsUsingBasicAttack = false;
    private bool _bIsUsingAbilityAttack = false;
    private bool _bIsUsingUltimateAttack = false;

    #endregion

    private static int playersDefeated = 0;
    
    private void Awake()
    {
        
        DontDestroyOnLoad(gameObject);

        if(gameObject.tag == "Player1")
        {
            _playerId = 1;
        }
        else if(gameObject.tag == "Player2")
        {
            _playerId = 2;
        }
    }

    private void OnDestroy()
    {
        Chained2ViolenceGameManager.Instance.OnGameStateChanged -= Chained2ViolenceGameManagerOnGameStateChanged;
        Chained2ViolenceGameManager.Instance.OnLobbyStateChanged -= Chained2ViolenceGameManagerOnLobbyStateChanged;
        Chained2ViolenceGameManager.Instance.OnSceneStateChanged -= Chained2ViolenceGameManagerOnSceneStateChanged;
    }

    void Start()
    {
        #region Instantiate components

        _playerMovement = GetComponent<PlayerMovement>();
        _playerCombat = GetComponent<PlayerCombat>();

        #endregion

        #region Set attributes

        currentHealth = _maxHealth;

        #endregion

        Chained2ViolenceGameManager.Instance.OnGameStateChanged += Chained2ViolenceGameManagerOnGameStateChanged;
        Chained2ViolenceGameManager.Instance.OnLobbyStateChanged += Chained2ViolenceGameManagerOnLobbyStateChanged;
        Chained2ViolenceGameManager.Instance.OnSceneStateChanged += Chained2ViolenceGameManagerOnSceneStateChanged;
    }
    private void FixedUpdate()
    {  
        UpdatePlayerMovement();      
    }
    private void Update()
    {
        GetPlayerMovementInput();
             
        UpdatePlayerCombat();

        HandleKnockout();
    }
    #region Player Movement
    private void UpdatePlayerMovement()
    {
        if (_playerMovement == null) return;

        _playerMovement.MovePlayer(_movementInput);

        if(_playerId == 1)
        {
            // Player 1 can use both gamepad and keyboard when player 2 hasn't joined
            if (InputManager.Instance.currentInputType == InputManager.InputType.Gamepad)
            {
                _playerMovement.RotatePlayerWithJoystick(_rotationInput);
            }
            else
            {
                _playerMovement.RotatePlayerToCursor();
            }
        }
        else if( _playerId == 2)
        {
            _playerMovement.RotatePlayerWithJoystick(_rotationInput);
        }
       
    }

    private void GetPlayerMovementInput()
    {
        if (_playerId == 1)
        {
            _movementInput = InputManager.Instance.GetMovementInput_P1();
            _rotationInput = InputManager.Instance.GetRotationInput_P1();
        }
        else if (_playerId == 2)
        {
            _movementInput = InputManager.Instance.GetMovementInput_P2();
            _rotationInput = InputManager.Instance.GetRotationInput_P2();
        }

    }

    #endregion

    #region Player Combat

    private void UpdatePlayerCombat()
    {
        GetPlayerCombatInput();
    }

    public Vector2 GetMovementInput()
    {
        return _movementInput;
    }

    private void GetPlayerCombatInput()
    {
        if (_playerId == 1)
        {
            _bIsUsingBasicAttack = InputManager.Instance.GetBasicAttackInput_P1();
            _bIsUsingAbilityAttack = InputManager.Instance.GetAbilityAttackInput_P1();
            _bIsUsingUltimateAttack = InputManager.Instance.GetUltimateAttackInput_P1();

            if (_bIsUsingBasicAttack)
            {
                _playerCombat.UseBaseAttack();
                Debug.Log("Player 1 is using basic attack");
                
            }
            else if(_bIsUsingAbilityAttack)
            {
                _playerCombat.UseAbility();
                Debug.Log("Player 1 is using Ability");
            }
            else if(_bIsUsingUltimateAttack)
            {
                UltimateAbilityManager.instance.UseUltimateAbilityPlayer1();
                Debug.Log("Player 1 is using Ultimate ability");
            }
        }
        else if (_playerId == 2)
        {
            _bIsUsingBasicAttack = InputManager.Instance.GetBasicAttackInput_P2();
            _bIsUsingAbilityAttack = InputManager.Instance.GetAbilityAttackInput_P2();
            _bIsUsingUltimateAttack = InputManager.Instance.GetUltimateAttackInput_P2();

            if (_bIsUsingBasicAttack)
            {
                _playerCombat.UseBaseAttack();
                Debug.Log("Player 2 is using basic attack");
            }
            else if(_bIsUsingAbilityAttack)
            {
                _playerCombat.UseAbility();
                Debug.Log("Player 2 is using Ability");
            }
            else if(_bIsUsingUltimateAttack)
            {
                UltimateAbilityManager.instance.UseUltimateAbilityPlayer2();
                Debug.Log("Player 2 is using Ultimate ability");
            }
        }
    }

    #endregion

    #region Player HP

    /// <summary>
    /// MAYBE SEPERATE SCRIPT FOR HANDLING PLAYER DEFEAT
    /// </summary>
    private void HandleKnockout()
    {
        if (Chained2ViolenceGameManager.Instance.currentSceneState != Chained2ViolenceGameManager.SceneState.ArenaScene) return;

        if(currentHealth <= 0)
        {
            if (playersDefeated == 2)
            {
                Chained2ViolenceGameManager.Instance.UpdateGamestate(Chained2ViolenceGameManager.GameState.GameOver);
                return;
            }

            playersDefeated++;
        }

        
    }

    public void SetHealth(float damage)
    {
        currentHealth -= damage;

        Debug.Log(gameObject.tag + " took: " +  damage + " damage" + ", current health = " + currentHealth);
    }

    //Used for upgrades
    public void SetMaxHealth(float newMaxHealth)
    {
        _maxHealth = newMaxHealth;
        currentHealth = _maxHealth;// heal to full when upgrading health

        if (currentHealth > _maxHealth)
        {
            currentHealth = _maxHealth;
        }

        Debug.Log("Player max health set to: " + _maxHealth);
    }

    #endregion

    #region Events

    private void Chained2ViolenceGameManagerOnGameStateChanged(Chained2ViolenceGameManager.GameState state)
    {
        if(state == Chained2ViolenceGameManager.GameState.Paused || state == Chained2ViolenceGameManager.GameState.GameOver)
        {
            _playerMovement.enabled = false;
            _playerCombat.enabled = false;
        }
        else if(state == Chained2ViolenceGameManager.GameState.Playing)
        {
            _playerMovement.enabled = true;
            _playerCombat.enabled = true;
        }
    }

    private void Chained2ViolenceGameManagerOnLobbyStateChanged(Chained2ViolenceGameManager.LobbyState state)
    {
        if (state == Chained2ViolenceGameManager.LobbyState.Paused)
        {
            _playerMovement.enabled = false;
            _playerCombat.enabled = false;
        }
        else if (state == Chained2ViolenceGameManager.LobbyState.Playing)
        {
            _playerMovement.enabled = true;
            _playerCombat.enabled = true;
        }
    }

    private void Chained2ViolenceGameManagerOnSceneStateChanged(Chained2ViolenceGameManager.SceneState state)
    {
        _playerMovement.enabled = true;
        _playerCombat.enabled = true;
    }

    #endregion
}



