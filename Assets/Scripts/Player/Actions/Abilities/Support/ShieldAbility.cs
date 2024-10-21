using UnityEngine;

public class ShieldAbility : MonoBehaviour, IAbility
{
    [Header("Shield Settings")]
    [SerializeField] private float maxShieldHealth = 20.0f; // Max amount of damage the shield can absorb
    private float currentShieldHealth;

    private bool isShieldActive = false;

    public void UseAbility()
    {
        ActivateShield();
    }

    private void ActivateShield()
    {
        if (isShieldActive) return; // Avoid reactivating an already active shield

        currentShieldHealth = maxShieldHealth;
        isShieldActive = true;
        Debug.Log("Shield Activated. Max Shield Health: " + maxShieldHealth);
    }

    // Call this method when damage is taken while the shield is active
    public void AbsorbDamage(float damage)
    {
        if (!isShieldActive) return;

        currentShieldHealth -= damage;
        Debug.Log("Shield absorbed " + damage + " damage. Remaining Shield Health: " + currentShieldHealth);

        if (currentShieldHealth <= 0)
        {
            BreakShield();
        }
    }

    private void BreakShield()
    {
        isShieldActive = false;
        currentShieldHealth = 0;
        Debug.Log("Shield has broken.");
    }

    // Check if shield is active
    public bool IsShieldActive()
    {
        return isShieldActive;
    }
}
