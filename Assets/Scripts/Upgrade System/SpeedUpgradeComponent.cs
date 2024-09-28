using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpgradeComponent : MonoBehaviour
{
    public SpeedUpgradeSO speedUpgradeData; // Reference to Scriptable Object

    //Playermovement references
    private PlayerMovement playerMovement; // Reference to the player's movement script
    private int currentSpeedLevel = 0;

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

    public void Upgrade()
    {
        if (playerMovement != null)
        {
            //playerMovement.speed += speedUpgradeData.speedIncrease;
            currentSpeedLevel++;
            Debug.Log("Speed upgraded! Current speed level: " + currentSpeedLevel);
        }
    }
}
