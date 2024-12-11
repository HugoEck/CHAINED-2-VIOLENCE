using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private PlayerAttributes _playerAttributes;

    private float _maxHealth;
    [SerializeField] private float regenerationRate = 1f; // Health per second
   // [SerializeField] private float respawnCooldown = 10f;
    private float regenerationCooldown = 10f; // Time to wait before regenerating health
    private float timeSinceLastDamageOrAction = 0f; // Tracks time since the last damage or action

    public float currentHealth;
    private bool isDead;
    //private bool respawnTimerSet;
    //private float respawnTime;

    //Unknown reason but each floatValue = 6sec
    private float _respawnCooldown = 2f;
    private float _respawnTime;

    // Arrays to store the found colliders
    private List<Collider> capsuleColliders = new List<Collider>();
    private List<Collider> boxColliders = new List<Collider>();

    private Animator animator;
    private CapsuleCollider capsuleCollider;

    private Material material;
    private Color flashColor = Color.white;
    private Color originalColor;
    private float flashDuration = 0.1f;
    private bool isFlashing;
    private Renderer renderer;

    public bool _bIsPlayerDisabled = false;
    public int _playerId { get; private set; }

    public event Action<float> OnHealthChanged; // For UI or effects
    public event Action OnPlayerDeath;
    public event Action OnPlayerRespawn;

    private ShieldAbility _shieldAbility;
    public GameObject player1Obj;
    public GameObject player2Obj;

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
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>(false);
        capsuleCollider = GetComponent<CapsuleCollider>();
        _shieldAbility = GetComponent<ShieldAbility>();
        _maxHealth = _playerAttributes.maxHP;
        currentHealth = _maxHealth;
        isDead = false;
    }

    private void Update()
    {
        if (!isDead)
        {
            // Increment timer if no actions have been performed
            timeSinceLastDamageOrAction += Time.deltaTime;

            // Start regenerating health only if the cooldown has passed
            if (timeSinceLastDamageOrAction >= regenerationCooldown)
            {
                RegenerateHealth();
            }
        }    
    }

    public void SetMaxHealth(float newMaxHealth)
    {
        _maxHealth = newMaxHealth;
        _playerAttributes.maxHP = newMaxHealth;

        // Heal to full if upgrading max health
        currentHealth = _maxHealth;
        OnHealthChanged?.Invoke(currentHealth);
    }

    public float GetMaxHealth()
    {
        return _playerAttributes.maxHP;
    }

    /// <summary>
    /// Applies damage to the player and triggers visuals.
    /// </summary>
    public void SetHealth(float damage)
    {
        // Reset the cooldown timer when the player takes damage
        timeSinceLastDamageOrAction = 0f;

        //if (GhostChain._bIsGhostChainActive) return; // Ignore damage if GhostChain is active

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
        currentHealth = Mathf.Clamp(currentHealth, 0, _maxHealth); // Ensure health doesn't go below 0

        Debug.Log($"{gameObject.name} took {damage} damage, current health: {currentHealth}");

        OnHealthChanged?.Invoke(currentHealth); // Trigger health update event

        if (currentHealth <= 0 && !isDead)
        {
            HandleKnockout();
        }
        else
        {
            ActivateVisuals(); // Flash damage visuals
        }
    }

    public void OnPlayerAttack()
    {
        timeSinceLastDamageOrAction = 0f; // Reset the timer when the player attacks
    }

    private void RegenerateHealth()
    {
        if (currentHealth < _maxHealth)
        {
            currentHealth += regenerationRate * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0, _maxHealth);
            OnHealthChanged?.Invoke(currentHealth);
        }
    }

    public void ToggleRagdoll(bool turnOn, GameObject player)
    {
        if (turnOn)
        {
            player.GetComponentInChildren<Animator>(false).enabled = false;
            player.GetComponent<CapsuleCollider>().enabled = false;
        }
        else if (!turnOn)
        {
            player.GetComponentInChildren<Animator>(false).enabled = true;
            player.GetComponent<CapsuleCollider>().enabled = true;
        }
    }

    /// <summary>
    /// Handles player death by toggling ragdoll and marking as dead.
    /// </summary>
    private void HandleKnockout()
    {
        if (Chained2ViolenceGameManager.Instance.currentSceneState != Chained2ViolenceGameManager.SceneState.ArenaScene) return;

        if (Chained2ViolenceGameManager.Instance.BIsPlayer2Assigned)
        {
            if (currentHealth <= 0 && _playerId == 1 && !_bIsPlayerDisabled)
            {
                ToggleRagdoll(true, player1Obj);
                _bIsPlayerDisabled = true;
                Player.playersDefeated++;

            }
            else if (currentHealth <= 0 && _playerId == 2 && !_bIsPlayerDisabled)
            {
                ToggleRagdoll(true, player2Obj);
                _bIsPlayerDisabled = true;
                Player.playersDefeated++;

            }

            if (_bIsPlayerDisabled)
            {
                EnableColliders();
                Respawn();
            }

            if (Player.playersDefeated == 2)
            {
                Player.playersDefeated = 0;
                Chained2ViolenceGameManager.Instance.UpdateGamestate(Chained2ViolenceGameManager.GameState.GameOver);
            }
        }
        else
        {

            if (currentHealth <= 0 && _playerId == 1 && !_bIsPlayerDisabled)
            {
                ToggleRagdoll(true, player1Obj);
                _bIsPlayerDisabled = true;
                Player.playersDefeated++;

            }

            if (_bIsPlayerDisabled)
            {
                EnableColliders();
                Respawn();
            }

            if (Player.playersDefeated == 1)
            {
                Player.playersDefeated = 0;
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
            Player.playersDefeated--;
            currentHealth = _playerAttributes.maxHP;
            _bIsPlayerDisabled = false;
            respawnTimerSet = false;

            if (_playerId == 1)
            {
                ToggleRagdoll(false, player1Obj);

                DisableColliders();
            }
            else if (_playerId == 2)
            {
                ToggleRagdoll(false, player2Obj);

                DisableColliders();
            }

        }
    }

    /// <summary>
    /// Initializes flashing visuals by finding the appropriate renderer.
    /// </summary>
    private void InitializeVisuals()
    {
        foreach (Transform child in transform)
        {
            if (!child.gameObject.activeInHierarchy || child.name == "Root" || child.name == "Cube" || child.name == "Chain_Joint")
                continue;

            renderer = child.GetComponentInChildren<Renderer>(false);
            if (renderer != null)
            {
                material = renderer.material;
                break;
            }
        }

        if (renderer != null)
        {
            material.EnableKeyword("_EMISSION");
            originalColor = material.GetColor("_EmissionColor");
        }
        else
        {
            Debug.LogError("No Renderer found for visuals!");
        }
    }

    /// <summary>
    /// Activates flash visuals to indicate damage.
    /// </summary>
    private void ActivateVisuals()
    {
        if (!isFlashing)
        {
            InitializeVisuals();
            StartCoroutine(FlashCoroutine());
        }
    }

    private IEnumerator FlashCoroutine()
    {
        isFlashing = true;

        float elapsedTime = 0f;
        material.EnableKeyword("_EMISSION");

        while (elapsedTime < flashDuration)
        {
            Color currentColor = Color.Lerp(originalColor, flashColor, elapsedTime / flashDuration);
            material.SetColor("_EmissionColor", currentColor);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        material.SetColor("_EmissionColor", flashColor);

        elapsedTime = 0f;
        while (elapsedTime < flashDuration)
        {
            Color currentColor = Color.Lerp(flashColor, originalColor, elapsedTime / flashDuration);
            material.SetColor("_EmissionColor", currentColor);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        material.SetColor("_EmissionColor", originalColor);
        material.DisableKeyword("_EMISSION");

        isFlashing = false;
    }

    // Method to disable the colliders by turning off the components
    public void DisableColliders()
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

        Debug.Log("Disabled all CapsuleColliders and BoxColliders.");
    }

    public void EnableColliders()
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

        Debug.Log("Disabled all CapsuleColliders and BoxColliders.");
    }

    //public float GetCurrentHealth() => currentHealth;
    //
    //public float GetMaxHealth() => maxHealth;
}
