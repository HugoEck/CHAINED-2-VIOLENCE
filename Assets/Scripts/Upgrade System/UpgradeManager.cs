using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private GameObject player1;
    private GameObject player2;

    private DamageUpgradeComponent player1AttackUpgrade;
    private DamageUpgradeComponent player2AttackUpgrade;

    private SpeedUpgradeComponent player1SpeedUpgrade;
    private SpeedUpgradeComponent player2SpeedUpgrade;

    private HealthUpgradeComponent player1HealthUpgrade;
    private HealthUpgradeComponent player2HealthUpgrade;

    public GameObject player1Upgrades;
    public GameObject player2Upgrades;

    public TMP_Text player1AttackLevelText;
    public TMP_Text player2AttackLevelText;
    public TMP_Text player1HealthLevelText;
    public TMP_Text player2HealthLevelText;
    public TMP_Text player1SpeedLevelText;
    public TMP_Text player2SpeedLevelText;

    // Start is called before the first frame update

    void Start()
    {
        player1 = GameObject.FindGameObjectWithTag("Player1");
        player2 = GameObject.FindGameObjectWithTag("Player2");

        player1AttackUpgrade = player1.GetComponent<DamageUpgradeComponent>();
        player2AttackUpgrade = player2.GetComponent<DamageUpgradeComponent>();

        player1SpeedUpgrade = player1.GetComponent<SpeedUpgradeComponent>();
        player2SpeedUpgrade = player2.GetComponent<SpeedUpgradeComponent>();

        player1HealthUpgrade = player1.GetComponent<HealthUpgradeComponent>();
        player2HealthUpgrade = player2.GetComponent<HealthUpgradeComponent>();

        player1Upgrades.SetActive(false);
        player2Upgrades.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpgradePlayer1Health(HealthUpgradeSO healthUpgrade)
    {
        if (player1HealthUpgrade != null)
        {
            player1HealthUpgrade.Upgrade(healthUpgrade);
            UpdateUpgradeLevelText();
        }
        else
        {
            Debug.LogError("Player 1 HealthUpgradeComponent is missing.");
        }
    }

    public void UpgradePlayer2Health(HealthUpgradeSO healthUpgrade)
    {
        if (player2HealthUpgrade != null)
        {
            player2HealthUpgrade.Upgrade(healthUpgrade);
            UpdateUpgradeLevelText();
        }
        else
        {
            Debug.LogError("Player 2 HealthUpgradeComponent is missing.");
        }
    }

    // Upgrade Player 1's damage using ScriptableObject data
    public void UpgradePlayer1Damage(DamageUpgradeSO damageUpgrade)
    {
        player1AttackUpgrade.Upgrade(damageUpgrade);
        UpdateUpgradeLevelText();
    }

    // Upgrade Player 2's damage using ScriptableObject data
    public void UpgradePlayer2Damage(DamageUpgradeSO damageUpgrade)
    {
        player2AttackUpgrade.Upgrade(damageUpgrade);
        UpdateUpgradeLevelText();
    }

    public void UpgradePlayer1Speed(SpeedUpgradeSO speedUpgrade)
    {
        player1SpeedUpgrade.Upgrade(speedUpgrade);
        UpdateUpgradeLevelText();
    }

    public void UpgradePlayer2Speed(SpeedUpgradeSO speedUpgrade)
    {
        player2SpeedUpgrade.Upgrade(speedUpgrade);
        UpdateUpgradeLevelText();
    }

    public void ShowPlayer1Upgrades()
    {
        player1Upgrades.SetActive(true);
        player2Upgrades.SetActive(false);
    }

    public void ShowPlayer2Upgrades()
    {
        player2Upgrades.SetActive(true);
        player1Upgrades.SetActive(false);
    }

    private void UpdateUpgradeLevelText()
    {
        if (player1AttackUpgrade != null && player1AttackLevelText != null)
        {
            player1AttackLevelText.text = "Attack Level: " + player1AttackUpgrade.currentUpgradeLevel.ToString();
        }

        if (player2AttackUpgrade != null && player2AttackLevelText != null)
        {
            player2AttackLevelText.text = "Attack Level: " + player2AttackUpgrade.currentUpgradeLevel.ToString();
        }

        if (player1HealthUpgrade != null && player1HealthLevelText != null)
        {
            player1HealthLevelText.text = "Health Level: " + player1HealthUpgrade.currentUpgradeLevel.ToString();
        }

        if (player2HealthUpgrade != null && player2HealthLevelText != null)
        {
            player2HealthLevelText.text = "Health Level: " + player2HealthUpgrade.currentUpgradeLevel.ToString();
        }

        if (player1SpeedUpgrade != null && player1SpeedLevelText != null)
        {
            player1SpeedLevelText.text = "Speed Level: " + player1SpeedUpgrade.currentUpgradeLevel.ToString();
        }

        if (player2SpeedUpgrade != null && player2SpeedLevelText != null)
        {
            player2SpeedLevelText.text = "Speed Level: " + player2SpeedUpgrade.currentUpgradeLevel.ToString();
        }
    }
}