using UnityEngine;

public abstract class UpgradeBase
{
    protected int maxLevel;
    protected int upgradeCostIncrease;

    public int currentLevel { get; protected set; } = 0;
    public int MaxLevel => maxLevel;

    public UpgradeBase(int maxLevel, int upgradeCostIncrease)
    {
        this.maxLevel = maxLevel;
        this.upgradeCostIncrease = upgradeCostIncrease;
    }

    public abstract void Upgrade();

    public int CalculateUpgradeCost()
    {
        return (currentLevel + 1) * upgradeCostIncrease;
    }

    protected bool CanUpgrade(int currentGold)
    {
        return currentLevel < maxLevel && currentGold >= CalculateUpgradeCost();
    }
}