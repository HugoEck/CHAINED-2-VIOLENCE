using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    [SerializeField] private GameObject farmWeaponsPrefab;
    [SerializeField] private GameObject warriorWeaponsPrefab;
    [SerializeField] private GameObject medievalWeaponsPrefab;
    [SerializeField] private Transform waypointsParent;  // Reference to the parent GameObject containing waypoints as child objects
    [SerializeField] private float respawnTime = 10f;

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
            GameObject randomWeaponPrefab = weaponPrefabs[Random.Range(0, weaponPrefabs.Count)];
            GameObject newWeapon = Instantiate(randomWeaponPrefab, spawnPoints[spawnIndex].position, Quaternion.identity);
            newWeapon.SetActive(true);

            spawnPointOccupied[spawnIndex] = true;
        }
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
