using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    [SerializeField] private GameObject farmWeaponsPrefab;  // Reference to the Farm_Weapons prefab containing all weapons
    [SerializeField] private Transform[] spawnPoints;  // Array of predefined spawn points in the scene
    public float respawnTime = 10f;  // Adjust the respawn time here

    private List<GameObject> weaponPrefabs = new List<GameObject>();  // List to hold each weapon prefab as a separate object
    private bool[] spawnPointOccupied;  // Track which spawn points are occupied

    private void Start()
    {
        // Initialize the occupied array
        spawnPointOccupied = new bool[spawnPoints.Length];

        // Load each child of farmWeaponsPrefab as a separate weapon prefab
        foreach (Transform weapon in farmWeaponsPrefab.transform)
        {
            weaponPrefabs.Add(weapon.gameObject);
        }

        // Place initial weapons in the scene
        InitialPlaceWeapons();

        // Start the respawn loop
        StartCoroutine(RespawnWeapons());
    }

    private void InitialPlaceWeapons()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            // Pick a random weapon from the weaponPrefabs list
            GameObject randomWeaponPrefab = weaponPrefabs[Random.Range(0, weaponPrefabs.Count)];

            // Instantiate a copy of the weapon at the spawn point
            GameObject newWeapon = Instantiate(randomWeaponPrefab, spawnPoints[i].position, Quaternion.identity);
            newWeapon.SetActive(true);

            // Mark the spawn point as occupied
            spawnPointOccupied[i] = true;
        }
    }

    private IEnumerator RespawnWeapons()
    {
        while (true)  // Continuously check for free spawn points
        {
            yield return new WaitForSeconds(respawnTime);  // Wait for the specified respawn time

            int spawnIndex = GetFreeSpawnPoint();
            if (spawnIndex != -1)  // If a free spawn point is found
            {
                Transform spawnPoint = spawnPoints[spawnIndex];

                // Pick a random weapon from the weaponPrefabs list
                GameObject randomWeaponPrefab = weaponPrefabs[Random.Range(0, weaponPrefabs.Count)];

                // Instantiate a new instance of the selected weapon at the spawn point
                GameObject newWeapon = Instantiate(randomWeaponPrefab, spawnPoint.position, Quaternion.identity);
                newWeapon.SetActive(true);

                // Mark the spawn point as occupied
                spawnPointOccupied[spawnIndex] = true;

                Debug.Log("Spawned individual weapon: " + randomWeaponPrefab.name + " at " + spawnPoint.name);
            }
            else
            {
                Debug.LogWarning("No free spawn points available for spawning new weapons.");
            }
        }
    }

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
    public void WeaponPickedUp(Transform weaponTransform)
    {
        // Find the spawn point corresponding to the weapon's position
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (spawnPoints[i].position == weaponTransform.position)
            {
                spawnPointOccupied[i] = false;  // Mark the spawn point as free
                Debug.Log("Spawn point freed: " + spawnPoints[i].name);
                break;
            }
        }
    }
}
