using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldDropManager : MonoBehaviour
{
    public ObjectPool goldPool;
    [SerializeField] float dropChance = 0.2f; //20% chance to drop gold for now.

    public void HandleEnemyDefeated(Vector3 enemyPostion)
    {
        if (Random.value <= dropChance)
        {
            GameObject gold = goldPool.GetObject();
            gold.transform.position = enemyPostion; // Spawn the gold at enemy pos
        }
    }

    public void ReturnGold(GameObject gold)
    { 
        goldPool.ReturnObject(gold);
    }
}
