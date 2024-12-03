using UnityEngine;
/// <summary>
/// Health upgrade class, adjust variables and parameters in the upgrade manager.
/// </summary>
public class HealthUpgrade : UpgradeBase
{
    private Player player1;
    private Player player2;
    private float healthIncrease;
    private float maxHealthMultiplier;

    public HealthUpgrade(Player p1, Player p2, float healthIncrease, float maxHealthMultiplier, int maxLevel, int upgradeCostIncrease) : base(maxLevel, upgradeCostIncrease)
    {
        this.player1 = p1;
        this.player2 = p2;
        this.healthIncrease = healthIncrease;
        this.maxHealthMultiplier = maxHealthMultiplier;
    }

    public override void Upgrade()
    {
        //float initialMaxHealth = 100f; //Give player a max health?

        //#region TMP
        //if (currentLevel >= maxLevel)
        //{
        //    UpgradeManager.Instance.StartCoroutine(UpgradeManager.Instance.ShowMaxUpgradeReachedMessage());
        //    return;
        //}

        //if (!CanUpgrade(GoldDropManager.Instance.GetGoldAmount()))
        //{
        //    Debug.LogWarning("Not enough gold or max level is reached for health upgrade");
        //    UpgradeManager.Instance.StartCoroutine(UpgradeManager.Instance.ShowNotEnoughGoldMessage());
        //    return;
        //}
        //#endregion

        //float initialMaxHealth = player1.InitialMaxHealth;
        //float newMaxHealth = player1.currentHealth + healthIncrease;
        //float maxAllowedHealth = initialMaxHealth * maxHealthMultiplier;

        //if (newMaxHealth <= maxAllowedHealth)
        //{
        //    player1.SetMaxHealth(newMaxHealth);
        //    player2.SetMaxHealth(newMaxHealth);
        //    currentLevel++;

        //    GoldDropManager.Instance.SpendGold(CalculateUpgradeCost());
        //    Debug.Log("Health upgraded - health: " + newMaxHealth + "current hp = " + player1.currentHealth);
        //}
        //else
        //{
        //    Debug.LogWarning("Max health upgrade reached");
        //    UpgradeManager.Instance.StartCoroutine(UpgradeManager.Instance.ShowMaxUpgradeReachedMessage());
        //}
    }
}
