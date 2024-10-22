using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainUpgrade : UpgradeBase
{
    private AdjustChainLength adjustChainLength;
    private int lengthIncreasePerUpgrade;

    public ChainUpgrade(AdjustChainLength adjustChainLength, int lengthIncreasePerUpgrade, int maxLevel, int upgradeCostIncrease)
        : base(maxLevel, upgradeCostIncrease)
    {
        this.adjustChainLength = adjustChainLength;
        this.lengthIncreasePerUpgrade = lengthIncreasePerUpgrade;
    }

    public override void Upgrade()
    {
        if (!CanUpgrade(GoldDropManager.Instance.GetGoldAmount()))
        {
            Debug.LogWarning("Not enough gold or max level reached for Chain upgrade!");
            return;
        }

        if (adjustChainLength != null && currentLevel < AdjustChainLength.AMOUNT_OF_UPGRADES)
        {
            adjustChainLength.IncreaseRopeLength(lengthIncreasePerUpgrade);
            currentLevel++;
            GoldDropManager.Instance.SpendGold(CalculateUpgradeCost());

            Debug.Log("Chain upgraded. New Chain Length: " + adjustChainLength.ReturnCurrentChainLength());
        }
        else
        {
            Debug.LogWarning("Maximum chain length reached.");
        }
    }
}

