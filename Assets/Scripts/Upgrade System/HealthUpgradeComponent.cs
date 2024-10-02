using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpgradeComponent : MonoBehaviour
{
    private PlayerManager playerManager;
    public int currentUpgradeLevel = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        if (playerManager == null)
        {
            Debug.LogError("PlayerManager component is missing!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Upgrade(HealthUpgradeSO healthUpgradeData)
    {
        if (playerManager != null && healthUpgradeData != null)
        {
            playerManager.maxHealth += healthUpgradeData.healthIncrease;
            playerManager.currentHealth = playerManager.maxHealth; // heal to full after the upgrade?
            currentUpgradeLevel++;
            Debug.Log("Health upgraded by " + healthUpgradeData.healthIncrease + ". New Max Health: " + playerManager.maxHealth);
        }
        else
        {
            Debug.LogWarning("Failed to upgrade health. PlayerManager or HealthUpgradeSO is null.");
        }
    }
}
