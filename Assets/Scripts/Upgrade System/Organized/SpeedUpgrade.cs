using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpgrade : UpgradeBase
{
    private PlayerMovement player1Movement;
    private PlayerMovement player2Movement;
    private float speedIncrease;
    private float initialSpeed;
    private float maxSpeedMultiplier;

    public SpeedUpgrade(PlayerMovement p1Movement, PlayerMovement p2Movement, float speedIncrease, float initialSpeed, float maxSpeedMultiplier, int maxLevel, int upgradeCostIncrease)
        : base(maxLevel, upgradeCostIncrease)
    {
        this.player1Movement = p1Movement;
        this.player2Movement = p2Movement;
        this.speedIncrease = speedIncrease;
        this.initialSpeed = initialSpeed;
        this.maxSpeedMultiplier = maxSpeedMultiplier;
    }

    public override void Upgrade()
    {
        if (!CanUpgrade(GoldDropManager.Instance.GetGoldAmount()))
        {
            Debug.LogWarning("Not enough gold or max level reached for Speed upgrade!");
            return;
        }

        // Calculate the new speed based on the current walking speed (to account for previous upgrades)
        float newSpeed = player1Movement.GetWalkingSpeed() + speedIncrease;
        float maxAllowedSpeed = initialSpeed * maxSpeedMultiplier;

        // Ensure the new speed doesn't exceed the maximum allowed speed (30% increase cap)
        if (newSpeed <= maxAllowedSpeed)
        {
            player1Movement.SetWalkingSpeed(newSpeed);
            player2Movement.SetWalkingSpeed(newSpeed);
            currentLevel++;

            GoldDropManager.Instance.SpendGold(CalculateUpgradeCost());

            Debug.Log("Speed upgraded for both players! New Speed: " + newSpeed);
        }
        else
        {
            Debug.LogWarning("Cannot upgrade speed further; maximum limit reached.");
        }
    }
}
