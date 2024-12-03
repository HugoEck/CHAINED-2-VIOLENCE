using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float regenerationRate = 1f; // Health per second
    [SerializeField] private float respawnCooldown = 10f;

    public float currentHealth;
    private bool isDead;
    private bool respawnTimerSet;
    private float respawnTime;

    private Animator animator;
    private CapsuleCollider capsuleCollider;

    private Material material;
    private Color flashColor = Color.white;
    private Color originalColor;
    private float flashDuration = 0.1f;
    private bool isFlashing;
    private Renderer renderer;

    public event Action<float> OnHealthChanged; // For UI or effects
    public event Action OnPlayerDeath;
    public event Action OnPlayerRespawn;

    private ShieldAbility _shieldAbility;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>(false);
        capsuleCollider = GetComponent<CapsuleCollider>();
        _shieldAbility = GetComponent<ShieldAbility>();
        InitializeVisuals();

        currentHealth = maxHealth;
        isDead = false;
    }

    private void Update()
    {
        if (isDead)
        {
            HandleRespawn();
        }
        else
        {
            RegenerateHealth();
        }
    }

    /// <summary>
    /// Applies damage to the player and triggers visuals.
    /// </summary>
    public void SetHealth(float damage)
    {
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
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health doesn't go below 0

        Debug.Log($"{gameObject.name} took {damage} damage, current health: {currentHealth}");

        OnHealthChanged?.Invoke(currentHealth); // Trigger health update event

        if (currentHealth <= 0 && !isDead)
        {
            HandleDeath();
        }
        else
        {
            ActivateVisuals(); // Flash damage visuals
        }
    }

    /// <summary>
    /// Regenerates health over time if below max health.
    /// </summary>
    private void RegenerateHealth()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += regenerationRate * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            OnHealthChanged?.Invoke(currentHealth);
        }
    }

    /// <summary>
    /// Handles player death by toggling ragdoll and marking as dead.
    /// </summary>
    private void HandleDeath()
    {
        isDead = true;
        ToggleRagdoll(true);
        OnPlayerDeath?.Invoke();

        Debug.Log($"{gameObject.name} has died.");
    }

    /// <summary>
    /// Handles the respawn process after a cooldown.
    /// </summary>
    private void HandleRespawn()
    {
        if (!respawnTimerSet)
        {
            respawnTime = respawnCooldown;
            respawnTimerSet = true;
        }

        respawnTime -= Time.deltaTime;

        if (respawnTime <= 0)
        {
            Respawn();
        }
    }

    /// <summary>
    /// Respawns the player by resetting health and toggling ragdoll off.
    /// </summary>
    private void Respawn()
    {
        isDead = false;
        currentHealth = maxHealth;
        respawnTimerSet = false;

        ToggleRagdoll(false);
        OnPlayerRespawn?.Invoke();

        Debug.Log($"{gameObject.name} has respawned.");
    }

    /// <summary>
    /// Toggles ragdoll by enabling/disabling animator and collider.
    /// </summary>
    private void ToggleRagdoll(bool enable)
    {
        if (animator != null) animator.enabled = !enable;
        if (capsuleCollider != null) capsuleCollider.enabled = !enable;

        Debug.Log($"{gameObject.name} ragdoll state: {(enable ? "Enabled" : "Disabled")}");
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
            //InitializeVisuals();
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

    public float GetCurrentHealth() => currentHealth;

    public float GetMaxHealth() => maxHealth;
}
