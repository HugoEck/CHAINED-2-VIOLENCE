using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageUpgrade", menuName = "Upgrades/Damage")]
public class DamageUpgradeSO : ScriptableObject
{
    public float damageIncrease;
    public int cost; //FIX LATER WHEN WE HAVE GOLD SCRIPT

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
