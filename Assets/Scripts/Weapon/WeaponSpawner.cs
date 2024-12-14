using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RarityProbability
{
    public WeaponRarityHandler.WeaponRarity rarity;
    public float spawnChance; // The relative weight of this rarity
}

public class WeaponSpawner : MonoBehaviour
{
    [SerializeField] private GameObject farmWeaponsPrefab;
    [SerializeField] private GameObject warriorWeaponsPrefab;
    [SerializeField] private GameObject medievalWeaponsPrefab;
    [SerializeField] private Transform waypointsParent;  // Reference to the parent GameObject containing waypoints as child objects
    [SerializeField] private float respawnTime = 10f;

    [Header("Rarity Spawn Probabilities")]
    [SerializeField] private List<RarityProbability> rarityProbabilities;

    private List<GameObject> weaponPrefabs = new List<GameObject>();
    private Transform[] spawnPoints;
    private bool[] spawnPointOccupied;

    private void Start()
    {
        // Automatically populate the spawnPoints array from waypointsParent's children
        spawnPoints = new Transform[waypointsParent.childCount];
        spawnPointOccupied = new bool[spawnPoints.Length];

        for (int i = 0; i < waypointsParent.childCount; i++)
        {
            spawnPoints[i] = waypointsParent.GetChild(i);
        }

        // Add all weapons from the different categories
        foreach (Transform weapon in farmWeaponsPrefab.transform)
        {
            weaponPrefabs.Add(weapon.gameObject);
        }
        foreach (Transform weapon in warriorWeaponsPrefab.transform)
        {
            weaponPrefabs.Add(weapon.gameObject);
        }
        foreach (Transform weapon in medievalWeaponsPrefab.transform)
        {
            weaponPrefabs.Add(weapon.gameObject);
        }

        PlaceInitialWeapons();
        StartCoroutine(RespawnWeapons());
    }

    private void PlaceInitialWeapons()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            SpawnWeaponAt(i);
        }
    }

    private void SpawnWeaponAt(int spawnIndex)
    {
        if (!spawnPointOccupied[spawnIndex])
        {
            // Filter weapons by chosen rarity
            WeaponRarityHandler.WeaponRarity chosenRarity = GetRandomRarity();
            List<GameObject> filteredWeapons = weaponPrefabs.FindAll(w =>
            {
                var rarityHandler = w.GetComponent<WeaponRarityHandler>();
                return rarityHandler != null && rarityHandler.rarity == chosenRarity;
            });

            if (filteredWeapons.Count == 0)
            {
                Debug.LogWarning($"No weapons found for rarity {chosenRarity}");
                return;
            }

            // Spawn a random weapon from the filtered list
            GameObject randomWeaponPrefab = filteredWeapons[Random.Range(0, filteredWeapons.Count)];
            GameObject newWeapon = Instantiate(randomWeaponPrefab, spawnPoints[spawnIndex].position, Quaternion.identity);
            newWeapon.SetActive(true);

            spawnPointOccupied[spawnIndex] = true;
        }
    }

    private WeaponRarityHandler.WeaponRarity GetRandomRarity()
    {
        // Calculate total weight
        float totalWeight = 0f;
        foreach (var rarity in rarityProbabilities)
        {
            totalWeight += rarity.spawnChance;
        }

        // Choose a random value within the total weight
        float randomValue = Random.Range(0, totalWeight);

        // Determine the rarity
        foreach (var rarity in rarityProbabilities)
        {
            if (randomValue < rarity.spawnChance)
            {
                return rarity.rarity;
            }
            randomValue -= rarity.spawnChance;
        }

        // Fallback to a default rarity
        return WeaponRarityHandler.WeaponRarity.Common;
    }

    private IEnumerator RespawnWeapons()
    {
        while (true)
        {
            yield return new WaitForSeconds(respawnTime);
            int freeSpawnIndex = GetFreeSpawnPoint();

            if (freeSpawnIndex != -1)
            {
                SpawnWeaponAt(freeSpawnIndex);
            }
        }
    }

    private int GetFreeSpawnPoint()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (!spawnPointOccupied[i]) return i;
        }
        return -1;
    }

    public void WeaponPickedUp(Transform weaponTransform)
    {
        float thresholdDistance = 0.5f;

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (Vector3.Distance(spawnPoints[i].position, weaponTransform.position) < thresholdDistance)
            {
                spawnPointOccupied[i] = false;
                return;
            }
        }
    }
}
