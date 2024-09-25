using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] GameObject enemyCreatorObject;
    NPC_Customization enemyCreater;

    [SerializeField] List<GameObject> spawnPoints = new List<GameObject>();


    int waveSize = 0;

    private void Awake()
    {
        enemyCreater = enemyCreatorObject.GetComponent<NPC_Customization>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateArmy(50, NPC_Customization.NPCTheme.Roman, NPC_Customization.NPCClass.Basic);
        }
    }

    public void CreateArmy(int waveSize, NPC_Customization.NPCTheme theme, NPC_Customization.NPCClass enemyClass)
    {
        enemyCreater.Randomize(theme, enemyClass);
        //GameObject randomEnemy = enemyCreatorObject.transform.Find("Current Body").gameObject;
        GameObject randomEnemy = Instantiate(enemyCreater.currentBody);
        randomEnemy.transform.parent = null;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)].transform;
        for (int i = 0; i < waveSize; i++)
        {
            GameObject newEnemy = Instantiate(randomEnemy, spawnPoint.position, spawnPoint.rotation);
            newEnemy.name = enemyClass.ToString() + " enemy" + i;
            newEnemy.transform.parent = null;
            enemyCreater.AddBehaviourToClass(newEnemy);
        }
    }



}
