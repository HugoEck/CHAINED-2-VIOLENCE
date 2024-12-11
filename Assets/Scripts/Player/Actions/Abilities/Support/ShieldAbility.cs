using UnityEngine;

public class ShieldAbility : MonoBehaviour, IAbility
{
    [Header("Activate Shield sound: ")]
    [SerializeField] private AudioClip supportAbilitySound;

    [Header("Absorb Damage sound: ")]
    [SerializeField] private AudioClip absorbDamageSound;

    [Header("Break Shield sound: ")]
    [SerializeField] private AudioClip breakShieldSound;

    private float maxShieldHealth;
    private float currentShieldHealth;

    [Header("Shield Visual")]
    [SerializeField] private GameObject shieldVisualPrefab;
    private GameObject activeShieldVisual;

    private bool isShieldActive = false;

    [Header("Cooldown Settings")]
    [SerializeField] public float cooldown = 5f;
    private float lastBreakTime = -Mathf.Infinity;

    [SerializeField] private GameObject otherPlayer;
    private GameObject activatingPlayer;

    private Player player;
    private PlayerHealth playerHealth;

    private void Start()
    {
        // Try to get the Player script
        player = GetComponent<Player>();
        playerHealth = GetComponent<PlayerHealth>();
        if (player == null)
        {
            Debug.LogError("Player script not found on the GameObject! Ensure it is attached.");
        }
    }

    private void OnEnable()
    {
        PlayerCombat.OnClassSwitchedUI += HandleClassSwitch;
    }

    private void OnDisable()
    {
        PlayerCombat.OnClassSwitchedUI -= HandleClassSwitch;
    }

    private void CalculateShieldHealth()
    {
        if (player != null)
        {
            maxShieldHealth = playerHealth.GetMaxHealth() * 0.2f; // Base shield health + 20% of player's base HP
        }
    }

    public void UseAbility()
    {
        if (Time.time >= lastBreakTime + cooldown)
        {
            CalculateShieldHealth();
            ActivateShield();
            ApplyShieldToOtherPlayer();
        }
        else
        {
            Debug.Log("Shield ability is on cooldown.");
        }
    }

    private void ActivateShield()
    {
        if (isShieldActive) return;

        currentShieldHealth = maxShieldHealth;
        isShieldActive = true;

        SFXManager.instance.PlaySFXClip(supportAbilitySound, transform, 1f);

        // Set the activating player
        activatingPlayer = gameObject;

        if (shieldVisualPrefab != null)
        {
            activeShieldVisual = Instantiate(shieldVisualPrefab, transform.position, Quaternion.identity, transform);
            activeShieldVisual.transform.localPosition = new Vector3(0, -3.0f, 0);
        }

        Debug.Log($"{gameObject.name} Shield Activated. Max Shield Health: {maxShieldHealth}");
    }

    private void ApplyShieldToOtherPlayer()
    {
        if (otherPlayer != null)
        {
            ShieldAbility otherPlayerShield = otherPlayer.GetComponent<ShieldAbility>();
            if (otherPlayerShield != null)
            {
                otherPlayerShield.ActivateShieldFromExternal(gameObject); // Activate shield with reference
                Debug.Log("Shield applied to other player: " + otherPlayer.name);
            }
        }
        else
        {
            Debug.LogWarning("Other player is not assigned!");
        }
    }

    public void ActivateShieldFromExternal(GameObject activator)
    {
        if (isShieldActive) return;

        CalculateShieldHealth();

        currentShieldHealth = maxShieldHealth;
        isShieldActive = true;
        activatingPlayer = activator;

        if (shieldVisualPrefab != null)
        {
            activeShieldVisual = Instantiate(shieldVisualPrefab, transform.position, Quaternion.identity, transform);
            activeShieldVisual.transform.localPosition = new Vector3(0, -3.0f, 0);
        }

        Debug.Log($"{gameObject.name} Shield Activated by {activator.name}. Max Shield Health: {maxShieldHealth}");
    }

    private void HandleClassSwitch(int playerId, PlayerCombat.PlayerClass newClass)
    {
        // Check if the activating player or other player is the one switching classes
        if (isShieldActive &&
            ((activatingPlayer != null && activatingPlayer.GetComponent<Player>()._playerId == playerId) ||
             (otherPlayer != null && otherPlayer.GetComponent<Player>()._playerId == playerId)))
        {
            BreakShield();

            if (otherPlayer != null)
            {
                ShieldAbility otherPlayerShield = otherPlayer.GetComponent<ShieldAbility>();
                otherPlayerShield?.BreakShield();
            }

            Debug.Log("Shield broken due to class switch.");
        }
    }

    public float AbsorbDamage(float damage)
    {
        if (!isShieldActive) return damage; // If shield is not active, pass damage through

        currentShieldHealth -= damage;
        Debug.Log(gameObject.name + " Shield absorbed " + damage + " damage. Remaining Shield Health: " + currentShieldHealth);

        SFXManager.instance.PlaySFXClip(absorbDamageSound, transform, 1f);

        if (currentShieldHealth <= 0)
        {
            float remainingDamage = Mathf.Abs(currentShieldHealth);
            BreakShield();
            return remainingDamage; // Return leftover damage that needs to be applied to the player
        }

        return 0; // Shield absorbed all damage
    }


    private void BreakShield()
    {
        if (!isShieldActive) return;

        SFXManager.instance.PlaySFXClip(breakShieldSound, transform, 1f);

        isShieldActive = false;
        currentShieldHealth = 0;
        lastBreakTime = Time.time;

        if (activeShieldVisual != null)
        {
            Destroy(activeShieldVisual);
        }

        Debug.Log(gameObject.name + " Shield has broken.");
    }

    public bool IsShieldActive()
    {
        return isShieldActive;
    }

    private void Update()
    {
        if (isShieldActive && activeShieldVisual != null)
        {
            Vector3 offset = new Vector3(0, -1.0f, 0);
            activeShieldVisual.transform.position = transform.position + offset;
        }
    }
}
