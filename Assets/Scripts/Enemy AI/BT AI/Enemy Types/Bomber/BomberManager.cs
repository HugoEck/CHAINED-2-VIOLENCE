using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberManager : BaseManager
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadStats()
    {
        maxHealth = 10 + maxHealthModifier;
        currentHealth = maxHealth;
        attack = 50 + attackModifier;
        defense = 0 + defenseModifier;
        navigation.maxSpeed = 5;
        attackSpeed = 1 + attackSpeedModifier;
        attackRange = 2.5f;
        unitCost = 1;

    }
}
