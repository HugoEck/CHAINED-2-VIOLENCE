using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpgradeComponent : MonoBehaviour
{
    public HealthUpgradeSO healthUpgradeData; // Reference to Scriptable Object

    public int currentHealth = 100;
    public int maxHealth = 100;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Upgrade()
    {
        if (healthUpgradeData != null)
        {
            maxHealth += healthUpgradeData.healthIncrease;
            currentHealth = maxHealth; // Heals to full when upgrade //remove later on
            Debug.Log("Health upgraded! New Max Health: " + maxHealth);
        }
        else
        {
            Debug.LogWarning("No Health Upgrade Data Assigned");
        }
    }
}
