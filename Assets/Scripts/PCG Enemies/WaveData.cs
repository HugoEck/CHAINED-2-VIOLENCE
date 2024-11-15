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
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Roman, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 5 }
            }
        ));

        waves.Add(CreateWave("Wave 2",
            new List<EnemyConfig>
            {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Roman, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 10 }
            }
        ));

        waves.Add(CreateWave("Wave 3",
            new List<EnemyConfig>
            {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Roman, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 10 }
            }
        ));

        waves.Add(CreateWave("Wave 4",
            new List<EnemyConfig>
            {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Roman, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 15 }
            }
        ));
        waves.Add(CreateWave("Wave 5",
            new List<EnemyConfig>
            {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Roman, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 20 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Roman, enemyClass = NPC_Customization.NPCClass.Warrior, waveSize = 1 }
            }
        ));
        waves.Add(CreateWave("Wave 6",
                new List<EnemyConfig>
                {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Roman, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 25 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Roman, enemyClass = NPC_Customization.NPCClass.Warrior, waveSize = 3 }
                }
            ));
        waves.Add(CreateWave("Wave 7",
                new List<EnemyConfig>
                {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Roman, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 30 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Roman, enemyClass = NPC_Customization.NPCClass.Warrior, waveSize = 3 }
                }
            ));
        waves.Add(CreateWave("Wave 8",
                new List<EnemyConfig>
                {
               new EnemyConfig { theme = NPC_Customization.NPCTheme.Roman, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 30 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Roman, enemyClass = NPC_Customization.NPCClass.Warrior, waveSize = 3 }
                }
            ));
        waves.Add(CreateWave("Wave 9",
                new List<EnemyConfig>
                {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Roman, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 60 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Roman, enemyClass = NPC_Customization.NPCClass.Warrior, waveSize = 6 }
                }
            ));
        waves.Add(CreateWave("Wave 10",
                new List<EnemyConfig>
                {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Fantasy, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 60 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Fantasy, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 7 }
                }
            ));
        waves.Add(CreateWave("Wave 11",
            new List<EnemyConfig>
            {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Pirate, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 70 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Pirate, enemyClass = NPC_Customization.NPCClass.RockThrower, waveSize = 10 }
            }
        ));
        waves.Add(CreateWave("Wave 12",
            new List<EnemyConfig>
            {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Pirate, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 100 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Pirate, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 10 }
            }
        ));
        waves.Add(CreateWave("Wave 13",
            new List<EnemyConfig>
            {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Pirate, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 100 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Pirate, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 10 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Pirate, enemyClass = NPC_Customization.NPCClass.RockThrower, waveSize = 10 }
            }
        ));
        waves.Add(CreateWave("Wave 14",
            new List<EnemyConfig>
            {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Pirate, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 100 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Pirate, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 10 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Pirate, enemyClass = NPC_Customization.NPCClass.RockThrower, waveSize = 15 }
            }
        ));
        waves.Add(CreateWave("Wave 15",
            new List<EnemyConfig>
            {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Cowboys, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 50 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Indians, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 50 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Cowboys, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 10 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Indians, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 10 }
            }
        ));
        waves.Add(CreateWave("Wave 16",
            new List<EnemyConfig>
            {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Cowboys, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 20 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Cowboys, enemyClass = NPC_Customization.NPCClass.Charger, waveSize = 1 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Indians, enemyClass = NPC_Customization.NPCClass.Charger, waveSize = 1 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Indians, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 20 }
            }
        ));
        waves.Add(CreateWave("Wave 17",
            new List<EnemyConfig>
            {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Cowboys, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 50 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Cowboys, enemyClass = NPC_Customization.NPCClass.Charger, waveSize = 1 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Indians, enemyClass = NPC_Customization.NPCClass.Charger, waveSize = 1 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Indians, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 20 }
            }
        ));
        waves.Add(CreateWave("Wave 18",
            new List<EnemyConfig>
            {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Cowboys, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 50 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Cowboys, enemyClass = NPC_Customization.NPCClass.Charger, waveSize = 1 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Indians, enemyClass = NPC_Customization.NPCClass.Charger, waveSize = 1 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Indians, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 20 }
            }
        ));
        waves.Add(CreateWave("Wave 19",
            new List<EnemyConfig>
            {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Cowboys, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 50 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Indians, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 50 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Cowboys, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 10 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Indians, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 10 }
            }
        ));
        waves.Add(CreateWave("Wave 20",
           new List<EnemyConfig>
           {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Farm, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 50 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Farm, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 10 }
           }
       ));
        waves.Add(CreateWave("Wave 21",
           new List<EnemyConfig>
           {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Farm, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 70 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Farm, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 10 }
           }
       ));
        waves.Add(CreateWave("Wave 22",
           new List<EnemyConfig>
           {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Farm, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 70 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Farm, enemyClass = NPC_Customization.NPCClass.RockThrower, waveSize = 10 }
           }
       ));
        waves.Add(CreateWave("Wave 23",
           new List<EnemyConfig>
           {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Farm, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 70 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Farm, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 10 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Farm, enemyClass = NPC_Customization.NPCClass.RockThrower, waveSize = 10 }
           }
       ));
        waves.Add(CreateWave("Wave 24",
           new List<EnemyConfig>
           {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Farm, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 100 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Farm, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 10 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Farm, enemyClass = NPC_Customization.NPCClass.Charger, waveSize = 1 }
           }
       ));
        waves.Add(CreateWave("Wave 25",
           new List<EnemyConfig>
           {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.CurrentDay, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 50 }
           }
       ));
        waves.Add(CreateWave("Wave 26",
           new List<EnemyConfig>
           {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.CurrentDay, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 50 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.CurrentDay, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 20 }
           }
       ));
        waves.Add(CreateWave("Wave 27",
           new List<EnemyConfig>
           {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.CurrentDay, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 50 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.CurrentDay, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 25 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.CurrentDay, enemyClass = NPC_Customization.NPCClass.RockThrower, waveSize = 10 }
           }
       ));
        waves.Add(CreateWave("Wave 28",
           new List<EnemyConfig>
           {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.CurrentDay, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 50 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.CurrentDay, enemyClass = NPC_Customization.NPCClass.Charger, waveSize = 2 }
           }
       ));
        waves.Add(CreateWave("Wave 29",
           new List<EnemyConfig>
           {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.CurrentDay, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 50 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.CurrentDay, enemyClass = NPC_Customization.NPCClass.Charger, waveSize = 2 }
           }
       ));
        waves.Add(CreateWave("Wave 31",
           new List<EnemyConfig>
           {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.SciFi, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 150 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.SciFi, enemyClass = NPC_Customization.NPCClass.Charger, waveSize = 1 }
           }
       ));
        waves.Add(CreateWave("Wave 32",
           new List<EnemyConfig>
           {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.SciFi, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 50 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.SciFi, enemyClass = NPC_Customization.NPCClass.Charger, waveSize = 2 }
           }
       ));
        waves.Add(CreateWave("Wave 33",
           new List<EnemyConfig>
           {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.SciFi, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 150 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.SciFi, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 50 }

           }
       ));
        waves.Add(CreateWave("Wave 34",
           new List<EnemyConfig>
           {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.SciFi, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 150 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.SciFi, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 50 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.SciFi, enemyClass = NPC_Customization.NPCClass.RockThrower, waveSize = 50 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.SciFi, enemyClass = NPC_Customization.NPCClass.Charger, waveSize = 2 }
           }
       ));
        waves.Add(CreateWave("Wave 35",
           new List<EnemyConfig>
           {
               //SPAWN BOSS HERE
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
