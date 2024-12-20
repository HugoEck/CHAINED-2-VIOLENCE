using Obi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject _spawnPoint;

    // Reference to the parent GameObject that contains all players and the chain
    private GameObject PlayerParent;

    private void Start()
    {
        // Find the PlayerParent GameObject (you can set this directly in the inspector or dynamically find it)
        PlayerParent = GameObject.FindWithTag("PlayerParent");

        // Set player positions (this will move the entire PlayerParent to the spawn point)
        SetPlayerPositions();
    }

    public void SetPlayerPositions()
    {
        if (PlayerParent != null && _spawnPoint != null)
        {
            // Move the PlayerParent to the spawn point
            PlayerParent.transform.position = _spawnPoint.transform.position;

            // All children will automatically maintain their relative positions to the PlayerParent.
        }
        else
        {
            Debug.LogError("PlayerParent or SpawnPoint not set.");
        }
    }
}


