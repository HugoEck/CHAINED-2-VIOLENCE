using System.Collections;
using System.Collections.Generic;
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

    // The spread of objects around the map
    public float itemXSpread;
    public float itemYSpread;
    public float itemZSpread;

    // Spawnpoint, below the ground
    private float spawnpointY = -15f;

    // Timer to make sure the right amount of objects are spawned, so you can't spam objects
    private float cooldownTime = 3f;
    private float cooldownTimer = 0f;

    // Layer that is to be checked for overlaps
    public LayerMask spawnedObjectLayer;

    // Size of the box for overlap check
    public float overlapTestBoxSize = 1f;
    #endregion

    private void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Space) && cooldownTimer <= 0f)
        {
            SpawnItems();
            cooldownTimer = cooldownTime;

        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            DespawnObjects();
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

        // If all attempts failed, return false
        Debug.Log("Failed to find a valid spawn position after " + maxAttempts + " attempts.");
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
}
