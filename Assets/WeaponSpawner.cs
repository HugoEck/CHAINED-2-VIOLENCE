using System.Collections;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    [SerializeField] private GameObject farmWeaponsPrefab;  // Reference to the Farm_Weapons prefab containing all weapons
    [SerializeField] private Transform[] spawnPoints;  // Array of predefined spawn points in the scene
    public float respawnTime = 1f;

    private GameObject instantiatedFarmWeapons;  // The instance of the Farm_Weapons prefab
    private bool[] spawnPointOccupied;  // Track which spawn points are occupied

    private void Start()
    {
        // Initialize the occupied array
        spawnPointOccupied = new bool[spawnPoints.Length];

        // Instantiate the Farm_Weapons prefab into the scene
        instantiatedFarmWeapons = Instantiate(farmWeaponsPrefab);

        // Place the weapons in the scene
        PlaceWeapons();

        // Start the respawn loop
        StartCoroutine(RespawnWeapons());
    }

    private void PlaceWeapons()
    {
        foreach (Transform weapon in instantiatedFarmWeapons.transform)
        {
            // Find a free spawn point
            int spawnIndex = GetFreeSpawnPoint();
            if (spawnIndex != -1)  // If a free spawn point is found
            {
                // Move the weapon to the free spawn point and activate it
                Transform spawnPoint = spawnPoints[spawnIndex];
                weapon.position = spawnPoint.position;
                weapon.gameObject.SetActive(true);

                // Mark this spawn point as occupied
                spawnPointOccupied[spawnIndex] = true;
            }
            else
            {
                Debug.LogWarning("No free spawn points available.");
            }
        }
    }

    // Method to find an available spawn point
    private int GetFreeSpawnPoint()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (!spawnPointOccupied[i])  // If the spawn point is not occupied
            {
                return i;  // Return the index of the free spawn point
            }
        }
        return -1;  // No free spawn points available
    }

    // Call this when a weapon is picked up to free the spawn point
    public void WeaponPickedUp(Transform spawnPoint)
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (spawnPoints[i] == spawnPoint)
            {
                spawnPointOccupied[i] = false;  // Mark the spawn point as free
                Debug.Log("Spawn point freed: " + spawnPoint.name);
                break;
            }
        }
    }

    // Coroutine to respawn weapons after a delay
    private IEnumerator RespawnWeapons()
    {
        while (true)  // Continuously check for free spawn points
        {
            yield return new WaitForSeconds(respawnTime);  // Wait for the specified respawn time

            // Check if there are any free spawn points and respawn weapons
            foreach (Transform weapon in instantiatedFarmWeapons.transform)
            {
                if (!weapon.gameObject.activeInHierarchy)  // If the weapon is not active (picked up)
                {
                    int spawnIndex = GetFreeSpawnPoint();
                    if (spawnIndex != -1)  // If a free spawn point is found
                    {
                        // Move the weapon to the free spawn point and activate it
                        Transform spawnPoint = spawnPoints[spawnIndex];
                        weapon.position = spawnPoint.position;
                        weapon.gameObject.SetActive(true);

                        // Mark the spawn point as occupied
                        spawnPointOccupied[spawnIndex] = true;
                        Debug.Log("Weapon respawned at: " + spawnPoint.name);
                    }
                    else
                    {
                        Debug.LogWarning("No free spawn points available for respawning.");
                    }
                }
            }
        }
    }
}
