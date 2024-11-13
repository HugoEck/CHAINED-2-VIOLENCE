using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using static NPC_Customization;

/// <summary>
/// This script handles updating all the player actions (i.e, movement, combat etc)
/// </summary>
public class Player : MonoBehaviour 
{
    // References for all player functionality (components) here
    #region Player components

    private PlayerMovement _playerMovement;
    private PlayerCombat _playerCombat;
    private ShieldAbility _shieldAbility;
    private AnimationStateController _animationStateController;

    private Collider _playerCollider;

    #endregion

    #region Player attributes

    [Header("Player attributes")]
    [SerializeField] private float _maxHealth;
    public float currentHealth { get; private set; }
    public float InitialMaxHealth { get; private set; }

    GameObject player1Obj;
    GameObject player2Obj;
    public int _playerId {  get; private set; }

    private LayerMask _defaultIgnoreCollisionLayer;
    #endregion

    #region Inputs

    private Vector2 _movementInput = Vector2.zero;
    private Vector2 _rotationInput = Vector2.zero;
    private bool _bIsUsingBasicAttack = false;
    private bool _bIsUsingAbilityAttack = false;
    private bool _bIsUsingUltimateAttack = false;

    #endregion

    private bool _bIsPlayerDisabled = false;

    private static int playersDefeated = 0;

    public event Action<float> PlayerSpawnedIn;
    
    private void Awake()
    {          
        if(gameObject.tag == "Player1")
        {
            player1Obj = gameObject;
            _playerId = 1;
        }
        else if(gameObject.tag == "Player2")
        {
            player2Obj = gameObject;
            _playerId = 2;
            currentHealth = 0;
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
        _shieldAbility = GetComponent<ShieldAbility>(); // Get reference to the ShieldAbility
        _animationStateController = GetComponent<AnimationStateController>();

        _playerCollider = GetComponent<Collider>();

        #endregion

        #region Set attributes

        currentHealth = _maxHealth;

        if (_playerId == 1)
        {       
            //_maxHealth = StatsTransfer.Player1MaxHealth > 0 ? StatsTransfer.Player1MaxHealth : _maxHealth;
            //currentHealth = StatsTransfer.Player1Health > 0 ? StatsTransfer.Player1Health : _maxHealth;
           
            
            PlayerSpawnedIn?.Invoke(_maxHealth);
            _maxHealth = StatsTransfer.Player1Health;
        }
        else if (_playerId == 2)
        {
            //_maxHealth = StatsTransfer.Player2MaxHealth > 0 ? StatsTransfer.Player2MaxHealth : _maxHealth;
            //currentHealth = StatsTransfer.Player2Health > 0 ? StatsTransfer.Player2Health : _maxHealth;
            
            PlayerSpawnedIn?.Invoke(_maxHealth);
            _maxHealth = StatsTransfer.Player2Health;
        }

        InitialMaxHealth = _maxHealth;
        currentHealth = _maxHealth; 

        _defaultIgnoreCollisionLayer = _playerCollider.excludeLayers.value;

        #endregion

        Chained2ViolenceGameManager.Instance.OnGameStateChanged += Chained2ViolenceGameManagerOnGameStateChanged;
        Chained2ViolenceGameManager.Instance.OnLobbyStateChanged += Chained2ViolenceGameManagerOnLobbyStateChanged;
        Chained2ViolenceGameManager.Instance.OnSceneStateChanged += Chained2ViolenceGameManagerOnSceneStateChanged;

        StartCoroutine(DisablePlayerMovementTmp());
    }
    private void FixedUpdate()
    {
        if (_bIsPlayerDisabled) return;

        UpdatePlayerMovement();      
    }
    private void Update()
    {
        if (_bIsPlayerDisabled) return;

        GetPlayerMovementInput();
             
        UpdatePlayerCombat();

        HandleKnockout();

        GhostChainIgnoreCollision();
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

            if (_bIsUsingBasicAttack && _playerCombat.IsAttackAllowed())
            {
                _animationStateController.StartAttackAnimation();
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

            if (_bIsUsingBasicAttack && _playerCombat.IsAttackAllowed())
            {
                _animationStateController.StartAttackAnimation();
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

        if(Chained2ViolenceGameManager.Instance.BIsPlayer2Assigned)
        {
            if (currentHealth <= 0 && _playerId == 1 && !_bIsPlayerDisabled)
            {
                player1Obj.GetComponentInChildren<Animator>(false).enabled = false;
                player1Obj.GetComponent<CapsuleCollider>().enabled = false;
                _bIsPlayerDisabled = true;
                playersDefeated++;
            }
            else if (currentHealth <= 0 && _playerId == 2 && !_bIsPlayerDisabled)
            {
                player2Obj.GetComponentInChildren<Animator>(false).enabled = false;
                player2Obj.GetComponent<CapsuleCollider>().enabled = false;
                _bIsPlayerDisabled = true;
                playersDefeated++;
            }

            if (playersDefeated == 2)
            {                
                playersDefeated = 0;
                Chained2ViolenceGameManager.Instance.UpdateGamestate(Chained2ViolenceGameManager.GameState.GameOver);
            }
        }
        else
        {

            if (currentHealth <= 0 && _playerId == 1 && !_bIsPlayerDisabled)
            {
                player1Obj.GetComponentInChildren<Animator>(false).enabled = false;
                player1Obj.GetComponent<CapsuleCollider>().enabled = false;
                _bIsPlayerDisabled = true;
                playersDefeated++;
            }
            else if (currentHealth <= 0 && _playerId == 2 && !_bIsPlayerDisabled)
            {
                player2Obj.GetComponentInChildren<Animator>(false).enabled = false;
                player2Obj.GetComponent<CapsuleCollider>().enabled = false;
                _bIsPlayerDisabled = true;
            }

            if (playersDefeated == 1)
            {                
                playersDefeated = 0;
                Chained2ViolenceGameManager.Instance.UpdateGamestate(Chained2ViolenceGameManager.GameState.GameOver);             
            }
        }
        
    }

    public void SetHealth(float damage)
    {
        if (GhostChain._bIsGhostChainActive) return;

        // Check if the shield is active and absorb damage first
        if (_shieldAbility != null && _shieldAbility.IsShieldActive())
        {
            // Absorb the damage with the shield
            float remainingDamage = _shieldAbility.AbsorbDamage(damage);

            // If the shield completely absorbed the damage, exit the function
            if (remainingDamage <= 0)
            {
                Debug.Log("Shield absorbed all the damage.");
                return;
            }

            // If the shield breaks and there's leftover damage, apply it to the player's health
            damage = remainingDamage;
        }

        // Apply the remaining damage to the player's health
        currentHealth -= damage;
        Debug.Log(gameObject.tag + " took: " + damage + " damage, current health = " + currentHealth);

        if (currentHealth <= 0)
        {
            // Handle player's death here if needed
            //Debug.Log(gameObject.tag + " has died.");
        }
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

    public float GetMaxHealth()
    {
        return _maxHealth;
    }

    #endregion

    #region Events

    private void Chained2ViolenceGameManagerOnGameStateChanged(Chained2ViolenceGameManager.GameState state)
    {      

        if (state == Chained2ViolenceGameManager.GameState.Paused || state == Chained2ViolenceGameManager.GameState.GameOver)
        {
            _bIsPlayerDisabled = true;
        }
        else if(state == Chained2ViolenceGameManager.GameState.Playing)
        {           
            _bIsPlayerDisabled = false;
        }
    }

    private void Chained2ViolenceGameManagerOnLobbyStateChanged(Chained2ViolenceGameManager.LobbyState state)
    {
        if (state == Chained2ViolenceGameManager.LobbyState.Paused)
        {
            _bIsPlayerDisabled = true;
        }
        else if (state == Chained2ViolenceGameManager.LobbyState.Playing)
        {

            _bIsPlayerDisabled = false;
        }
    }

    private void Chained2ViolenceGameManagerOnSceneStateChanged(Chained2ViolenceGameManager.SceneState state)
    {
             
    }

    private IEnumerator DisablePlayerMovementTmp()
    {
        _bIsPlayerDisabled = true;

        yield return new WaitForSeconds(1);

        _bIsPlayerDisabled = false;
    }
    #endregion

    #region Ability chain Related Logic

    private void GhostChainIgnoreCollision()
    {
        
        if (GhostChain._bIsGhostChainActive)
        {
            _playerCollider.excludeLayers = GhostChain.ignoreCollisionLayers;
            
        }
        else
        {
            _playerCollider.excludeLayers = _defaultIgnoreCollisionLayer;
        }
       
    }

    #endregion
}



