using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private GameObject player1;
    private GameObject player2;

    private PlayerStats player1Stats;
    private PlayerStats player2Stats;

    public GameObject player1Upgrades;
    public GameObject player2Upgrades;

    // Start is called before the first frame update
    void Start()
    {
        player1 = GameObject.FindGameObjectWithTag("Player1");
        player2 = GameObject.FindGameObjectWithTag("Player2");

        player1Stats = player1.GetComponent<PlayerStats>();
        player2Stats = player2.GetComponent<PlayerStats>();

        player1Upgrades.SetActive(false);
        player2Upgrades.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Upgrade Player 1's health using ScriptableObject data
    public void UpgradePlayer1Health(HealthUpgradeSO healthUpgrade)
    {
        player1Stats.ApplyHealthUpgrade(healthUpgrade);
    }

    // Upgrade Player 1's damage using ScriptableObject data
    public void UpgradePlayer1Damage(DamageUpgradeSO damageUpgrade)
    {
        player1Stats.ApplyDamageUpgrade(damageUpgrade);
    }

    // Upgrade Player 2's health using ScriptableObject data
    public void UpgradePlayer2Health(HealthUpgradeSO healthUpgrade)
    {
        player2Stats.ApplyHealthUpgrade(healthUpgrade);
    }

    // Upgrade Player 2's damage using ScriptableObject data
    public void UpgradePlayer2Damage(DamageUpgradeSO damageUpgrade)
    {
        player2Stats.ApplyDamageUpgrade(damageUpgrade);
    }

    public void UpgradePlayer1Speed(SpeedUpgradeSO speedUpgrade)
    {
        player1Stats.ApplySpeedUpgrade(speedUpgrade);
    }

    public void UpgradePlayer2Speed(SpeedUpgradeSO speedUpgrade)
    {
        player2Stats.ApplySpeedUpgrade(speedUpgrade);
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