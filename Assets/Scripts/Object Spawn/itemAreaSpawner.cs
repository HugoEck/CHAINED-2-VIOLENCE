using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class itemAreaSpawner : MonoBehaviour
{
    #region Variables
    // Array of objects that can be spawned
    public GameObject[] itemsToPickFrom;

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

    private bool itemsSpawnedForWave1 = false;
    private bool itemsSpawnedForWave6 = false;
    private bool itemsSpawnedForWave11 = false;
    private bool isDespawning = false;

    #endregion

    private void Update()
    {
        SpawnWithWaves();
        
        if (Input.GetKeyDown(KeyCode.N))
        {
            //WaveManager.currentWave++;
        }
    }

    void SpawnItems()
    {
        int successfullySpawned = 0;

        for (int i = 0; i < numObjectsToSpawn; i++)
        {
            if (SpreadItem())  // Only increment if an item was successfully spawned
            {
                successfullySpawned++;
            }

            if (successfullySpawned >= numObjectsToSpawn)
            {
                break;  // Stop when the desired number of objects is spawned
            }
        }
    }

    bool SpreadItem()
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

            // Pick a random item from the array
            int randomIndex = Random.Range(0, itemsToPickFrom.Length);
            GameObject itemToSpread = itemsToPickFrom[randomIndex];

            // Perform overlap check using an overlap box
            Vector3 overlapTestBoxScale = new Vector3(overlapTestBoxSize, overlapTestBoxSize, overlapTestBoxSize);
            Collider[] collidersInsideOverlapBox = new Collider[10];
            int numberOfCollidersFound = Physics.OverlapBoxNonAlloc(randPosition, overlapTestBoxScale, collidersInsideOverlapBox, Quaternion.identity, spawnedObjectLayer);

            // If no collisions, spawn the object
            if (numberOfCollidersFound == 0)
            {
                GameObject clone = Instantiate(itemToSpread, randPosition, Quaternion.identity);
                spawnedObjects.Add(clone);

                #region Spawn UnderGround
                // clone.AddComponent<MoveUpwards>();  // If needed for objects to move up
                #endregion

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
    private void SpawnWithWaves()
    {
        if (waveManager != null)
        {
            // Wave 1 - Spawn items directly
            if (WaveManager.currentWave == 1 && !itemsSpawnedForWave1)
            {
                SpawnItems();
                itemsSpawnedForWave1 = true;
            }

            // Wave 6 - Despawn, wait for the cooldown, then spawn new items
            if (WaveManager.currentWave == 6 && !itemsSpawnedForWave6)
            {
                // Start despawning phase if not already in progress
                if (!isDespawning)
                {
                    DespawnObjects();
                    isDespawning = true; // Set despawning flag
                    cooldownTimer = 5f; // Reset cooldown timer for waiting period
                }
                else
                {
                    // Wait for the cooldown before spawning new items
                    cooldownTimer -= Time.deltaTime;
                    //Debug.Log("Cooldown Timer: " + cooldownTimer);
                    if (cooldownTimer <= 0)
                    {
                        // Spawn new items once cooldown is over
                        SpawnItems();
                        itemsSpawnedForWave6 = true; // Mark items for wave 6 as spawned
                        isDespawning = false; // Reset the despawning flag
                    }
                }
            }

            // Wave 11 - Same logic as wave 6
            if (WaveManager.currentWave == 11 && !itemsSpawnedForWave11)
            {
                // Start despawning phase if not already in progress
                if (!isDespawning)
                {
                    DespawnObjects();
                    isDespawning = true; // Set despawning flag
                    cooldownTimer = 5f; // Reset cooldown timer for waiting period
                }
                else
                {
                    // Wait for the cooldown before spawning new items
                    cooldownTimer -= Time.deltaTime;
                    //Debug.Log("Cooldown Timer: " + cooldownTimer);
                    if (cooldownTimer <= 0)
                    {
                        // Spawn new items once cooldown is over
                        SpawnItems();
                        itemsSpawnedForWave11 = true; // Mark items for wave 11 as spawned
                        isDespawning = false; // Reset the despawning flag
                    }
                }
            }
        }
    }
}
