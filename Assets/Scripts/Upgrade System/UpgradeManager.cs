using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Obi;
using System.Runtime.CompilerServices;
/// <summary>
/// UpgradeManager used for handling player upgrades. The upgrades are shared.
/// Clean up code. REMOVED SCRIPTABLE OBJECTS..
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

    private HealthUpgrade healthUpgrade;
    private DamageUpgrade damageUpgrade;
    private SpeedUpgrade speedUpgrade;
    private ChainUpgrade chainUpgrade;

    private int currentAttackLevel = 0;
    private int currentHealthLevel = 0;
    private int currentSpeedLevel = 0;
    private int currentChainLevel = 0;
    private const int MaxUpgradeLevel = 10;
    [SerializeField] public int UpgradeCostIncrease = 20;

    // Upgrade caps -- % increase from base stat.
    [SerializeField] private float MaxAttackMultiplier = 1.3f;
    [SerializeField] private float MaxHealthMultiplier = 1.5f;
    [SerializeField] private float MaxSpeedMultiplier = 2.0f; //1.3f;

    // Upgrade Increment Values (instead of ScriptableObject)
    [SerializeField] private float attackDamageIncrease = 1f;  // Default damage increase per level
    [SerializeField] private int healthIncrease = 20;       // Default health increase per level
    [SerializeField] private float speedIncrease = 20f;

    //CHAIN UPGRADE
    private AdjustChainLength adjustChainLength;

    #region TextMeshPro
    public TMP_Text attackLevelText;
    public TMP_Text healthLevelText;
    public TMP_Text speedLevelText;
    public TMP_Text chainLevelText;
    public TMP_Text notEnoughGoldText;
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

        // Initialize the upgrade systems
        healthUpgrade = new HealthUpgrade(player1Manager, player2Manager, healthIncrease, MaxHealthMultiplier, MaxUpgradeLevel, UpgradeCostIncrease);
        damageUpgrade = new DamageUpgrade(player1Manager, player2Manager, player1Combat, player2Combat, attackDamageIncrease, MaxAttackMultiplier, MaxUpgradeLevel, UpgradeCostIncrease);
        speedUpgrade = new SpeedUpgrade(player1Movement, player2Movement, speedIncrease, 1000f, MaxSpeedMultiplier, MaxUpgradeLevel, UpgradeCostIncrease);
        
        // Initialize chain upgrade
        if (adjustChainLength != null)
        {
            chainUpgrade = new ChainUpgrade(adjustChainLength, 1, AdjustChainLength.AMOUNT_OF_UPGRADES, UpgradeCostIncrease);
        }
    }
    public void UpgradeHealth()
    {
        healthUpgrade.Upgrade();
        UpdateUpgradeLevelText();
    }

    public void UpgradeDamage()
    {
        damageUpgrade.Upgrade();
        UpdateUpgradeLevelText();
    }

    public void UpgradeSpeed()
    {
        speedUpgrade.Upgrade();
        UpdateUpgradeLevelText();
    }

    public void UpgradeChain()
    {
        chainUpgrade.Upgrade();
        UpdateUpgradeLevelText();
    }

    private int CalculateUpgradeCost(int currentLevel)
    {
        return (currentLevel + 1) * UpgradeCostIncrease;
    }

    private IEnumerator ShowNotEnoughGoldMessage()
    {
        if (notEnoughGoldText != null)
        {
            notEnoughGoldText.text = "Not enough gold!";
        }

        yield return new WaitForSeconds(2f); // Wait for 2 sec

        if (notEnoughGoldText != null)
        {
            notEnoughGoldText.text = "";
        }
    }


    private void UpdateUpgradeLevelText()
    {
        if (attackLevelText != null)
        {
            attackLevelText.text = "Attack Level: " + damageUpgrade.currentLevel.ToString();
        }

        if (healthLevelText != null)
        {
            healthLevelText.text = "Health Level: " + healthUpgrade.currentLevel.ToString();
        }

        if (speedLevelText != null)
        {
            speedLevelText.text = "Speed Level: " + speedUpgrade.currentLevel.ToString();
        }

        if (chainLevelText != null)
        {
            chainLevelText.text = "Chain Level: " + chainUpgrade.currentLevel.ToString();
        }
    }
}