using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnemySpawnerPGA : MonoBehaviour
{
    [Header("UI Elements")]
    public Canvas spawnMenuCanvas;
    public TextMeshProUGUI interactionText;
    public List<Button> enemyButtons;

    [Header("Spawn Settings")]
    public List<GameObject> enemyPrefabs;
    public List<Transform> spawnPoints;
    public float spawnCooldown = 2f;
    public float interactionRadius = 5f;

    private bool isPlayerInRange = false;
    private float lastSpawnTime = -2f;
    private GameObject player;

    void Start()
    {
        // Find player object by tag
        player = GameObject.FindGameObjectWithTag("Player1");

        // Hide UI elements initially
        interactionText.gameObject.SetActive(false);
        spawnMenuCanvas.gameObject.SetActive(false);

        // Assign each button to spawn the respective enemy prefab
        for (int i = 0; i < enemyButtons.Count; i++)
        {
            if (i < enemyPrefabs.Count)
            {
                int index = i; // Capture the current index for the lambda expression
                enemyButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = $"Spawn {enemyPrefabs[index].name}";
                enemyButtons[i].onClick.RemoveAllListeners(); // Clear any previous listeners to avoid duplication
                enemyButtons[i].onClick.AddListener(() => OnSpawnButtonClicked(enemyPrefabs[index]));
                enemyButtons[i].interactable = true; // Ensure button is interactable
                enemyButtons[i].gameObject.SetActive(true);
            }
            else
            {
                enemyButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        // Check player proximity
        if (player != null)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance <= interactionRadius)
            {
                if (!isPlayerInRange)
                {
                    interactionText.gameObject.SetActive(true);
                    isPlayerInRange = true;
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    spawnMenuCanvas.gameObject.SetActive(!spawnMenuCanvas.gameObject.activeSelf);
                }
            }
            else
            {
                if (isPlayerInRange)
                {
                    interactionText.gameObject.SetActive(false);
                    spawnMenuCanvas.gameObject.SetActive(false);
                    isPlayerInRange = false;
                }
            }
        }
    }

    void OnSpawnButtonClicked(GameObject enemyPrefab)
    {
        if (Time.time - lastSpawnTime >= spawnCooldown)
        {
            SpawnEnemy(enemyPrefab);
        }
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        if (spawnPoints.Count > 0)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            if (spawnedEnemy == null)
            {
                Debug.LogError("Failed to instantiate enemy prefab: " + enemyPrefab.name);
            }
            lastSpawnTime = Time.time;
        }
        else
        {
            Debug.LogWarning("No spawn points assigned for enemy spawning.");
        }
    }
}
