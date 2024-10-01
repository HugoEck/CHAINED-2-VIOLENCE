using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class WaveManager : MonoBehaviour
{
    [SerializeField] GameObject enemyCreatorObject;
    NPC_Customization enemyCreater;

    [SerializeField] List<GameObject> spawnPoints = new List<GameObject>();
    List<Wave> waves = new List<Wave>();
    WaveData waveData = new WaveData();

    [SerializeField] TextMeshProUGUI text;
    public int currentWave = 0;

    private void Awake()
    {
        enemyCreater = enemyCreatorObject.GetComponent<NPC_Customization>();
    }



    private void Start()
    {
        waveData.LoadWaves(waves);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currentWave < waves.Count)
        {
            
                SpawnWave(waves[currentWave]);
            
            text.text = "Current wave is: " + currentWave;
            currentWave++;

        }
    }

    public void SpawnWave(Wave wave)
    {
        StartCoroutine(SpawnWaveCoroutine(wave));
    }

    private IEnumerator SpawnWaveCoroutine(Wave wave)
    {
        GameObject waveParent = new GameObject($"{wave.waveName} Army");

        foreach (var enemyConfig in wave.enemyConfigs)
        {
            // Randomize and create the base enemy
            enemyCreater.Randomize(enemyConfig.theme, enemyConfig.enemyClass);
            GameObject randomEnemy = Instantiate(enemyCreater.currentBody);
            randomEnemy.transform.parent = waveParent.transform; // Set the parent for the base enemy

            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)].transform;

            for (int i = 0; i < enemyConfig.waveSize; i++)
            {
                // Instantiate a new enemy
                GameObject newEnemy = Instantiate(randomEnemy, spawnPoint.position, spawnPoint.rotation);
                newEnemy.name = $"{enemyConfig.enemyClass} enemy {i + 1}";
                newEnemy.transform.parent = waveParent.transform; // Set the parent for the new enemy

                // Add behavior to the new enemy
                enemyCreater.AddBehaviourToClass(newEnemy);

                // Optionally yield return null to spread creation over frames
                yield return null; // This will wait for one frame before continuing the loop
            }
        }
    }





}

