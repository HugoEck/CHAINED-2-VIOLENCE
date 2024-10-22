using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class UpgradeBase
{
    //protected int currentLevel = 0;
    protected int maxLevel;
    protected int upgradeCostIncrease;

    public int currentLevel { get; protected set; } = 0;
    public UpgradeBase(int maxLevel, int upgradeCostIncrease)
    {
        this.maxLevel = maxLevel;
        this.upgradeCostIncrease = upgradeCostIncrease;
    }

    public abstract void Upgrade();

    protected int CalculateUpgradeCost()
    {
        return (currentLevel + 1) * upgradeCostIncrease;
    }

    protected bool CanUpgrade(int currentGold)
    {
        return currentLevel < maxLevel && currentGold >= CalculateUpgradeCost();
    }
}

