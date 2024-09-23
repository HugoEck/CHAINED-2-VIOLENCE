using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUpgradeComponent : MonoBehaviour
{
    public DamageUpgradeSO damageUpgradeData; // Reference to Scriptable Object

    public int baseDamage = 10;

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
        if(damageUpgradeData != null)
        {
            baseDamage += damageUpgradeData.damageIncrease;
            Debug.Log("Damage upgraded! New Base Damage: " + baseDamage);
        }
        else
        {
            Debug.LogWarning("No Damage Upgrade Data assigned.");
        }
    }
}
