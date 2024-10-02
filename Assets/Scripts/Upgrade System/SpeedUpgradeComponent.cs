using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpgradeComponent : MonoBehaviour
{
    private PlayerMovement playerMovement; // Reference to the player's movement script
    public int currentUpgradeLevel = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement component is missing!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Upgrade(SpeedUpgradeSO speedUpgradeData)
    {
        if (playerMovement != null)
        {
            playerMovement.SetWalkingSpeed(playerMovement.WalkingSpeed + speedUpgradeData.speedIncrease);
            currentUpgradeLevel++;
            Debug.Log("Speed upgraded! Current speed level: " + currentUpgradeLevel);
        }
    }
}
