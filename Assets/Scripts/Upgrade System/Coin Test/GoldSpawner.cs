using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// FOR TESTING PURPOSE, REMOVE LATER ONCE CROWD AND EMEIES DROP GOLD
/// </summary>
public class GoldSpawner : MonoBehaviour
{
    public GoldDropManager goldDropManager;
    public Transform spawnPoint;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) 
        {
            SpawnGold();
        }
    }

    private void SpawnGold()
    {
        if (goldDropManager != null && spawnPoint != null)
        {
            goldDropManager.HandleEnemyDefeated(spawnPoint.position);
            Debug.Log("Gold spawned at: " + spawnPoint.position);
        }
        else
        {
            Debug.LogWarning("GoldDropManager or SpawnPoint is not assigned.");
        }
    }
}
