using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class WaveManager : MonoBehaviour
{
    [Header("Fonts")]
    public TMP_Asset romanFont;
    public TMP_Asset scifiFont;
    public TMP_Asset farmFont;
    public TMP_Asset fantasyFont;
    public TMP_Asset westernFont;
    public TMP_Asset pirateFont;
    public TMP_Asset currentDayFont;


    [Header("Enemy stuff")]
    [SerializeField] GameObject enemyCreatorObject;
    NPC_Customization enemyCreator;
    public static int ActiveEnemies = 0;

    private float targetTime = 50;
    private float timer = 0;

    [SerializeField] List<GameObject> spawnPoints = new List<GameObject>();
    List<Wave> waves = new List<Wave>();
    WaveData waveData = new WaveData();

    [SerializeField] TextMeshProUGUI text;
    public static int currentWave = 0;
    private int previousWave = -1;

    [Header("Items")]
    public ItemPicker itemPicker;

    public enum CurrentEra
    {
        Roman,
        Fantasy,
        Pirate,
        Western,
        Farm,
        CurrentDay,
        SciFi
    }
    public CurrentEra currentEra = new CurrentEra();



    public float deltaTime;
    private void OnEnable()
    {
        ActiveEnemies = 0;
        currentWave = 0;
    }



    private void Start()
    {
        enemyCreator = enemyCreatorObject.GetComponent<NPC_Customization>();
        waveData.LoadWaves(waves);
        //StartCoroutine(SpawnWavesRegularly());
    }


    private void Update()
    {
        timer += Time.deltaTime;
        if (ActiveEnemies == 0 /*|| timer > targetTime*/)
        {
            timer = 0;
            //ChangeEra();
            //SpawnWave(waves[currentWave]);

            itemPicker.ActivateItems();


            if (currentWave != previousWave)
            {
                // Update the previous wave tracker
                previousWave = currentWave;

                // Change the era based on the wave
                ChangeEra();

                // Spawn the wave
                SpawnWave(waves[currentWave]);
            }
                //currentWave++;
            }

            //Debug spawner
            if (Input.GetKeyDown(KeyCode.L))
        {
            ChangeEra();
            SpawnWave(waves[currentWave]);
            currentWave++;
            timer = 0;
        }

        //FPS writer
        //deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        //float fps = 1.0f / deltaTime;
        //text.text = Mathf.Ceil(fps).ToString();
    }

    public void SpawnWave(Wave wave)
    {
        text.text = "Wave " + currentWave;
        StartCoroutine(FadeInText(1, 3));
        StartCoroutine(SpawnWaveCoroutine(wave));
    }

    private IEnumerator SpawnWaveCoroutine(Wave wave)
    {
        GameObject waveParent = new GameObject($"{wave.waveName} Army");

        int totalEnemies = 0;

        foreach (var enemyConfig in wave.enemyConfigs)
        {
            totalEnemies += enemyConfig.waveSize;
        }

        float spawnInterval = Mathf.Max(1.0f / (totalEnemies / 30f), 0.1f); // Ensure there's at least 0.1s between spawns


        foreach (var enemyConfig in wave.enemyConfigs)
        {
            // Randomize and create the base enemy
            enemyCreator.Randomize(enemyConfig.theme, enemyConfig.enemyClass);
            GameObject randomEnemy = Instantiate(enemyCreator.currentBody);
            //randomEnemy.transform.parent = waveParent.transform; // Set the parent for the base enemy
            randomEnemy.transform.position = new Vector3(100, 0, 100);
            for (int i = 0; i < enemyConfig.waveSize; i++)
            {
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)].transform;
                enemyCreator.Randomize(enemyConfig.theme, enemyConfig.enemyClass); // Apply to each enemy
                GameObject newEnemy = Instantiate(enemyCreator.currentBody, spawnPoint.position, spawnPoint.rotation);
                newEnemy.name = $"{enemyConfig.enemyClass} enemy {i + 1}";
                newEnemy.transform.parent = waveParent.transform;

                enemyCreator.AddBehaviourToClass(newEnemy);
                ActiveEnemies++;

                yield return new WaitForSeconds(spawnInterval);
            }
        }


        yield return null;
    }

    private IEnumerator FadeInText(float fadeDuration, float stayDuration)
    {
        Color color = text.color;
        color.a = 0; // Start with fully transparent
        text.color = color;

        text.font = (TMP_FontAsset)GetFont();

        // Gradually increase alpha to 1
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            color.a = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            text.color = color;
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the final alpha is set to 1
        color.a = 1;
        text.color = color;
        elapsedTime = 0;

        while (elapsedTime < stayDuration)
        {
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            color.a = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            text.color = color;
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the final alpha is set to 1
        color.a = 0;
        text.color = color;
    }

    private void ChangeEra()
    {
        if (waves[currentWave].enemyConfigs[0].theme == NPC_Customization.NPCTheme.Roman)
        {
            currentEra = CurrentEra.Roman;
        }
        else if (waves[currentWave].enemyConfigs[0].theme == NPC_Customization.NPCTheme.SciFi)
        {
            currentEra = CurrentEra.SciFi;
        }
        else if (waves[currentWave].enemyConfigs[0].theme == NPC_Customization.NPCTheme.Farm)
        {
            currentEra = CurrentEra.Farm;
        }
        else if (waves[currentWave].enemyConfigs[0].theme == NPC_Customization.NPCTheme.Fantasy)
        {
            currentEra = CurrentEra.Fantasy;
        }
        else if (waves[currentWave].enemyConfigs[0].theme == NPC_Customization.NPCTheme.Cowboys)
        {
            currentEra = CurrentEra.Western;
        }
        else if (waves[currentWave].enemyConfigs[0].theme == NPC_Customization.NPCTheme.Pirate)
        {
            currentEra = CurrentEra.Pirate;
        }
        else if (waves[currentWave].enemyConfigs[0].theme == NPC_Customization.NPCTheme.CurrentDay)
        {
            currentEra = CurrentEra.CurrentDay;
        }

    }

    private TMP_Asset GetFont()
    {
        Debug.Log(waves[currentWave].waveName);

        if (waves[currentWave].enemyConfigs[0].theme == NPC_Customization.NPCTheme.Roman)
        {
            return romanFont;
        }
        else if (waves[currentWave].enemyConfigs[0].theme == NPC_Customization.NPCTheme.SciFi)
        {
            return scifiFont;
        }
        else if (waves[currentWave].enemyConfigs[0].theme == NPC_Customization.NPCTheme.Farm)
        {
            return farmFont;
        }
        else if (waves[currentWave].enemyConfigs[0].theme == NPC_Customization.NPCTheme.Fantasy)
        {
            return fantasyFont;
        }
        else if (waves[currentWave].enemyConfigs[0].theme == NPC_Customization.NPCTheme.Cowboys)
        {
            return westernFont;
        }
        else if (waves[currentWave].enemyConfigs[0].theme == NPC_Customization.NPCTheme.Pirate)
        {
            return pirateFont;
        }
        else if (waves[currentWave].enemyConfigs[0].theme == NPC_Customization.NPCTheme.CurrentDay)
        {
            return currentDayFont;
        }
        return romanFont;
    }





}

