using UnityEngine;
/// <summary>
/// Damage upgrade class, adjust variables and parameters in the upgrade manager.
/// </summary>
public class DamageUpgrade : UpgradeBase
{
    private PlayerCombat player1Combat;
    private PlayerCombat player2Combat;
    private float damageIncrease;
    private float maxDamageMultiplier;

    public DamageUpgrade(PlayerCombat p1Combat, PlayerCombat p2Combat, float damageIncrease, float maxDamageMultiplier, int maxLevel, int upgradeCostIncrease) : base(maxLevel, upgradeCostIncrease)
    {
        this.player1Combat = p1Combat;
        this.player2Combat = p2Combat;
        this.damageIncrease = damageIncrease;
        this.maxDamageMultiplier = maxDamageMultiplier;
    }

    public override void Upgrade()
    {
        float initialAttackDamage = 10f; //Set to same value as player damage when game is started, put this in playerCombat.
        //float initialAttackDamage = player1Combat.InitialAttackDamage;
        #region TMP
        if (currentLevel >= maxLevel)
        {
            UpgradeManager.Instance.StartCoroutine(UpgradeManager.Instance.ShowMaxUpgradeReachedMessage());
            return;
        }

        if (!CanUpgrade(GoldDropManager.Instance.GetGoldAmount()))
        {
            UpgradeManager.Instance.StartCoroutine(UpgradeManager.Instance.ShowNotEnoughGoldMessage());
            return;
        }
        #endregion

        float newDamage = player1Combat.attackDamage + damageIncrease;
        float maxAllowedDamage = initialAttackDamage * maxDamageMultiplier;

        if (newDamage <= maxAllowedDamage)
        {
            player1Combat.SetAttackDamage(newDamage);
            player2Combat.SetAttackDamage(newDamage);
            currentLevel++;

            GoldDropManager.Instance.SpendGold(CalculateUpgradeCost());
            Debug.Log("Damage upgraded - New Attack Damage: " + newDamage);
        }
        else
        {
            Debug.LogWarning("Max damage upgrade reached");
            UpgradeManager.Instance.StartCoroutine(UpgradeManager.Instance.ShowMaxUpgradeReachedMessage());
        }
    }
}
