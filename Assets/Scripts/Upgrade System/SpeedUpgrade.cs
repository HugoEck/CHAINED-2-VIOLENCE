using UnityEngine;
/// <summary>
/// Movement speed upgrade class, adjust variables and parameters in the upgrade manager.
/// </summary>
public class SpeedUpgrade : UpgradeBase
{
    private PlayerMovement player1Movement;
    private PlayerMovement player2Movement;
    private float speedIncrease;
    //private float initialSpeed;
    private float maxSpeedMultiplier;

    public SpeedUpgrade(PlayerMovement p1Movement, PlayerMovement p2Movement, float speedIncrease, float maxSpeedMultiplier, int maxLevel, int upgradeCostIncrease) : base(maxLevel, upgradeCostIncrease)
    {
        this.player1Movement = p1Movement;
        this.player2Movement = p2Movement;
        this.speedIncrease = speedIncrease;
        this.maxSpeedMultiplier = maxSpeedMultiplier;
    }

    public override void Upgrade()
    {
        //float initialSpeed = 1000f; //Set to same value as starting movement speed.. //Put this in playermovement.
        float initialSpeed = player1Movement.originalWalkingSpeed;

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

        float newSpeed = player1Movement.GetWalkingSpeed() + speedIncrease;
        float maxAllowedSpeed = initialSpeed * maxSpeedMultiplier;

        if (newSpeed <= maxAllowedSpeed)
        {
            player1Movement.SetWalkingSpeed(newSpeed);
            player2Movement.SetWalkingSpeed(newSpeed);
            currentLevel++;

            GoldDropManager.Instance.SpendGold(CalculateUpgradeCost());

            Debug.Log("Speed upgraded - speed: " + newSpeed);
        }
        else
        {
            Debug.LogWarning("Max speed upgrade reached");
            UpgradeManager.Instance.StartCoroutine(UpgradeManager.Instance.ShowMaxUpgradeReachedMessage());
        }
    }
}