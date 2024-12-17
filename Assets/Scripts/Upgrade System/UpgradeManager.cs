using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Obi;
using System.Runtime.CompilerServices;

/// <summary>
/// Upgrade manager, all variables are shared between the players. Manages the upgrades.
/// </summary>
public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }
    private AdjustChainLength adjustChainLength;


    private PlayerAttributes player1Attributes;
    private PlayerAttributes player2Attributes;

    [SerializeField] private int baseUpgradeCost = 20;
    [SerializeField] private int maxUpgradeLevel = 10;
    [SerializeField] public int UpgradeCostIncrease = 20;

    [Header("Upgrade Increments")]
    [SerializeField] private float attackDamageIncrease = 1f;
    [SerializeField] private float maxHealthIncrease = 20f;
    [SerializeField] private float movementSpeedIncrease = 10f;

    [Header("Upgrade Levels")]
    private int healthLevel = 0;
    private int attackLevel = 0;
    private int speedLevel = 0;
    private int chainLevel = 0;

    #region TextMeshPro
    public TMP_Text attackLevelText;
    public TMP_Text healthLevelText;
    public TMP_Text speedLevelText;
    public TMP_Text chainLevelText;
    public TMP_Text notEnoughGoldText;
    public TMP_Text maxUpgradeReachedText;
    public TMP_Text attackUpgradeCostText;
    public TMP_Text healthUpgradeCostText;
    public TMP_Text speedUpgradeCostText;
    public TMP_Text chainUpgradeCostText;
    #endregion

    private GameObject player1;
    private GameObject player2;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        player1 = GameObject.FindGameObjectWithTag("Player1");
        player2 = GameObject.FindGameObjectWithTag("Player2");

        if (player1 != null) player1Attributes = player1.GetComponent<PlayerAttributes>();
        if (player2 != null) player2Attributes = player2.GetComponent<PlayerAttributes>();

        if (player1Attributes == null || player2Attributes == null)
        {
            Debug.LogError("JACK START script not found on players!");
        }

        GameObject chainObject = GameObject.Find("Obi_Chain");
        if (chainObject != null)
        {
            adjustChainLength = chainObject.GetComponent<AdjustChainLength>();
        }
    }

    void Update()
    {
        if (player1 == null || player2 == null)
        {
            player1 = GameObject.FindGameObjectWithTag("Player1");
            player2 = GameObject.FindGameObjectWithTag("Player2");
        }

        if (player1 != null) player1Attributes = player1.GetComponent<PlayerAttributes>();
        if (player2 != null) player2Attributes = player2.GetComponent<PlayerAttributes>();

        if (player1Attributes == null || player2Attributes == null)
        {
            Debug.LogError("JACK script not found on players!");
        }
    }

    public void UpgradeMaxHealth()
    {
        int cost = CalculateUpgradeCost(healthLevel);

        if (CanAfford(cost) && healthLevel < maxUpgradeLevel)
        {
            // Directly apply the upgrade to player attributes
            player1Attributes.UpgradeMaxHealth(maxHealthIncrease);
            player2Attributes.UpgradeMaxHealth(maxHealthIncrease);

            // Update StatsTransfer to reflect the cumulative upgrades
            StatsTransfer.Player1UpgradedMaxHP = player1Attributes._upgradeMaxHP;
            StatsTransfer.Player2UpgradedMaxHP = player2Attributes._upgradeMaxHP;

            // Save the upgraded values to StatsTransfer
            StatsTransfer.Instance.SaveStatsPlayer1Upgrades(player1Attributes);
            StatsTransfer.Instance.SaveStatsPlayer2Upgrades(player2Attributes);

            SpendGold(cost);
            healthLevel++;
            UpdateUI();

            Debug.Log($"JACK Player 1 New HP: {player1Attributes.maxHP}, Upgrade Level: {healthLevel}");
            Debug.Log($"JACK Player 2 New HP: {player2Attributes.maxHP}, Upgrade Level: {healthLevel}");
        }
        else
        {
            ShowUpgradeError(healthLevel);
        }
    }

    public void UpgradeAttackDamage()
    {
        int cost = CalculateUpgradeCost(attackLevel);

        if (CanAfford(cost) && attackLevel < maxUpgradeLevel)
        {
            player1Attributes.UpgradeAttackDamage(attackDamageIncrease);
            player2Attributes.UpgradeAttackDamage(attackDamageIncrease);

            StatsTransfer.Player1UpgradedAttackDamage = player1Attributes._upgradeAttackDamage;
            StatsTransfer.Player2UpgradedAttackDamage = player2Attributes._upgradeAttackDamage;

            StatsTransfer.Instance.SaveStatsPlayer1Upgrades(player1Attributes);
            StatsTransfer.Instance.SaveStatsPlayer2Upgrades(player2Attributes);

            SpendGold(cost);
            attackLevel++;
            UpdateUI();

            // Debug Logs
            Debug.Log($"JACK Player 1 New Damage: {player1Attributes.attackDamage}, Upgrade Level: {attackLevel}");
            Debug.Log($"JACK Player 2 New Damage: {player2Attributes.attackDamage}, Upgrade Level: {attackLevel}");
        }
        else
        {
            ShowUpgradeError(attackLevel);
        }
    }

    public void UpgradeMovementSpeed()
    {
        int cost = CalculateUpgradeCost(speedLevel);

        if (CanAfford(cost) && speedLevel < maxUpgradeLevel)
        {
            player1Attributes.UpgradeMovementSpeed(movementSpeedIncrease);
            player2Attributes.UpgradeMovementSpeed(movementSpeedIncrease);

            StatsTransfer.Player1UpgradedSpeed = player1Attributes._upgradeMovementSpeed;
            StatsTransfer.Player2UpgradedSpeed = player2Attributes._upgradeMovementSpeed;

            StatsTransfer.Instance.SaveStatsPlayer1Upgrades(player1Attributes);
            StatsTransfer.Instance.SaveStatsPlayer2Upgrades(player2Attributes);

            SpendGold(cost);
            speedLevel++;
            UpdateUI();

            Debug.Log($"JACK Player 1 New Speed: {player1Attributes.movementSpeed}, Upgrade Level: {speedLevel}");
            Debug.Log($"JACK Player 2 New Speed: {player2Attributes.movementSpeed}, Upgrade Level: {speedLevel}");
        }
        else
        {
            ShowUpgradeError(speedLevel);
        }
    }

    public void UpgradeChain()
    {
        int cost = CalculateUpgradeCost(chainLevel);
        int lengthIncreasePerUpgrade = 1;
        if (CanAfford(cost) && chainLevel < AdjustChainLength.AMOUNT_OF_UPGRADES)
        {
            adjustChainLength.IncreaseRopeLength(lengthIncreasePerUpgrade);
            chainLevel++;

            SpendGold(cost);
            StatsTransfer.CurrentChainLength = adjustChainLength.ReturnCurrentChainLength();
            UpdateUI();
        }
        else
        {
            ShowUpgradeError(chainLevel);
        }
    }

    private bool CanAfford(int cost)
    {
        return GoldDropManager.Instance.GetGoldAmount() >= cost;
    }

    private void SpendGold(int amount)
    {
        GoldDropManager.Instance.SpendGold(amount);
    }

    private int CalculateUpgradeCost(int currentLevel)
    {
        return Mathf.CeilToInt(baseUpgradeCost * Mathf.Pow(1.5f, currentLevel));
    }

    private bool CanUpgrade()
    {
        // fix later if needed.
        return true;
    }

    private void ShowUpgradeError(int level)
    {
        if (level >= maxUpgradeLevel)
        {
            StartCoroutine(ShowMaxUpgradeReachedMessage());
        }
        else
        {
            StartCoroutine(ShowNotEnoughGoldMessage());
        }
    }

    #region Text UI Updates
    private void UpdateUI()
    {
        // Update Level Texts
        if (healthLevelText != null) healthLevelText.text = $"Health Level: {healthLevel}";

        if (attackLevelText != null) attackLevelText.text = $"Attack Level: {attackLevel}";

        if (speedLevelText != null) speedLevelText.text = $"Speed Level: {speedLevel}";

        if (chainLevelText != null) chainLevelText.text = $"Chain Level: {chainLevel}";

        // Update Cost Texts
        if (healthUpgradeCostText != null)
        {
            healthUpgradeCostText.text = healthLevel >= maxUpgradeLevel
                ? "Cost: Maxed"
                : $"Cost: {CalculateUpgradeCost(healthLevel)}";
        }

        if (attackUpgradeCostText != null)
        {
            attackUpgradeCostText.text = attackLevel >= maxUpgradeLevel
                ? "Cost: Maxed"
                : $"Cost: {CalculateUpgradeCost(attackLevel)}";
        }

        if (speedUpgradeCostText != null)
        {
            speedUpgradeCostText.text = speedLevel >= maxUpgradeLevel
                ? "Cost: Maxed"
                : $"Cost: {CalculateUpgradeCost(speedLevel)}";
        }

        if (chainUpgradeCostText != null)
        {
            chainUpgradeCostText.text = chainLevel >= AdjustChainLength.AMOUNT_OF_UPGRADES //fix later
                ? "Cost: Maxed"
                : $"Cost: {CalculateUpgradeCost(chainLevel)}";
        }
    }

    public IEnumerator ShowNotEnoughGoldMessage()
    {
        if (notEnoughGoldText != null)
        {
            notEnoughGoldText.text = "Not enough gold!";
        }

        yield return new WaitForSeconds(2f);

        if (notEnoughGoldText != null)
        {
            notEnoughGoldText.text = "";
        }
    }

    public IEnumerator ShowMaxUpgradeReachedMessage()
    {
        if (maxUpgradeReachedText != null)
        {
            maxUpgradeReachedText.text = "Max upgrade reached!";
        }

        yield return new WaitForSeconds(2f);

        if (maxUpgradeReachedText != null)
        {
            maxUpgradeReachedText.text = "";
        }
    }

    private void UpdateLevelUI()
    {
        // Update level texts
        healthLevelText.text = $"Health: Level {healthLevel}";
        attackLevelText.text = $"Attack: Level {attackLevel}";
        speedLevelText.text = $"Speed: Level {speedLevel}";

        // Update gold text
        //goldText.text = $"Gold: {GoldDropManager.Instance.GetGoldAmount()}";
    }

    #endregion
}