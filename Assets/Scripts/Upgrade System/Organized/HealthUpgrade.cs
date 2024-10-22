using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpgrade : UpgradeBase
{
    private Player player1;
    private Player player2;
    private float healthIncrease;
    private float maxHealthMultiplier;
    private float initialMaxHealth = 100f;

    public HealthUpgrade(Player p1, Player p2, float healthIncrease, float maxHealthMultiplier, int maxLevel, int upgradeCostIncrease)
        : base(maxLevel, upgradeCostIncrease)
    {
        this.player1 = p1;
        this.player2 = p2;
        this.healthIncrease = healthIncrease;
        this.maxHealthMultiplier = maxHealthMultiplier;
    }

    public override void Upgrade()
    {
        if (!CanUpgrade(GoldDropManager.Instance.GetGoldAmount()))
        {
            Debug.LogWarning("Not enough gold or max level reached for Health upgrade!");
            return;
        }

        float newMaxHealth = player1.currentHealth + healthIncrease;
        float maxAllowedHealth = initialMaxHealth * maxHealthMultiplier;

        if (newMaxHealth <= maxAllowedHealth)
        {
            player1.SetMaxHealth(newMaxHealth);
            player2.SetMaxHealth(newMaxHealth);
            currentLevel++;

            GoldDropManager.Instance.SpendGold(CalculateUpgradeCost());
            Debug.Log("Health upgraded for both players! New Max Health: " + newMaxHealth);
        }
        else
        {
            Debug.LogWarning("Health upgrade exceeds the maximum allowed limit.");
        }
    }
}
