using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private GameObject player1;
    private GameObject player2;

    private SpeedUpgradeComponent player1SpeedUpgrade;
    private SpeedUpgradeComponent player2SpeedUpgrade;

    private HealthUpgradeComponent player1HealthUpgrade;
    private HealthUpgradeComponent player2HealthUpgrade;

    public GameObject player1Upgrades;
    public GameObject player2Upgrades;

    // Start is called before the first frame update
    void Start()
    {
        player1 = GameObject.FindGameObjectWithTag("Player1");
        player2 = GameObject.FindGameObjectWithTag("Player2");
        
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
        }
        else
        {
            Debug.LogError("Player 2 HealthUpgradeComponent is missing.");
        }
    }

    // Upgrade Player 1's damage using ScriptableObject data
    public void UpgradePlayer1Damage(DamageUpgradeSO damageUpgrade)
    {
        //player1Stats.ApplyDamageUpgrade(damageUpgrade);
    }

    // Upgrade Player 2's damage using ScriptableObject data
    public void UpgradePlayer2Damage(DamageUpgradeSO damageUpgrade)
    {
        //player2Stats.ApplyDamageUpgrade(damageUpgrade);
    }

    public void UpgradePlayer1Speed(SpeedUpgradeSO speedUpgrade)
    {
        player1SpeedUpgrade.Upgrade(speedUpgrade);
    }

    public void UpgradePlayer2Speed(SpeedUpgradeSO speedUpgrade)
    {
        player2SpeedUpgrade.Upgrade(speedUpgrade);
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
}