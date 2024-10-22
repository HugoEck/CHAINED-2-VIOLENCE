using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUpgrade : UpgradeBase
{
    private Player player1;
    private Player player2;
    private PlayerCombat player1Combat;
    private PlayerCombat player2Combat;
    private float damageIncrease;
    private float maxDamageMultiplier;
    private float initialAttackDamage = 10f;

    public DamageUpgrade(Player p1, Player p2, PlayerCombat p1Combat, PlayerCombat p2Combat, float damageIncrease, float maxDamageMultiplier, int maxLevel, int upgradeCostIncrease)
        : base(maxLevel, upgradeCostIncrease)
    {
        this.player1 = p1;
        this.player2 = p2;
        this.player1Combat = p1Combat;
        this.player2Combat = p2Combat;
        this.damageIncrease = damageIncrease;
        this.maxDamageMultiplier = maxDamageMultiplier;
    }

    public override void Upgrade()
    {
        if (!CanUpgrade(GoldDropManager.Instance.GetGoldAmount()))
        {
            Debug.LogWarning("Not enough gold or max level reached for Damage upgrade!");
            return;
        }

        float newDamage = player1Combat.attackDamage + damageIncrease;
        float maxAllowedDamage = initialAttackDamage * maxDamageMultiplier;

        if (newDamage <= maxAllowedDamage)
        {
            player1Combat.SetAttackDamage(newDamage);
            player2Combat.SetAttackDamage(newDamage);
            currentLevel++;

            GoldDropManager.Instance.SpendGold(CalculateUpgradeCost());
            Debug.Log("Damage upgraded for both players! New Attack Damage: " + newDamage);
        }
        else
        {
            Debug.LogWarning("Damage upgrade exceeds the maximum allowed limit.");
        }
    }
}
