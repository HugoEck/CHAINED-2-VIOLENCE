using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainUpgrade : UpgradeBase
{
    private AdjustChainLength adjustChainLength;
    private int lengthIncreasePerUpgrade;

    public ChainUpgrade(AdjustChainLength adjustChainLength, int lengthIncreasePerUpgrade, int maxLevel, int upgradeCostIncrease) : base(maxLevel, upgradeCostIncrease)
    {
        this.adjustChainLength = adjustChainLength;
        this.lengthIncreasePerUpgrade = lengthIncreasePerUpgrade;
    }

    public override void Upgrade()
    {
        #region TMP
        if (currentLevel >= AdjustChainLength.AMOUNT_OF_UPGRADES)
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

        if (adjustChainLength != null && currentLevel < AdjustChainLength.AMOUNT_OF_UPGRADES)
        {
            adjustChainLength.IncreaseRopeLength(lengthIncreasePerUpgrade);
            currentLevel++;
            GoldDropManager.Instance.SpendGold(CalculateUpgradeCost());

            Debug.Log("Chain upgraded - New Chain Length: " + adjustChainLength.ReturnCurrentChainLength());
        }
        else
        {
            Debug.LogWarning("Maxed chain level reached");
            UpgradeManager.Instance.StartCoroutine(UpgradeManager.Instance.ShowMaxUpgradeReachedMessage());
        }
    }
}

