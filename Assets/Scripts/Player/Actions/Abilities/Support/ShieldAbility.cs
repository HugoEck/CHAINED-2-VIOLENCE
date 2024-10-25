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

    // Reference to the other player
    [SerializeField] private GameObject otherPlayer; // Assign the other player's GameObject in the Inspector or via code

    public void UseAbility()
    {
        ActivateShield();
        ApplyShieldToOtherPlayer(); // Activate shield for the other player as well
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

    // Call this method when damage is taken while the shield is active
    public float AbsorbDamage(float damage)
    {
        if (!isShieldActive) return damage; // If shield is not active, pass damage through

        currentShieldHealth -= damage;
        Debug.Log(gameObject.name + " Shield absorbed " + damage + " damage. Remaining Shield Health: " + currentShieldHealth);

        // If shield health goes below or equals zero, break the shield and calculate remaining damage
        if (currentShieldHealth <= 0)
        {
            float remainingDamage = Mathf.Abs(currentShieldHealth); // Any excess damage over the shield's health
            BreakShield();
            return remainingDamage; // Return leftover damage that needs to be applied to the player
        }

        // If shield is still active and absorbed all damage, return 0 damage
        return 0;
    }

    private void BreakShield()
    {
        isShieldActive = false;
        currentShieldHealth = 0;

        // Destroy the shield visual when the shield breaks
        if (activeShieldVisual != null)
        {
            Destroy(activeShieldVisual);
        }

        Debug.Log(gameObject.name + " Shield has broken.");
    }

    // Check if shield is active
    public bool IsShieldActive()
    {
        return isShieldActive;
    }

    private void Update()
    {
        // Optional: If you want the shield to follow the player’s movement
        if (isShieldActive && activeShieldVisual != null)
        {
            activeShieldVisual.transform.position = transform.position;
        }
    }
}
