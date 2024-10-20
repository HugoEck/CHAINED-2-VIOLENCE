using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Obi;
using System.Runtime.CompilerServices;
/// <summary>
/// UpgradeManager used for handling player upgrades. The upgrades are shared.
/// Clean up code.
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
    private int currentChainLevel = 0;
    private const int MaxUpgradeLevel = 10;
    [SerializeField] public int UpgradeCostIncrease = 20;

    // Upgrade caps -- % increase from base stat.
    [SerializeField] private float MaxAttackMultiplier = 1.3f;
    [SerializeField] private float MaxHealthMultiplier = 1.5f;
    [SerializeField] private float MaxSpeedMultiplier = 1.3f;

    //CHAIN UPGRADE
    private AdjustChainLength adjustChainLength;

    #region TextMeshPro
    public TMP_Text attackLevelText;
    public TMP_Text healthLevelText;
    public TMP_Text speedLevelText;
    public TMP_Text chainLevelText;
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

        GameObject chainObject = GameObject.Find("Obi_Chain");
        if (chainObject != null)
        {
            adjustChainLength = chainObject.GetComponent<AdjustChainLength>();
        }

        currentAttackDamage = player1Combat.attackDamage;
        currentMaxHealth = player1Manager.currentHealth;
        currentSpeed = player1Movement.originalWalkingSpeed;
    }

    // Upgrade Damage with Scriptable Object Data
    public void UpgradeDamage(DamageUpgradeSO damageUpgrade)
    {
        float initialAttackDamage = 10f; //match with damage from script or get reference later on

        int upgradeCost = CalculateUpgradeCost(currentAttackLevel);

        if (GoldDropManager.Instance.GetGoldAmount() >= upgradeCost && damageUpgrade != null && currentAttackLevel < MaxUpgradeLevel)
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

                GoldDropManager.Instance.SpendGold(upgradeCost);
                UpdateUpgradeLevelText();
            }
        }
    }

    //Upgrade Health with Scriptable Object Data
    public void UpgradeHealth(HealthUpgradeSO healthUpgrade)
    {
        float initialMaxHealth = 100f;
        int upgradeCost = CalculateUpgradeCost(currentHealthLevel);
        if (GoldDropManager.Instance.GetGoldAmount() >= upgradeCost && healthUpgrade != null && currentHealthLevel < MaxUpgradeLevel)
        {
            float newMaxHealth = currentMaxHealth + healthUpgrade.healthIncrease;
            float maxAllowedHealth = initialMaxHealth * MaxHealthMultiplier;

            if (newMaxHealth <= maxAllowedHealth)
            {
                currentMaxHealth = newMaxHealth;
                currentHealthLevel++;

                player1Manager.SetMaxHealth(currentMaxHealth);
                player2Manager.SetMaxHealth(currentMaxHealth);

                Debug.Log("Health upgraded for both players! New Max Health: " + currentMaxHealth);

                GoldDropManager.Instance.SpendGold(upgradeCost);
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
        int upgradeCost = CalculateUpgradeCost(currentSpeedLevel);

        if (GoldDropManager.Instance.GetGoldAmount() >= upgradeCost && speedUpgrade != null && currentSpeedLevel < MaxUpgradeLevel)
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
                GoldDropManager.Instance.SpendGold(upgradeCost);
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

    // Chain Upgrades
    public void UpgradeChain(int amountsOfUnits)
    {
        int upgradeCost = CalculateUpgradeCost(currentChainLevel);

        if (adjustChainLength == null)
        {
            Debug.LogError("Chain reference missing");
            return;
        }

        if (GoldDropManager.Instance.GetGoldAmount() >= upgradeCost && currentChainLevel < AdjustChainLength.AMOUNT_OF_UPGRADES)
        {
            adjustChainLength.IncreaseRopeLength(amountsOfUnits);
            currentChainLevel++;
            Debug.Log("Chain upgraded - New Chain Length: " + adjustChainLength.ReturnCurrentChainLength());

            GoldDropManager.Instance.SpendGold(upgradeCost);
            UpdateUpgradeLevelText();
        }
        else
        {
            Debug.LogWarning("Maximum chain upgrade level reached");
        }
    }
    private int CalculateUpgradeCost(int currentLevel)
    {
        return (currentLevel + 1) * UpgradeCostIncrease;
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

        if (chainLevelText != null)
        {
            chainLevelText.text = "Chain Level: " + currentChainLevel.ToString(); // Update the chain upgrade level
        }
    }
}