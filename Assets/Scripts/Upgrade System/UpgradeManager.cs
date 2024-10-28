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
    #region References
    private GameObject player1;
    private GameObject player2;

    private AdjustChainLength adjustChainLength;

    private Player player1Manager;
    private Player player2Manager;

    private PlayerCombat player1Combat;
    private PlayerCombat player2Combat;

    private PlayerMovement player1Movement;
    private PlayerMovement player2Movement;

    private HealthUpgrade healthUpgrade;
    private DamageUpgrade damageUpgrade;
    private SpeedUpgrade speedUpgrade;
    private ChainUpgrade chainUpgrade;
    #endregion

    [SerializeField] private int MaxUpgradeLevel = 10;
    [SerializeField] public int UpgradeCostIncrease = 20;

    // How much to increase the upgrade with.
    [SerializeField] private float attackDamageIncrease = 1f;
    [SerializeField] private int healthIncrease = 20;
    [SerializeField] private float speedIncrease = 20f;

    // Upgrade cap, % increase from the base value.
    [SerializeField] private float MaxAttackMultiplier = 1.3f;
    [SerializeField] private float MaxHealthMultiplier = 1.5f;
    [SerializeField] private float MaxSpeedMultiplier = 1.3f;
    #region TextMeshPro
    public TMP_Text attackLevelText;
    public TMP_Text healthLevelText;
    public TMP_Text speedLevelText;
    public TMP_Text chainLevelText;
    public TMP_Text notEnoughGoldText;
    public TMP_Text maxUpgradeReachedText;
    #endregion

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
        }
    }

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

        healthUpgrade = new HealthUpgrade(player1Manager, player2Manager, healthIncrease, MaxHealthMultiplier, MaxUpgradeLevel, UpgradeCostIncrease);
        damageUpgrade = new DamageUpgrade(player1Combat, player2Combat, attackDamageIncrease, MaxAttackMultiplier, MaxUpgradeLevel, UpgradeCostIncrease);
        speedUpgrade = new SpeedUpgrade(player1Movement, player2Movement, speedIncrease, MaxSpeedMultiplier, MaxUpgradeLevel, UpgradeCostIncrease);
       
        if (adjustChainLength != null)
        {
            chainUpgrade = new ChainUpgrade(adjustChainLength, 1, AdjustChainLength.AMOUNT_OF_UPGRADES, UpgradeCostIncrease);
        }
    }
    public void UpgradeHealth()
    {
        healthUpgrade.Upgrade();
        UpgradeGameData.Instance.PlayerHealth = player1Manager.currentHealth;
        UpdateUpgradeLevelText();
    }

    public void UpgradeDamage()
    {
        damageUpgrade.Upgrade();
        UpgradeGameData.Instance.PlayerAttack = player1Combat.attackDamage;
        UpdateUpgradeLevelText();
    }

    public void UpgradeSpeed()
    {
        speedUpgrade.Upgrade();
        UpgradeGameData.Instance.PlayerSpeed = player1Movement.GetWalkingSpeed();
        UpdateUpgradeLevelText();
    }

    public void UpgradeChain()
    {
        chainUpgrade.Upgrade();
        UpgradeGameData.Instance.ChainLength = adjustChainLength.ReturnCurrentChainLength();
        UpdateUpgradeLevelText();
    }

    #region Text UI Updates
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
    #endregion
}