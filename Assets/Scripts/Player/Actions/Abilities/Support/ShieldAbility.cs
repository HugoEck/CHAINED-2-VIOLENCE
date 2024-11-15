using UnityEngine;

public class ShieldAbility : MonoBehaviour, IAbility
{
    [Header("Shield Settings")]
    [SerializeField] private float maxShieldHealth = 20.0f; // Max amount of damage the shield can absorb
    private float currentShieldHealth;

    [Header("Shield Visual")]
    [SerializeField] private GameObject shieldVisualPrefab;  // Assign your shield visual prefab here
    private GameObject activeShieldVisual; // To keep track of the instantiated shield visual

    private bool isShieldActive = false;

    [Header("Cooldown Settings")]
    [SerializeField] public float cooldown = 5f; // Cooldown duration in seconds
    private float lastBreakTime = -Mathf.Infinity; // Time when the shield last broke

    [SerializeField] private GameObject otherPlayer; // Reference to the other player

    public void UseAbility()
    {
        if (Time.time >= lastBreakTime + cooldown) // Check if cooldown has elapsed
        {
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
        if (isShieldActive) return; // Avoid reactivating an already active shield

        currentShieldHealth = maxShieldHealth;
        isShieldActive = true;

        // Instantiate the shield visual
        if (shieldVisualPrefab != null)
        {
            activeShieldVisual = Instantiate(shieldVisualPrefab, transform.position, Quaternion.identity, transform);
            activeShieldVisual.transform.localPosition = new Vector3(0, -3.0f, 0); // Adjust Y value as needed
        }

        Debug.Log(gameObject.name + " Shield Activated. Max Shield Health: " + maxShieldHealth);
    }

    private void ApplyShieldToOtherPlayer()
    {
        if (otherPlayer != null)
        {
            ShieldAbility otherPlayerShield = otherPlayer.GetComponent<ShieldAbility>();
            if (otherPlayerShield != null)
            {
                otherPlayerShield.ActivateShield(); // Activate shield for the other player
                Debug.Log("Shield applied to other player: " + otherPlayer.name);
            }
        }
        else
        {
            Debug.LogWarning("Other player is not assigned!");
        }
    }

    public float AbsorbDamage(float damage)
    {
        if (!isShieldActive) return damage; // If shield is not active, pass damage through

        currentShieldHealth -= damage;
        Debug.Log(gameObject.name + " Shield absorbed " + damage + " damage. Remaining Shield Health: " + currentShieldHealth);

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
        isShieldActive = false;
        currentShieldHealth = 0;
        lastBreakTime = Time.time; // Start the cooldown timer when the shield breaks

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
            Vector3 offset = new Vector3(0, -1.0f, 0); // Adjust Y value as needed
            activeShieldVisual.transform.position = transform.position + offset;
        }
    }
}
