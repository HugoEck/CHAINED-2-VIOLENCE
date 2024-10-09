using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class WaveManager : MonoBehaviour
{
    

    [SerializeField] GameObject enemyCreatorObject;
    NPC_Customization enemyCreator;

    [SerializeField] GameObject spawnPortal;

    [SerializeField] List<GameObject> spawnPoints = new List<GameObject>();
    List<Wave> waves = new List<Wave>();
    WaveData waveData = new WaveData();

    [SerializeField] TextMeshProUGUI text;
    public int currentWave = 0;


    public float deltaTime;
    private void Awake()
    {
        enemyCreator = enemyCreatorObject.GetComponent<NPC_Customization>();
    }



    private void Start()
    {
        waveData.LoadWaves(waves);
        StartCoroutine(SpawnWavesRegularly());
    }

    private IEnumerator SpawnWavesRegularly()
    {
        while (true)
        {
            if (currentWave < waves.Count)
            {
                SpawnWave(waves[currentWave]);
                text.text = "Current wave is: " + currentWave;
                currentWave++;
            }
            yield return new WaitForSeconds(20f); // Wait for 60 seconds before spawning the next wave
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) && currentWave < waves.Count)
        {
            SpawnWave(waves[currentWave]);
            currentWave++;
            text.text = "Current wave is: " + currentWave;
        }

        //Debug Wave
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Wave wave = new Wave();
        //    wave.waveName = "testWave";
        //    EnemyConfig enemy = new EnemyConfig();

        //    //Set enemy theme and class to debug here
        //    enemy.enemyClass = NPC_Customization.NPCClass.RockThrower;
        //    enemy.theme = NPC_Customization.NPCTheme.Roman;

        //    enemy.waveSize = 10;
        //    wave.enemyConfigs = new List<EnemyConfig>();
        //    wave.enemyConfigs.Add(enemy);
        //    SpawnWave(wave);
        //    text.text = "Trying out " + enemy.theme.ToString() + " "+ enemy.enemyClass.ToString();
        //}

        //FPS writer
        //deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        //float fps = 1.0f / deltaTime;
        //text.text = Mathf.Ceil(fps).ToString();
    }

    public void SpawnWave(Wave wave)
    {
        StartCoroutine(SpawnWaveCoroutine(wave));
    }

    private IEnumerator SpawnWaveCoroutine(Wave wave)
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)].transform;
        GameObject waveParent = new GameObject($"{wave.waveName} Army");

        waveParent.transform.position = spawnPoint.position; 
        GameObject portal = Instantiate(spawnPortal, waveParent.transform);

        foreach (var enemyConfig in wave.enemyConfigs)
        {
            // Randomize and create the base enemy
            enemyCreator.Randomize(enemyConfig.theme, enemyConfig.enemyClass);
            GameObject randomEnemy = Instantiate(enemyCreator.currentBody);
            //randomEnemy.transform.parent = waveParent.transform; // Set the parent for the base enemy
            randomEnemy.transform.position = new Vector3(100, 0, 100);


            for (int i = 0; i < enemyConfig.waveSize; i++)
            {
                // Instantiate a new enemy
                GameObject newEnemy = Instantiate(randomEnemy, spawnPoint.position, spawnPoint.rotation);
                newEnemy.name = $"{enemyConfig.enemyClass} enemy {i + 1}";
                newEnemy.transform.parent = waveParent.transform; // Set the parent for the new enemy

                // Add behavior to the new enemy
                enemyCreator.AddBehaviourToClass(newEnemy);

                // Optionally yield return null to spread creation over frames
                yield return null; // This will wait for one frame before continuing the loop
            }

        }
        yield return new WaitForSeconds(3.0f);
            Destroy(portal);
    }





}

