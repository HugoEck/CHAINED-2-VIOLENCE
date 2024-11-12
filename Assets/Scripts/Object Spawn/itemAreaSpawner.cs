using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class itemAreaSpawner : MonoBehaviour
{
    #region Variables
    // Array of objects that can be spawned
    public GameObject[] romanObjects;
    public GameObject[] fantasyObjects;
    public GameObject[] pirateObjects;
    public GameObject[] westernObjects;
    public GameObject[] farmObjects;
    public GameObject[] modernDayObjects;
    public GameObject[] scifiObjects;

    // List of objects that has been added
    public List<GameObject> spawnedObjects = new List<GameObject>();

    // Max objects that can be spawned at the time
    public int numObjectsToSpawn;

    // Size of the box for overlap check
    public float overlapTestBoxSize = 1f;
    // The spread of objects around the map
    public float itemXSpread;
    public float itemYSpread;
    public float itemZSpread;
    private float cooldownTimer = 0f;

    // Layer that is to be checked for overlaps
    public LayerMask spawnedObjectLayer;
    public WaveManager waveManager;

    private bool itemsSpawnedForRomanWave = false;
    private bool itemsSpawnedForFantasyWave = false;
    private bool itemsSpawnedForPirateWave = false;
    private bool itemsSpawnedForWesternWave = false;
    private bool itemsSpawnedForFarmWave = false;
    private bool itemsSpawnedForModernDayWave = false;
    private bool itemsSpawnedForSciFiWave = false;

    private bool isDespawning = false;

    #endregion

    private void Update()
    {
        SpawnWithWaves();

        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    WaveManager.currentWave++;
        //    Debug.Log(WaveManager.currentWave);
        //}
    }

    void SpawnItems(GameObject[] objectArray)
    {
        int successfullySpawned = 0;

        for (int i = 0; i < numObjectsToSpawn; i++)
        {
            if (SpreadItem(objectArray))  // Only increment if an item was successfully spawned
            {
                successfullySpawned++;
            }

            if (successfullySpawned >= numObjectsToSpawn)
            {
                break;  // Stop when the desired number of objects is spawned
            }
        }
    }

    bool SpreadItem(GameObject[] objectArray)
    {
        int maxAttempts = 5;  // Maximum number of attempts to find a valid position
        int attempt = 0;

        if (attempt < maxAttempts)
        {
            attempt++;

            // Generate a random position within the spread
            Vector3 randPosition = new Vector3(
                Random.Range(-itemXSpread, itemXSpread),
                0f,  // Surface Y-position
                Random.Range(-itemZSpread, itemZSpread)) + transform.position;

            // Pick a random item from the selected object array
            int randomIndex = Random.Range(0, objectArray.Length);
            GameObject itemToSpread = objectArray[randomIndex];

            // Perform overlap check using an overlap box
            Vector3 overlapTestBoxScale = new Vector3(overlapTestBoxSize, overlapTestBoxSize, overlapTestBoxSize);
            Collider[] collidersInsideOverlapBox = new Collider[10];
            int numberOfCollidersFound = Physics.OverlapBoxNonAlloc(randPosition, overlapTestBoxScale, collidersInsideOverlapBox, Quaternion.identity, spawnedObjectLayer);

            // If no collisions, spawn the object
            if (numberOfCollidersFound == 0)
            {
                GameObject clone = Instantiate(itemToSpread, randPosition, Quaternion.identity);
                spawnedObjects.Add(clone);

                return true;  // Successfully spawned an item
            }
        }
        return false;  // Failed to spawn due to overlap
    }   

    void DespawnObjects()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
            {
                TrapManager trap = obj.GetComponent<TrapManager>();
                TrampolineManager trampoline = obj.GetComponent<TrampolineManager>();
                ObjectShader objectShader = obj.GetComponent<ObjectShader>();

                if (trap != null)
                {
                    objectShader.StartDespawn();
                    trap.DespawnTrap();
                }
                if (trampoline != null)
                {
                    trampoline.DespawnTrampoline();
                }
                else
                {
                    //obj.AddComponent<MoveDownwards>();
                    objectShader.StartDespawn();
                }
            }
        }
        spawnedObjects.Clear();
    }

    void SpawnWithWaves()
    {
        if (waveManager != null)
        {
            // Wave 1 - Spawn Roman objects
            if (WaveManager.currentWave == 1 && !itemsSpawnedForRomanWave)
            {
                SpawnItems(romanObjects);
                itemsSpawnedForRomanWave = true;
            }

            // For each wave with despawning and cooldown before spawning new items
            HandleWave(5, fantasyObjects, ref itemsSpawnedForFantasyWave);
            HandleWave(10, pirateObjects, ref itemsSpawnedForPirateWave);
            HandleWave(15, westernObjects, ref itemsSpawnedForWesternWave);
            HandleWave(20, farmObjects, ref itemsSpawnedForFarmWave);
            HandleWave(25, modernDayObjects, ref itemsSpawnedForModernDayWave);
            HandleWave(30, scifiObjects, ref itemsSpawnedForSciFiWave);
        }
    }

    void HandleWave(int waveNumber, GameObject[] objectsToSpawn, ref bool itemsSpawnedFlag)
    {
        if (WaveManager.currentWave == waveNumber && !itemsSpawnedFlag)
        {
            if (!isDespawning)
            {
                DespawnObjects();
                isDespawning = true;
                cooldownTimer = 5f; // Reset cooldown timer
            }
            else
            {
                cooldownTimer -= Time.deltaTime;
                if (cooldownTimer <= 0)
                {
                    SpawnItems(objectsToSpawn);
                    itemsSpawnedFlag = true;
                    isDespawning = false; // Reset despawning for next wave
                }
            }
        }
    }

    //private void SpawnWithWaves()
    //{
    //    if (waveManager != null)
    //    {
    //        // Wave 1 - Spawn Roman objects
    //        if (WaveManager.currentWave == 1 && !itemsSpawnedForRomanWave)
    //        {
    //            SpawnItems(romanObjects);
    //            itemsSpawnedForRomanWave = true;
    //        }

    //        // Wave 5 - Despawn, wait for the cooldown, then spawn Fantasy objects
    //        if (WaveManager.currentWave == 5 && !itemsSpawnedForFantasyWave)
    //        {
    //            if (!isDespawning)
    //            {
    //                DespawnObjects();
    //                isDespawning = true;
    //                cooldownTimer = 5f;
    //            }
    //            else
    //            {
    //                cooldownTimer -= Time.deltaTime;
    //                if (cooldownTimer <= 0)
    //                {
    //                    SpawnItems(fantasyObjects);
    //                    itemsSpawnedForFantasyWave = true;
    //                    isDespawning = false;
    //                }
    //            }
    //        }

    //        // Add additional waves as needed with similar logic

    //        // Example for Pirate Wave 10
    //        if (WaveManager.currentWave == 10 && !itemsSpawnedForPirateWave)
    //        {
    //            if (!isDespawning)
    //            {
    //                DespawnObjects();
    //                isDespawning = true;
    //                cooldownTimer = 5f;
    //            }
    //            else
    //            {
    //                cooldownTimer -= Time.deltaTime;
    //                if (cooldownTimer <= 0)
    //                {
    //                    SpawnItems(pirateObjects);
    //                    itemsSpawnedForPirateWave = true;
    //                    isDespawning = false;
    //                }
    //            }
    //        }
    //        // Wave 15
    //        if (WaveManager.currentWave == 15 && !itemsSpawnedForWesternWave)
    //        {
    //            // Start despawning phase if not already in progress
    //            if (!isDespawning)
    //            {
    //                DespawnObjects();
    //                isDespawning = true; // Set despawning flag
    //                cooldownTimer = 5f; // Reset cooldown timer for waiting period
    //            }
    //            else
    //            {
    //                // Wait for the cooldown before spawning new items
    //                cooldownTimer -= Time.deltaTime;
    //                //Debug.Log("Cooldown Timer: " + cooldownTimer);
    //                if (cooldownTimer <= 0)
    //                {
    //                    // Spawn new items once cooldown is over
    //                    SpawnItems(westernObjects);
    //                    itemsSpawnedForWesternWave = true;
    //                    isDespawning = false; // Reset the despawning flag
    //                }
    //            }
    //        }
    //        // Wave 20
    //        if (WaveManager.currentWave == 20 && !itemsSpawnedForFarmWave)
    //        {
    //            // Start despawning phase if not already in progress
    //            if (!isDespawning)
    //            {
    //                DespawnObjects();
    //                isDespawning = true; // Set despawning flag
    //                cooldownTimer = 5f; // Reset cooldown timer for waiting period
    //            }
    //            else
    //            {
    //                // Wait for the cooldown before spawning new items
    //                cooldownTimer -= Time.deltaTime;
    //                //Debug.Log("Cooldown Timer: " + cooldownTimer);
    //                if (cooldownTimer <= 0)
    //                {
    //                    // Spawn new items once cooldown is over
    //                    SpawnItems(farmObjects);
    //                    itemsSpawnedForFarmWave = true;
    //                    isDespawning = false; // Reset the despawning flag
    //                }
    //            }
    //        }
    //        // Wave 25
    //        if (WaveManager.currentWave == 25 && !itemsSpawnedForModernDayWave)
    //        {
    //            // Start despawning phase if not already in progress
    //            if (!isDespawning)
    //            {
    //                DespawnObjects();
    //                isDespawning = true; // Set despawning flag
    //                cooldownTimer = 5f; // Reset cooldown timer for waiting period
    //            }
    //            else
    //            {
    //                // Wait for the cooldown before spawning new items
    //                cooldownTimer -= Time.deltaTime;
    //                //Debug.Log("Cooldown Timer: " + cooldownTimer);
    //                if (cooldownTimer <= 0)
    //                {
    //                    // Spawn new items once cooldown is over
    //                    SpawnItems(modernDayObjects);
    //                    itemsSpawnedForModernDayWave = true;
    //                    isDespawning = false; // Reset the despawning flag
    //                }
    //            }
    //        }
    //        // Wave 30
    //        if (WaveManager.currentWave == 30 && !itemsSpawnedForSciFiWave)
    //        {
    //            // Start despawning phase if not already in progress
    //            if (!isDespawning)
    //            {
    //                DespawnObjects();
    //                isDespawning = true; // Set despawning flag
    //                cooldownTimer = 5f; // Reset cooldown timer for waiting period
    //            }
    //            else
    //            {
    //                // Wait for the cooldown before spawning new items
    //                cooldownTimer -= Time.deltaTime;
    //                //Debug.Log("Cooldown Timer: " + cooldownTimer);
    //                if (cooldownTimer <= 0)
    //                {
    //                    // Spawn new items once cooldown is over
    //                    SpawnItems(scifiObjects);
    //                    itemsSpawnedForSciFiWave = true;
    //                    isDespawning = false; // Reset the despawning flag
    //                }
    //            }
    //        }
    //    }
    //}
}
