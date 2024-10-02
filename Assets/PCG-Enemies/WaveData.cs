using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveData : MonoBehaviour
{
    public void LoadWaves(List<Wave> waves)
    {
        waves.Add(CreateWave("Wave 1",
            new List<EnemyConfig>
            {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Roman, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 10 }
            }
        ));

        waves.Add(CreateWave("Wave 2",
            new List<EnemyConfig>
            {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Roman, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 5 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Roman, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 2 }
            }
        ));

        waves.Add(CreateWave("Wave 3",
            new List<EnemyConfig>
            {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Roman, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 1 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Roman, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 1 }
            }
        ));

        waves.Add(CreateWave("Wave 4",
            new List<EnemyConfig>
            {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Roman, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 10 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Roman, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 2 }
            }
        ));
    }

    public Wave CreateWave(string waveName, List<EnemyConfig> enemyConfigs)
    {
        Wave newWave = new Wave
        {
            waveName = waveName,
            enemyConfigs = enemyConfigs
        };
        return newWave;
    }
}


[System.Serializable]
public class EnemyConfig
{
    public NPC_Customization.NPCTheme theme;        
    public NPC_Customization.NPCClass enemyClass;   
    public int waveSize;                             
}

[System.Serializable]
public class Wave
{
    public string waveName;              
    public List<EnemyConfig> enemyConfigs; 
    
}
