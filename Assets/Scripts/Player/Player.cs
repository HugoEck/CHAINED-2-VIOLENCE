using HighlightPlus;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static NPC_Customization;

/// <summary>
/// This script handles updating all the player actions (i.e, movement, combat etc)
/// </summary>
public class Player : MonoBehaviour
{
    // References for all player functionality (components) here
    //#region Player Sound Effects

    //[Header("Take Damage SoundArray: ")]
    //[SerializeField] private AudioClip[] takeDamageSounds;

    //[Header("Respawn Sound: ")]
    //[SerializeField] private AudioClip respawnSound;

    //[Header("Player Death Sound: ")]
    //[SerializeField] private AudioClip playerDeathSound;

    //#endregion

    #region Player components

    private PlayerMovement _playerMovement;
    private PlayerCombat _playerCombat;
    private ShieldAbility _shieldAbility;
    private AnimationStateController _animationStateController;
    private ClassSelector _classSelector;
    private WeaponManager _weaponManager;
    private HighlightEffect _highlightEffect;

    private Collider _playerCollider;
    private Rigidbody _playerRigidbody;
    [SerializeField] private PlayerAttributes playerAttributes;
    RigidBodyMassManager rigidBodyMassManager;

    #endregion

    #region Player attributes

    public float currentHealth { get; private set; }

    GameObject player1Obj;
    GameObject player2Obj;
    public int _playerId { get; private set; }

    private LayerMask _defaultIgnoreCollisionLayer;
    #endregion

    #region Inputs

    private Vector2 _movementInput = Vector2.zero;
    private Vector2 _rotationInput = Vector2.zero;
    private bool _bIsUsingBasicAttack = false;
    private bool _bIsUsingAbilityAttack = false;
    private bool _bIsUsingUltimateAttack = false;

    #endregion

    // Variables for regeneration tracking
    private float regenerationCooldown = 5f; // Time to wait before regenerating
    private float timeSinceLastCombatAction_Player1 = 0f; // Timer for Player 1
    private float timeSinceLastCombatAction_Player2 = 0f; // Timer for Player 2
    [SerializeField] private float regenerationRate = 10f; // Health regenerated per second

    public bool _bIsPlayerDisabled = false;
    public bool bHasPlayerEnteredCombat = false;

    private static int playersDefeated = 0;

    public event Action<float> PlayerSpawnedIn;

    private float _respawnCooldown = 10;
    private float _respawnTime;

    // Arrays to store the found colliders
    private List<Collider> capsuleColliders = new List<Collider>();
    private List<Collider> boxColliders = new List<Collider>();


    //Flash indication
    [HideInInspector] private Material material; // Assign the material in the Inspector
    [HideInInspector] private Color flashColor = Color.white; // The color you want it to flash
    [HideInInspector] private float flashDuration = 0.1f; // Duration of the flash
    [HideInInspector] private Color originalColor;
    [HideInInspector] public bool isFlashing = false;
    Renderer renderer;

    public VignetteEffect vignetteEffect;
    private AudioClipManager audioClipManager;

    private void Awake()
    {
        if (gameObject.tag == "Player1")
        {
            player1Obj = gameObject;
            _playerId = 1;
        }
        else if (gameObject.tag == "Player2")
        {
            player2Obj = gameObject;
            _playerId = 2;
            currentHealth = 0;
        }

        playerAttributes.SetPlayerId(_playerId);
    }

    private void OnDestroy()
    {
        Chained2ViolenceGameManager.Instance.OnGameStateChanged -= Chained2ViolenceGameManagerOnGameStateChanged;
        Chained2ViolenceGameManager.Instance.OnLobbyStateChanged -= Chained2ViolenceGameManagerOnLobbyStateChanged;
        Chained2ViolenceGameManager.Instance.OnSceneStateChanged -= Chained2ViolenceGameManagerOnSceneStateChanged;
        _classSelector.OnClassSwitched -= PlayerCombatOnClassSwitched;
    }

    void Start()
    {
        #region Instantiate components

        _playerMovement = GetComponent<PlayerMovement>();
        _playerCombat = GetComponent<PlayerCombat>();
        _shieldAbility = GetComponent<ShieldAbility>(); // Get reference to the ShieldAbility
        _animationStateController = GetComponent<AnimationStateController>();
        _classSelector = GetComponent<ClassSelector>();

        _playerCollider = GetComponent<Collider>();
        _playerRigidbody = GetComponent<Rigidbody>();
        rigidBodyMassManager = GetComponent<RigidBodyMassManager>();
        _weaponManager = GetComponent<WeaponManager>();
        _highlightEffect = GetComponent<HighlightEffect>();
        #endregion

        #region Set attributes



        _defaultIgnoreCollisionLayer = _playerCollider.excludeLayers.value;

        #endregion

        Chained2ViolenceGameManager.Instance.OnGameStateChanged += Chained2ViolenceGameManagerOnGameStateChanged;
        Chained2ViolenceGameManager.Instance.OnLobbyStateChanged += Chained2ViolenceGameManagerOnLobbyStateChanged;
        Chained2ViolenceGameManager.Instance.OnSceneStateChanged += Chained2ViolenceGameManagerOnSceneStateChanged;
        _classSelector.OnClassSwitched += PlayerCombatOnClassSwitched;

        //StartCoroutine(DisablePlayerMovementTmp());

        // Find and add all CapsuleColliders and BoxColliders to the arrays
        FindAndStoreColliders(transform);

        // Turn off the colliders (disable their components)
        if (_playerId == 1)
            DisableColliders();
        else
            EnableColliders();

        playerAttributes.SetBaseValues(_playerCombat.currentPlayerClass);
        
        if ( _playerId == 1)
        {
            StatsTransfer.Instance.SaveStatsPlayer1(playerAttributes);
            _playerRigidbody.mass = playerAttributes.mass;

        }
        else if (_playerId == 2)
        {
            StatsTransfer.Instance.SaveStatsPlayer2(playerAttributes);
            _playerRigidbody.mass = playerAttributes.mass;
        }

        currentHealth = playerAttributes.maxHP;

        if(_playerId == 2)
        {
            if (!Chained2ViolenceGameManager.Instance.BIsPlayer2Assigned)
            {
                _bIsPlayerDisabled = true;
            }
        }
       
        if(Chained2ViolenceGameManager.Instance.currentSceneState != Chained2ViolenceGameManager.SceneState.LobbyScene)
        {
            if(!_bIsPlayerDisabled)
            {
                _playerRigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;

            }        
        }

        vignetteEffect = FindAnyObjectByType<VignetteEffect>();
        audioClipManager = FindAnyObjectByType<AudioClipManager>();
    }

    private void FixedUpdate()
    {
        if (_bIsPlayerDisabled) return;

        UpdatePlayerMovement();
    }
    private void Update()
    {
        HandleKnockout();
        UpdateHealthBar();

        if (_bIsPlayerDisabled) return;

        // Increment inactivity timers for each player
        if (_playerId == 1)
        {
            if(bHasPlayerEnteredCombat)
            {
                ResetCombatInactivityTimer(1);
                timeSinceLastCombatAction_Player1 += Time.deltaTime;
            }
            
            // Check if Player 1 is eligible for regeneration
            if (timeSinceLastCombatAction_Player1 >= regenerationCooldown)
            {
                bHasPlayerEnteredCombat = false;
                bHasResetInactivityTimer = false;
                RegenerateHealth(1);
            }
        }
        else if (_playerId == 2)
        {
            if(bHasPlayerEnteredCombat)
            {
                ResetCombatInactivityTimer(2);
                timeSinceLastCombatAction_Player2 += Time.deltaTime;
            }
            
            // Check if Player 2 is eligible for regeneration
            if (timeSinceLastCombatAction_Player2 >= regenerationCooldown)
            {
                bHasPlayerEnteredCombat = false;
                bHasResetInactivityTimer = false;
                RegenerateHealth(2);
            }
        }

        GetPlayerMovementInput();

        UpdatePlayerCombat();
        CheckIfCurrentHPExceedsMaxHP();  
        TakeDebugDamage(); // Used to kill players for debug, remove later.
        GhostChainIgnoreCollision();
        GetDropWeaponInput();
    }
    #region Player Movement
    private void UpdatePlayerMovement()
    {
        if (_playerMovement == null) return;

        _playerMovement.MovePlayer(_movementInput);

        if (_playerId == 1)
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
        else if (_playerId == 2)
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

    private void GetDropWeaponInput()
    {
        if (_playerId == 1 && InputManager.Instance.GetDropWeaponInput_P1())
        {
            _weaponManager.DropWeapon();

        }
        if (_playerId == 2 && InputManager.Instance.GetDropWeaponInput_P2())
        {
            _weaponManager.DropWeapon();
        }
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
            else if (_bIsUsingAbilityAttack)
            {
                _playerCombat.UseAbility();
                Debug.Log("Player 1 is using Ability");
            }
            else if (_bIsUsingUltimateAttack)
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
            else if (_bIsUsingAbilityAttack)
            {
                _playerCombat.UseAbility();
                Debug.Log("Player 2 is using Ability");
            }
            else if (_bIsUsingUltimateAttack)
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
    public void ToggleRagdoll(bool turnOn, GameObject player)
    {
        if(turnOn)
        {
            player.GetComponentInChildren<Animator>(false).enabled = false;
            player.GetComponent<CapsuleCollider>().enabled = false;
        }
        else if(!turnOn)
        {
            player.GetComponentInChildren<Animator>(false).enabled = true;
            player.GetComponent<CapsuleCollider>().enabled = true;
        }
    }
    
    private void HandleKnockout()
    {
        if (Chained2ViolenceGameManager.Instance.currentSceneState != Chained2ViolenceGameManager.SceneState.ArenaScene) return;

        if (Chained2ViolenceGameManager.Instance.BIsPlayer2Assigned)
        {
            if (currentHealth <= 0 && _playerId == 1 && !_bIsPlayerDisabled)
            {
                rigidBodyMassManager.RestoreOriginalMasses();

                if(audioClipManager.playerDeath != null)
                {                
                SFXManager.instance.PlaySFXClip(audioClipManager.playerDeath, transform, 1f);
                }

                ToggleRagdoll(true, player1Obj);
                _bIsPlayerDisabled = true;
                playersDefeated++;

            }
            else if (currentHealth <= 0 && _playerId == 2 && !_bIsPlayerDisabled)
            {
                rigidBodyMassManager.RestoreOriginalMasses();
                if (audioClipManager.playerDeath != null)
                {
                    SFXManager.instance.PlaySFXClip(audioClipManager.playerDeath, transform, 1f);
                }

                ToggleRagdoll(true, player2Obj);
                _bIsPlayerDisabled = true;
                playersDefeated++;
            }

            if (_bIsPlayerDisabled)
            {
                EnableColliders();
                Respawn();
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
                if (audioClipManager.playerDeath != null)
                {
                    SFXManager.instance.PlaySFXClip(audioClipManager.playerDeath, transform, 1f);
                }

                ToggleRagdoll(true, player1Obj);
                _bIsPlayerDisabled = true;
                playersDefeated++;

            }

            if (playersDefeated == 1)
            {
                playersDefeated = 0;
                Chained2ViolenceGameManager.Instance.UpdateGamestate(Chained2ViolenceGameManager.GameState.GameOver);
                HealthBar.Instance.ResetPlayerHealthBars();
            }
        }

    }

   

    private bool respawnTimerSet = false;
    private void Respawn()
    {
        if (!respawnTimerSet)
        {
            _respawnTime = _respawnCooldown;
            respawnTimerSet = true;
        }

        _respawnTime -= Time.deltaTime;

        if (_respawnTime <= 0)
        {
            playersDefeated--;
            currentHealth = playerAttributes.maxHP;
            _bIsPlayerDisabled = false;
            respawnTimerSet = false;
            if (audioClipManager.playerRespawn != null)
            {
            SFXManager.instance.PlaySFXClip(audioClipManager.playerRespawn, transform, 1f);
            }

            if (_playerId == 1)
            {
                ToggleRagdoll(false, player1Obj);
                rigidBodyMassManager.SetMassesToZero();
                DisableColliders();
                

            }
            else if (_playerId == 2)
            {
                ToggleRagdoll(false, player2Obj);
                rigidBodyMassManager.SetMassesToZero();
                DisableColliders();
            }
        }
    }

    public void SetHealth(float damage)
    {
        Debug.Log("Dealt "+damage);
        if (GhostChain._bIsGhostChainActive) return;

        vignetteEffect.TriggerVignette(_playerId);

        bHasPlayerEnteredCombat = true;


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

        if (audioClipManager.takeDamagePlayer != null)
        {
        SFXManager.instance.PlayRandomSFXClip(audioClipManager.takeDamagePlayer, transform, 1f);
        }
        // Apply the remaining damage to the player's health
        currentHealth -= damage;
        Debug.Log(gameObject.tag + " took: " + damage + " damage, current health = " + currentHealth);

        if (currentHealth <= 0)
        {

            // Handle player's death here if needed
            //Debug.Log(gameObject.tag + " has died.");
        }
        _highlightEffect.HitFX();
        //Flash indication

    }

    public void UpdateHealthBar()
    {
        if (HealthBar.Instance != null)
        {
            HealthBar.Instance.UpdateHealthBar(_playerId, currentHealth, GetMaxHealth());
        }
    }

    private void CheckIfCurrentHPExceedsMaxHP()
    {
        if (_playerId == 1)
        {
            if (currentHealth > playerAttributes.maxHP)
            {
                currentHealth = playerAttributes.maxHP;
            }
        }
        else if (_playerId == 2)
        {
            if (currentHealth > playerAttributes.maxHP)
            {
                currentHealth = playerAttributes.maxHP;
            }
        }
    }

    // Method to regenerate health
    // Method to regenerate health for a specific player
    private void RegenerateHealth(int playerId)
    {
        if (playerId == 1 && currentHealth < playerAttributes.maxHP)
        {
            if(!bHasPlayerEnteredCombat)
            {
                currentHealth += regenerationRate * Time.deltaTime;
                currentHealth = Mathf.Clamp(currentHealth, 0, playerAttributes.maxHP);

                UpdateHealthBar();
            }
            
            //Debug.Log($"Player 1 regenerated health: {currentHealth}/{playerAttributes.maxHP}");
        }
        else if (playerId == 2 && currentHealth < playerAttributes.maxHP)
        {
            if (!bHasPlayerEnteredCombat)
            {
                currentHealth += regenerationRate * Time.deltaTime;
                currentHealth = Mathf.Clamp(currentHealth, 0, playerAttributes.maxHP);
            }
                
            UpdateHealthBar();
            //Debug.Log($"Player 2 regenerated health: {currentHealth}/{playerAttributes.maxHP}");
        }
    }

    private bool bHasResetInactivityTimer = false;
    // Method to reset the combat inactivity timer for a specific player
    private void ResetCombatInactivityTimer(int playerId)
    {
        if (bHasResetInactivityTimer) return;

        if (playerId == 1)
        {
            bHasResetInactivityTimer = true;
            timeSinceLastCombatAction_Player1 = 0f;
        }
        else if (playerId == 2)
        {
            bHasResetInactivityTimer = true;
            timeSinceLastCombatAction_Player2 = 0f;
        }
    }

    public float GetMaxHealth()
    {
        return playerAttributes.maxHP;
    }

    private void TakeDebugDamage()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            SetHealth(10);
        }
    }

    #endregion

    public void InitializeFlash()
    {
        // Iterate through all the children (deeply) of the player object
        foreach (Transform child in transform)
        {
            // Skip if the GameObject is inactive or specific named GameObjects
            if (!child.gameObject.activeInHierarchy || child.name == "Root" || child.name == "Cube" || child.name == "Chain_Joint")
            {
                continue;
            }

            // Try to get the Renderer component from this child
            renderer = child.GetComponentInChildren<Renderer>(false);  // `true` includes inactive objects
            if (renderer != null)
            {
                material = renderer.material;
                break;  // We found a Renderer, no need to continue searching
            }
        }

        if (renderer != null)
        {
            material.EnableKeyword("_EMISSION");
            originalColor = material.GetColor("_EmissionColor");
        }
        else
        {
            Debug.LogError("No Renderer found in children!");
        }
    }

    public void ActivateVisuals()
    {
        if (!isFlashing)
        {
            InitializeFlash();
            StartCoroutine(FlashCoroutine());
        }
    }

    private IEnumerator FlashCoroutine()
    {
        isFlashing = true;

        float elapsedTime = 0f;

        // Enable emission if it's off
        material.EnableKeyword("_EMISSION");

        while (elapsedTime < flashDuration)
        {
            // Calculate the current color by interpolating between the original and flash colors
            Color currentColor = Color.Lerp(originalColor, flashColor, elapsedTime / flashDuration);
            material.SetColor("_EmissionColor", currentColor);

            // Increment elapsed time
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Set to the flash color at the end of the flashDuration
        material.SetColor("_EmissionColor", flashColor);

        // Lerp back to the original color
        elapsedTime = 0f;
        while (elapsedTime < flashDuration)
        {
            Color currentColor = Color.Lerp(flashColor, originalColor, elapsedTime / flashDuration);
            material.SetColor("_EmissionColor", currentColor);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset to the original color and disable emission if it was off
        material.SetColor("_EmissionColor", originalColor);
        material.DisableKeyword("_EMISSION");

        isFlashing = false;
    }

    #region Events

    private void Chained2ViolenceGameManagerOnGameStateChanged(Chained2ViolenceGameManager.GameState state)
    {

        if (state == Chained2ViolenceGameManager.GameState.Paused || state == Chained2ViolenceGameManager.GameState.GameOver)
        {
            _bIsPlayerDisabled = true;
        }
        else if (state == Chained2ViolenceGameManager.GameState.Playing)
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
    private void PlayerCombatOnClassSwitched(GameObject player, PlayerCombat.PlayerClass newClass)
    {
        if(newClass == PlayerCombat.PlayerClass.Default)
        {
            _playerRigidbody.mass = playerAttributes.mass;
        }
        else if(newClass == PlayerCombat.PlayerClass.Tank)
        {
            _playerRigidbody.mass = playerAttributes.mass;
        }
        else if (newClass == PlayerCombat.PlayerClass.Warrior)
        {
            _playerRigidbody.mass = playerAttributes.mass;
        }
        else if (newClass == PlayerCombat.PlayerClass.Support)
        {           
            _playerRigidbody.mass = playerAttributes.mass;
        }
        else if (newClass == PlayerCombat.PlayerClass.Ranged)
        {
            _playerRigidbody.mass = playerAttributes.mass;
        }

        currentHealth = playerAttributes.maxHP;
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
        CapsuleCollider[] ragdollCapsules;

        if (GhostChain._bIsGhostChainActive)
        {
            _playerCollider.excludeLayers = GhostChain.ignoreCollisionLayers;
            _playerRigidbody.excludeLayers = GhostChain.ignoreCollisionLayers;

        }
        else
        {
            _playerCollider.excludeLayers = _defaultIgnoreCollisionLayer;
            _playerRigidbody.excludeLayers = _defaultIgnoreCollisionLayer;
        }

    }

    void FindAndStoreColliders(Transform parentTransform)
    {
        // Iterate through all the colliders in this GameObject and its children
        Collider[] colliders = parentTransform.GetComponentsInChildren<Collider>();

        // Iterate through the colliders and add them to the appropriate list
        foreach (var collider in colliders)
        {
            if(collider == colliders[0])
            {
                continue;
            }

            if (collider is CapsuleCollider)
            {
                capsuleColliders.Add(collider);
            }
            else if (collider is BoxCollider)
            {
                boxColliders.Add(collider);
            }
        }
    }

    // Method to disable the colliders by turning off the components
    void DisableColliders()
    {
        // Disable all CapsuleColliders
        foreach (var capsuleCollider in capsuleColliders)
        {
            capsuleCollider.enabled = false;
        }

        // Disable all BoxColliders
        foreach (var boxCollider in boxColliders)
        {
            boxCollider.enabled = false;
        }

        //Debug.Log("Disabled all CapsuleColliders and BoxColliders.");
    }

    void EnableColliders()
    {
        // Disable all CapsuleColliders
        foreach (var capsuleCollider in capsuleColliders)
        {
            capsuleCollider.enabled = true;
        }

        // Disable all BoxColliders
        foreach (var boxCollider in boxColliders)
        {
            boxCollider.enabled = true;
        }

        //Debug.Log("Disabled all CapsuleColliders and BoxColliders.");
    }


    #endregion
}



