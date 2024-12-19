using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class itemAreaSpawner : MonoBehaviour
{
    #region VARIABLES

    // PUBLIC
    public GameObject[] romanObjects;
    public GameObject[] fantasyObjects;
    public GameObject[] pirateObjects;
    public GameObject[] westernObjects;
    public GameObject[] farmObjects;
    public GameObject[] modernDayObjects;
    public GameObject[] scifiObjects;
    public GameObject boulder;

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

    public LayerMask spawnedObjectLayer;
    public WaveManager waveManager;

    // PRIVATE
    private float cooldownTimer = 0f;

    private bool itemsSpawnedForRomanWave = false;
    private bool itemsSpawnedForFantasyWave = false;
    private bool itemsSpawnedForPirateWave = false;
    private bool itemsSpawnedForWesternWave = false;
    private bool itemsSpawnedForFarmWave = false;
    private bool itemsSpawnedForModernDayWave = false;
    private bool itemsSpawnedForSciFiWave = false;

    private bool boulderForRoman = false;
    private bool boulderForRoman2 = false;
    private bool boulderForFantasy = false;
    private bool boulderForPirate = false;
    private bool boulderForWestern = false;
    private bool boulderForFarm = false;
    private bool boulderForModern = false;
    private bool boulderForSciFi = false;

    private bool isDespawning = false;


    //-------------SAMS SCRIPTS-------------------------

    public GridGraphUpdater gridGraphUpdater;
    public float navMeshOffsetMultiplier = 1.2f; // Offset multiplier for NavMesh calculations

    #endregion

    #region UPDATE
    private void Update()
    {
        SpawnWithWaves();

        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    WaveManager.currentWave++;
        //    Debug.Log(WaveManager.currentWave);
        //}
    }
    #endregion

    #region SPAWN & SPREAD ITEMS
    private void SpawnItems(GameObject[] objectArray)
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

    //private bool SpreadItem(GameObject[] objectArray)
    //{
    //    int maxAttempts = 5;  // Maximum number of attempts to find a valid position
    //    int attempt = 0;

    //    if (attempt < maxAttempts)
    //    {
    //        attempt++;

    //        // Generate a random position within the spread
    //        Vector3 randPosition = new Vector3(
    //            Random.Range(-itemXSpread, itemXSpread),
    //            0f,  // Surface Y-position
    //            Random.Range(-itemZSpread, itemZSpread)) + transform.position;

    //        // Pick a random item from the selected object array
    //        int randomIndex = Random.Range(0, objectArray.Length);
    //        GameObject itemToSpread = objectArray[randomIndex];

    //        // Perform overlap check using an overlap box
    //        Vector3 overlapTestBoxScale = new Vector3(overlapTestBoxSize, overlapTestBoxSize, overlapTestBoxSize);
    //        Collider[] collidersInsideOverlapBox = new Collider[10];
    //        int numberOfCollidersFound = Physics.OverlapBoxNonAlloc(randPosition, overlapTestBoxScale, collidersInsideOverlapBox, Quaternion.identity, spawnedObjectLayer);

    //        // If no collisions, spawn the object
    //        if (numberOfCollidersFound == 0)
    //        {
    //            GameObject clone = Instantiate(itemToSpread, randPosition, Quaternion.identity);
    //            spawnedObjects.Add(clone);


    //            #region SAMS KOD
    //            //----------------SAMS KOD: UPDATETING PATHFINDING-------------------------------
    //            float updateRadius = 0;
    //            Collider collider = clone.GetComponent<Collider>();
    //            if (collider != null)
    //            {
    //                Bounds bounds = collider.bounds;
    //                updateRadius = (Mathf.Max(bounds.size.x, bounds.size.z) / 2.0f) + 1;
    //            }
    //            if (!clone.CompareTag("Decor"))
    //            {
    //                gridGraphUpdater.UpdateGrid(clone.transform.position, updateRadius);
    //            }
    //            #endregion

    //            return true;  // Successfully spawned an item
    //        }
    //    }
    //    return false;  // Failed to spawn due to overlap
    //}
    private bool SpreadItem(GameObject[] objectArray)
    {
        int maxAttempts = 5; // Maximum number of attempts to find a valid position
        int attempt = 0;

        if (attempt < maxAttempts)
        {
            attempt++;

            // Generate a random position within the spread
            Vector3 randPosition = new Vector3(
                Random.Range(-itemXSpread, itemXSpread),
                0f, // Surface Y-position
                Random.Range(-itemZSpread, itemZSpread)) + transform.position;

            // Pick a random item from the selected object array
            int randomIndex = Random.Range(0, objectArray.Length);
            GameObject itemToSpread = objectArray[randomIndex];

            // Perform overlap check using the overlap test box
            Vector3 overlapBoxScale = new Vector3(overlapTestBoxSize, overlapTestBoxSize, overlapTestBoxSize);
            Collider[] collidersInsideOverlapBox = new Collider[10]; // Predefined array to avoid GC
            int numberOfCollidersFound = Physics.OverlapBoxNonAlloc(
                randPosition,
                overlapBoxScale / 2f, // Divide by 2 because the box size is defined by extents
                collidersInsideOverlapBox,
                Quaternion.identity,
                spawnedObjectLayer
            );

            // If no collisions, spawn the object
            if (numberOfCollidersFound == 0)
            {
                // Instantiate the object
                GameObject clone = Instantiate(itemToSpread, randPosition, Quaternion.identity);
                spawnedObjects.Add(clone);

                // Apply rotation if required
                //ApplyRandomRotation(clone);

                #region NAVMESH UPDATE
                UpdatePathfinding(clone); // Update NavMesh with the multiplier applied
                #endregion

                return true; // Successfully spawned an item
            }
        }

        return false; // Failed to spawn due to overlap
    }
    #endregion

    #region DESPAWN OBJECTS
    private void DespawnObjects()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
            {
                TrapManager trap = obj.GetComponent<TrapManager>();
                TrampolineManager trampoline = obj.GetComponent<TrampolineManager>();
                ObjectShader objectShader = obj.GetComponent<ObjectShader>();
                BoulderManager boulderManager = obj.GetComponent<BoulderManager>();

                if (trap != null)
                {
                    objectShader.StartDespawn();
                    trap.DespawnTrap();
                }
                if (trampoline != null)
                {
                    trampoline.DespawnTrampoline();
                }
                if (boulderManager != null)
                {
                    //foreach (GameObject misc in boulderManager.objectsToRemove)
                    //{
                    //    Destroy(misc);
                    //    boulderManager.objectsToRemove.Remove(misc);
                    //}
                    Destroy(obj);
                }
                else
                {
                    //obj.AddComponent<MoveDownwards>();
                    objectShader.StartDespawn();
                }

            }

            #region SAMS KOD
            //----------------SAMS KOD: UPDATETING PATHFINDING-------------------------------
            float updateRadius = 0;
            Collider collider = obj.GetComponent<Collider>();
            if (collider != null)
            {
                Bounds bounds = collider.bounds;
                updateRadius = (Mathf.Max(bounds.size.x, bounds.size.z) / 2.0f) + 1;
            }
            if (!obj.CompareTag("Decor"))
            {
                gridGraphUpdater.RemoveObstacleUpdate(obj.transform.position, updateRadius);
            }
            #endregion
        }
        spawnedObjects.Clear();
    }
    #endregion

    #region WAVE LOGIC
    private void HandleWave(int waveNumber, GameObject[] objectsToSpawn, ref bool itemsSpawnedFlag)
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
    private void SpawnWithWaves()
    {
        if (waveManager != null)
        {
            //// Wave 1 - Spawn Roman objects
            //if (WaveManager.currentWave == 1 && !itemsSpawnedForRomanWave)
            //{
            //    SpawnItems(romanObjects);
            //    StartCoroutine(SpawnBoulderWithDelay());
            //    itemsSpawnedForRomanWave = true;
            //}

            HandleBoulders();

            // For each wave with despawning and cooldown before spawning new items
            HandleWave(5, fantasyObjects, ref itemsSpawnedForFantasyWave);
            HandleWave(10, pirateObjects, ref itemsSpawnedForPirateWave);
            HandleWave(15, westernObjects, ref itemsSpawnedForWesternWave);
            HandleWave(20, farmObjects, ref itemsSpawnedForFarmWave);
            HandleWave(25, modernDayObjects, ref itemsSpawnedForModernDayWave);
            HandleWave(30, scifiObjects, ref itemsSpawnedForSciFiWave);
        }
    }
    public void SpawnRomanObjects()
    {
        // Wave 1 - Spawn Roman objects
        if (WaveManager.currentWave == 0 && !itemsSpawnedForRomanWave)
        {
            SpawnItems(romanObjects);
            StartCoroutine(SpawnBoulderWithDelay());
            itemsSpawnedForRomanWave = true;
        }
    }
    #endregion

    #region BOULDER LOGIC
    private void HandleBoulders()
    {
        if (WaveManager.currentWave == 5 && !boulderForRoman)
        {
            StartCoroutine(SpawnBoulderWithDelay());
            boulderForRoman = true;
        }
        if (WaveManager.currentWave == 6 && !boulderForRoman2)
        {
            StartCoroutine(SpawnBoulderWithDelay());
            StartCoroutine(SpawnBoulderWithDelay());
            boulderForRoman2 = true;
        }
        if (WaveManager.currentWave == 11 && !boulderForFantasy)
        {
            StartCoroutine(SpawnBoulderWithDelay());
            boulderForFantasy = true;
        }
        if (WaveManager.currentWave == 21 && !boulderForPirate)
        {
            StartCoroutine(SpawnBoulderWithDelay());
            boulderForPirate = true;
        }
        if (WaveManager.currentWave == 32 && !boulderForWestern)
        {
            StartCoroutine(SpawnBoulderWithDelay());
            boulderForWestern = true;
        }
        if (WaveManager.currentWave == 43 && !boulderForFarm)
        {
            StartCoroutine(SpawnBoulderWithDelay());
            boulderForFarm = true;
        }
        if (WaveManager.currentWave == 54 && !boulderForModern)
        {
            StartCoroutine(SpawnBoulderWithDelay());
            boulderForModern = true;
        }
        if (WaveManager.currentWave == 60 && !boulderForSciFi)
        {
            StartCoroutine(SpawnBoulderWithDelay());
            boulderForSciFi = true;
        }
    }

    public IEnumerator SpawnBoulderWithDelay()
    {
        // Generate a random delay between 10 and 20 seconds
        float delay = Random.Range(10f, 20f);
        //Debug.Log($"Spawning boulder in {delay} seconds.");
        yield return new WaitForSeconds(delay);

        // Define spawn positions and their corresponding opposite target positions
        Vector3[] spawnPositions = {
        new Vector3(0, 0.5f, 45),    // Top-center
        new Vector3(45, 0.5f, 0),    // Right-center
        new Vector3(0, 0.5f, -45),   // Bottom-center
        new Vector3(-45, 0.5f, 0),   // Left-center
        new Vector3(-35, 0.5f, -35), // New diagonal spawn (bottom-left)
        new Vector3(-35, 0.5f, 35)   // New diagonal spawn (top-left)
    };

        Vector3[] targetPositions = {
        new Vector3(0, 0.5f, -45),   // Opposite of Top-center
        new Vector3(-45, 0.5f, 0),   // Opposite of Right-center
        new Vector3(0, 0.5f, 45),    // Opposite of Bottom-center
        new Vector3(45, 0.5f, 0),    // Opposite of Left-center
        new Vector3(35, 0.5f, 35),   // New diagonal target (top-right)
        new Vector3(35, 0.5f, -35)   // New diagonal target (bottom-right)
    };

        // Randomly pick one of the spawn positions
        int index = Random.Range(0, spawnPositions.Length);
        Vector3 spawnPosition = spawnPositions[index];
        Vector3 targetPosition = targetPositions[index];

        // Instantiate the boulder at the determined spawn position
        GameObject boulderInstance = Instantiate(boulder, spawnPosition, Quaternion.identity);
        spawnedObjects.Add(boulderInstance);

        // Set the boulder's movement direction in the BoulderManager
        BoulderManager boulderManager = boulderInstance.GetComponent<BoulderManager>();
        if (boulderManager != null)
        {
            boulderManager.SetMovementDirection((targetPosition - spawnPosition).normalized);
            boulderManager.spawner = this; // Set the reference to the spawner
            Debug.Log($"Boulder spawned at: {spawnPosition}, moving towards: {targetPosition}");
        }
        else
        {
            Debug.LogError("BoulderManager script not found on boulder prefab.");
        }
    }
    public void RemoveObjectFromCollision(GameObject obj)
    {
        if (spawnedObjects.Contains(obj))
        {
            spawnedObjects.Remove(obj);
            Debug.Log($"Removed {obj.name} from spawnedObjects list.");
        }
    }
    #endregion

    #region PATHFINDING

    private void UpdatePathfinding(GameObject obj)
    {
        float updateRadius = CalculateUpdateRadius(obj);

        if (!obj.CompareTag("Decor"))
        {
            gridGraphUpdater.UpdateGrid(obj.transform.position, updateRadius);
        }
    }

    private float CalculateUpdateRadius(GameObject obj)
    {
        Vector3 worldSize = GetObjectWorldSize(obj) * navMeshOffsetMultiplier; // Apply the multiplier
        return Mathf.Max(worldSize.x, worldSize.z) / 2f + 1f; // Max dimension + buffer
    }

    private Vector3 GetObjectWorldSize(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            return renderer.bounds.size; // World size (x, y, z)
        }

        Collider collider = obj.GetComponent<Collider>();
        if (collider != null)
        {
            return collider.bounds.size; // Use collider size if Renderer is missing
        }

        Debug.LogWarning($"Object {obj.name} does not have a Renderer or Collider for size calculations!");
        return Vector3.one; // Default size
    }

    #endregion
}
