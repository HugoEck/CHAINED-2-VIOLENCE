using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used in the UpgradeManager to apply upgrades.
/// </summary>
public class PlayerStats : MonoBehaviour
{
    // Start is called before the first frame update
    public int currentHealth;
    public int maxHealth;
    public int baseDamage;
    public float moveSpeed;

    void Start()
    {
        // Initialize with some default values
        maxHealth = 100;
        currentHealth = maxHealth;
        baseDamage = 10;
        moveSpeed = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Methods to apply upgrades (called by the UpgradeManager)
    public void ApplyHealthUpgrade(HealthUpgradeSO healthUpgrade)
    {
        maxHealth += healthUpgrade.healthIncrease;
        currentHealth = maxHealth;  // Optional: reset current health to max
        Debug.Log("Health upgraded! New Max Health: " + maxHealth);
    }

    public void ApplyDamageUpgrade(DamageUpgradeSO damageUpgrade)
    {
        baseDamage += damageUpgrade.damageIncrease;
        Debug.Log("Damage upgraded! New Base Damage: " + baseDamage);
    }

    public void ApplySpeedUpgrade(SpeedUpgradeSO speedUpgrade)
    {
        moveSpeed += speedUpgrade.speedIncrease;
        Debug.Log("Speed upgraded! New Move Speed: " + moveSpeed);
    }
}
