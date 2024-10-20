using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldDropManager : MonoBehaviour
{
    [SerializeField] private float dropChance = 0.2f; // = 20% chance to drop
    [SerializeField] private int goldAmount = 0;

    public void HandleEnemyDefeated(Vector3 enemyPosition)
    {
        if (Random.value <= dropChance)
        {
            goldAmount += 1;
            Debug.Log("Gold dropped at: " + enemyPosition + ". Total gold: " + goldAmount);
        }
    }

    public int GetGoldAmount()
    {
        return goldAmount;
    }
}
