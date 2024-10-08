using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// UpgradeManager used for handling player upgrades. The upgrades are shared.
/// Add gold script to upgrades, clean up code.
/// </summary>
public class UpgradeManager : MonoBehaviour
{
    private GameObject player1;
    private GameObject player2;

    private Player player1Manager;
    private Player player2Manager;

    private PlayerCombat player1Combat;
    private PlayerCombat player2Combat;

    private PlayerMovement player1Movement;
    private PlayerMovement player2Movement;

    // Initial Stats
    private float currentAttackDamage;
    private float currentMaxHealth;
    private float currentSpeed;

    private int currentAttackLevel = 0;
    private int currentHealthLevel = 0;
    private int currentSpeedLevel = 0;
    private const int MaxUpgradeLevel = 10;

    // Upgrade caps -- % increase from base stat.
    private const float MaxAttackMultiplier = 1.3f;
    private const float MaxHealthMultiplier = 1.5f;
    private const float MaxSpeedMultiplier = 1.3f;

    #region TextMeshPro
    public TMP_Text attackLevelText;
    public TMP_Text healthLevelText;
    public TMP_Text speedLevelText;
    #endregion

    void Start()
    {
        player1 = GameObject.FindGameObjectWithTag("Player1");
        player2 = GameObject.FindGameObjectWithTag("Player2");

        if (player1 == null || player2 == null)
        {
            Debug.LogError("Player 1 and 2 not found");
            return;
        }

        player1Manager = player1.GetComponent<Player>();
        player2Manager = player2.GetComponent<Player>();

        player1Combat = player1.GetComponent<PlayerCombat>();
        player2Combat = player2.GetComponent<PlayerCombat>();

        player1Movement = player1.GetComponent<PlayerMovement>();
        player2Movement = player2.GetComponent<PlayerMovement>();

        currentAttackDamage = player1Combat.attackDamage;
        currentMaxHealth = player1Manager._currentHealth;
        currentSpeed = player1Movement.originalWalkingSpeed;
    }

    // Upgrade Damage with Scriptable Object Data
    public void UpgradeDamage(DamageUpgradeSO damageUpgrade)
    {
        float initialAttackDamage = 10f;

        if (damageUpgrade != null && currentAttackLevel < MaxUpgradeLevel)
        {
            float newAttackDamage = currentAttackDamage + damageUpgrade.damageIncrease;
            float maxAllowedAttackDamage = initialAttackDamage * MaxAttackMultiplier;

            if (newAttackDamage <= maxAllowedAttackDamage)
            {
                currentAttackDamage = newAttackDamage;
                currentAttackLevel++;

                player1Combat.SetAttackDamage(currentAttackDamage);
                player2Combat.SetAttackDamage(currentAttackDamage);

                Debug.Log("Damage upgraded. New Attack Damage: " + currentAttackDamage);
                UpdateUpgradeLevelText();
            }
            else
            {
                Debug.LogWarning("Damage upgrade exceeds the maximum allowed limit.");
            }
        }
        else
        {
            Debug.LogWarning("Maximum attack upgrade level reached.");
        }
    }

    //Upgrade Health with Scriptable Object Data
    public void UpgradeHealth(HealthUpgradeSO healthUpgrade)
    {
        float initialMaxHealth = 100f; //Max hp so its not dependant on current health upgrade level

        if (healthUpgrade != null && currentHealthLevel < MaxUpgradeLevel)
        {
            float newMaxHealth = currentMaxHealth + healthUpgrade.healthIncrease;
            float maxAllowedHealth = initialMaxHealth * MaxHealthMultiplier;

            if (newMaxHealth <= maxAllowedHealth)
            {
                currentMaxHealth = newMaxHealth;
                currentHealthLevel++;

                // Update both players' health
                player1Manager.SetMaxHealth(currentMaxHealth);
                player2Manager.SetMaxHealth(currentMaxHealth);

                Debug.Log("Health upgraded for both players! New Max Health: " + currentMaxHealth);
                UpdateUpgradeLevelText();
            }
            else
            {
                Debug.LogWarning("Health upgrade exceeds the maximum allowed limit.");
            }
        }
        else
        {
            Debug.LogWarning("Maximum health upgrade level reached.");
        }
    }

    // Upgrades Speed with Scriptable Object Data
    public void UpgradeSpeed(SpeedUpgradeSO speedUpgrade)
    {
        if (speedUpgrade != null && currentSpeedLevel < MaxUpgradeLevel)
        {
            float newSpeed = currentSpeed + speedUpgrade.speedIncrease;
            float maxAllowedSpeed = player1Movement.originalWalkingSpeed * MaxSpeedMultiplier;

            if (newSpeed <= maxAllowedSpeed)
            {
                currentSpeed = newSpeed;
                currentSpeedLevel++;

                player1Movement.SetWalkingSpeed(currentSpeed);
                player2Movement.SetWalkingSpeed(currentSpeed);

                Debug.Log("Speed upgraded for both players! New Speed: " + currentSpeed);
                UpdateUpgradeLevelText();
            }
            else
            {
                Debug.LogWarning("Speed upgrade exceeds the maximum allowed limit.");
            }
        }
        else
        {
            Debug.LogWarning("Maximum speed upgrade level reached.");
        }
    }

    private void UpdateUpgradeLevelText()
    {
        if (attackLevelText != null)
        {
            attackLevelText.text = "Attack Level: " + currentAttackLevel.ToString();
        }

        if (healthLevelText != null)
        {
            healthLevelText.text = "Health Level: " + currentHealthLevel.ToString();
        }

        if (speedLevelText != null)
        {
            speedLevelText.text = "Speed Level: " + currentSpeedLevel.ToString();
        }
    }
}