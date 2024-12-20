using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveData : MonoBehaviour
{
    public void LoadWaves(List<Wave> waves)
    {
        waves.Add(CreateWave("Wave 0",
            new List<EnemyConfig>
            {

                new EnemyConfig { theme = NPC_Customization.NPCTheme.Roman, enemyClass = NPC_Customization.NPCClass.Bomber, waveSize = 1 }

            }
        ));
        waves.Add(CreateWave("Wave 1",
            new List<EnemyConfig>
            {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Roman, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 10 }
            }
        ));

        waves.Add(CreateWave("Wave 2",
            new List<EnemyConfig>
            {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Roman, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 1 }
            }
        ));

        waves.Add(CreateWave("Wave 3",
            new List<EnemyConfig>
            {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Roman, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 20 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Roman, enemyClass = NPC_Customization.NPCClass.Warrior, waveSize = 5 }
            }
        ));

        waves.Add(CreateWave("Wave 4",
            new List<EnemyConfig>
            {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Roman, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 25 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Roman, enemyClass = NPC_Customization.NPCClass.Warrior, waveSize = 7 }
            }
        ));
        waves.Add(CreateWave("Wave 5",
            new List<EnemyConfig>
            {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Fantasy, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 30 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Fantasy, enemyClass = NPC_Customization.NPCClass.Warrior, waveSize = 5 }
            }
        ));
        waves.Add(CreateWave("Wave 6",
                new List<EnemyConfig>
                {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Fantasy, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 30 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Fantasy, enemyClass = NPC_Customization.NPCClass.Warrior, waveSize = 1 }
                }
            ));
        waves.Add(CreateWave("Wave 7",
                new List<EnemyConfig>
                {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Fantasy, enemyClass = NPC_Customization.NPCClass.Charger, waveSize = 1 }
                }
            ));
        waves.Add(CreateWave("Wave 8",
                new List<EnemyConfig>
                {
               new EnemyConfig { theme = NPC_Customization.NPCTheme.Fantasy, enemyClass = NPC_Customization.NPCClass.Warrior, waveSize = 15 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Fantasy, enemyClass = NPC_Customization.NPCClass.Charger, waveSize = 3 }
                }
            ));
        waves.Add(CreateWave("Wave 9",
                new List<EnemyConfig>
                {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Fantasy, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 50 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Fantasy, enemyClass = NPC_Customization.NPCClass.Warrior, waveSize = 5 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Fantasy, enemyClass = NPC_Customization.NPCClass.Charger, waveSize = 2 }
                }
            ));
        waves.Add(CreateWave("Wave 10",
                new List<EnemyConfig>
                {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Pirate, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 10 }
                }
            ));
        waves.Add(CreateWave("Wave 11",
            new List<EnemyConfig>
            {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Pirate, enemyClass = NPC_Customization.NPCClass.Bannerman, waveSize = 1 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Pirate, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 10 }
            }
        ));
        waves.Add(CreateWave("Wave 12",
            new List<EnemyConfig>
            {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Pirate, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 30 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Pirate, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 15 }
            }
        ));
        waves.Add(CreateWave("Wave 13",
            new List<EnemyConfig>
            {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Pirate, enemyClass = NPC_Customization.NPCClass.Bannerman, waveSize = 1 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Pirate, enemyClass = NPC_Customization.NPCClass.Warrior, waveSize = 35 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Pirate, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 15 }
            }
        ));
        waves.Add(CreateWave("Wave 14",
            new List<EnemyConfig>
            {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Pirate, enemyClass = NPC_Customization.NPCClass.Bannerman, waveSize = 1 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Pirate, enemyClass = NPC_Customization.NPCClass.Warrior, waveSize = 35 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Pirate, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 15 }
            }
        ));
        waves.Add(CreateWave("Wave 15",
            new List<EnemyConfig>
            {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Cowboys, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 15 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Cowboys, enemyClass = NPC_Customization.NPCClass.Charger, waveSize = 1 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Natives, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 15 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Natives, enemyClass = NPC_Customization.NPCClass.Charger, waveSize = 1 }
            }
        ));
        waves.Add(CreateWave("Wave 16",
            new List<EnemyConfig>
            {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Cowboys, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 20 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Cowboys, enemyClass = NPC_Customization.NPCClass.Warrior, waveSize = 10 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Natives, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 20 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Natives, enemyClass = NPC_Customization.NPCClass.Warrior, waveSize = 10 }
            }
        ));
        waves.Add(CreateWave("Wave 17",
            new List<EnemyConfig>
            {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Cowboys, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 20 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Cowboys, enemyClass = NPC_Customization.NPCClass.Warrior, waveSize = 10 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Cowboys, enemyClass = NPC_Customization.NPCClass.Charger, waveSize = 1 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Natives, enemyClass = NPC_Customization.NPCClass.Bannerman, waveSize = 1 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Natives, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 20 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Natives, enemyClass = NPC_Customization.NPCClass.Warrior, waveSize = 10 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Natives, enemyClass = NPC_Customization.NPCClass.Charger, waveSize = 1 }
            }
        ));
        waves.Add(CreateWave("Wave 18",
            new List<EnemyConfig>
            {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Cowboys, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 30 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Cowboys, enemyClass = NPC_Customization.NPCClass.Warrior, waveSize = 7 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Cowboys, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 30 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Cowboys, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 17 }
            }
        ));
        waves.Add(CreateWave("Wave 19",
            new List<EnemyConfig>
            {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Cowboys, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 30 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Cowboys, enemyClass = NPC_Customization.NPCClass.Warrior, waveSize = 10 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Cowboys, enemyClass = NPC_Customization.NPCClass.Charger, waveSize = 1 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Natives, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 30 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Natives, enemyClass = NPC_Customization.NPCClass.Warrior, waveSize = 10 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Natives, enemyClass = NPC_Customization.NPCClass.Charger, waveSize = 1 }
            }
        ));
        waves.Add(CreateWave("Wave 20",
           new List<EnemyConfig>
           {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Farm, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 60 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Farm, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 5 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Farm, enemyClass = NPC_Customization.NPCClass.Warrior, waveSize = 20 }
           }
       ));
        waves.Add(CreateWave("Wave 21",
           new List<EnemyConfig>
           {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Farm, enemyClass = NPC_Customization.NPCClass.Charger, waveSize = 3 }
           }
       ));
        waves.Add(CreateWave("Wave 22",
           new List<EnemyConfig>
           {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Farm, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 25 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Farm, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 10 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Farm, enemyClass = NPC_Customization.NPCClass.RockThrower, waveSize = 10 }
           }
       ));
        waves.Add(CreateWave("Wave 23",
           new List<EnemyConfig>
           {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Farm, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 30 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Farm, enemyClass = NPC_Customization.NPCClass.RockThrower, waveSize = 10 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Farm, enemyClass = NPC_Customization.NPCClass.Warrior, waveSize = 10 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Farm, enemyClass = NPC_Customization.NPCClass.Charger, waveSize = 1 }
           }
       ));
        waves.Add(CreateWave("Wave 24",
           new List<EnemyConfig>
           {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Farm, enemyClass = NPC_Customization.NPCClass.Bannerman, waveSize = 1 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Farm, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 30 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Farm, enemyClass = NPC_Customization.NPCClass.RockThrower, waveSize = 10 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Farm, enemyClass = NPC_Customization.NPCClass.Warrior, waveSize = 10 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.Farm, enemyClass = NPC_Customization.NPCClass.Charger, waveSize = 1 }
           }
       ));

        //NO WARRIORS FOR CURRENT DAY
        waves.Add(CreateWave("Wave 25",
           new List<EnemyConfig>
           {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.CurrentDay, enemyClass = NPC_Customization.NPCClass.Bannerman, waveSize = 1 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.CurrentDay, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 20 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.CurrentDay, enemyClass = NPC_Customization.NPCClass.Warrior, waveSize = 20 }
           }
       ));
        waves.Add(CreateWave("Wave 26",
           new List<EnemyConfig>
           {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.CurrentDay, enemyClass = NPC_Customization.NPCClass.Bannerman, waveSize = 1 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.CurrentDay, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 30 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.CurrentDay, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 20 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.CurrentDay, enemyClass = NPC_Customization.NPCClass.Warrior, waveSize = 20 }
           }
       ));
        waves.Add(CreateWave("Wave 27",
           new List<EnemyConfig>
           {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.CurrentDay, enemyClass = NPC_Customization.NPCClass.Bannerman, waveSize = 1 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.CurrentDay, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 60 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.CurrentDay, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 30 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.CurrentDay, enemyClass = NPC_Customization.NPCClass.Warrior, waveSize = 20 }
           }
       ));
        waves.Add(CreateWave("Wave 28",
           new List<EnemyConfig>
           {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.CurrentDay, enemyClass = NPC_Customization.NPCClass.Bannerman, waveSize = 1 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.CurrentDay, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 30 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.CurrentDay, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 20 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.CurrentDay, enemyClass = NPC_Customization.NPCClass.Warrior, waveSize = 20 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.CurrentDay, enemyClass = NPC_Customization.NPCClass.Charger, waveSize = 1 }
           }
       ));
        waves.Add(CreateWave("Wave 29",
           new List<EnemyConfig>
           {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.CurrentDay, enemyClass = NPC_Customization.NPCClass.Bannerman, waveSize = 1 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.CurrentDay, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 30 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.CurrentDay, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 20 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.CurrentDay, enemyClass = NPC_Customization.NPCClass.Warrior, waveSize = 20 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.CurrentDay, enemyClass = NPC_Customization.NPCClass.Charger, waveSize = 1 }
           }
       ));
        waves.Add(CreateWave("Wave 30",
           new List<EnemyConfig>
           {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.SciFi, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 50 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.SciFi, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 20 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.SciFi, enemyClass = NPC_Customization.NPCClass.Charger, waveSize = 1 }
           }
       ));
        waves.Add(CreateWave("Wave 31",
           new List<EnemyConfig>
           {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.SciFi, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 60 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.SciFi, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 20 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.SciFi, enemyClass = NPC_Customization.NPCClass.Charger, waveSize = 1 }
           }
       ));
        waves.Add(CreateWave("Wave 32",
           new List<EnemyConfig>
           {
                new EnemyConfig { theme = NPC_Customization.NPCTheme.SciFi, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 60 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.SciFi, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 20 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.SciFi, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 20 }
           }
       ));
        waves.Add(CreateWave("Wave 33",
           new List<EnemyConfig>
           {
                 new EnemyConfig { theme = NPC_Customization.NPCTheme.SciFi, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 60 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.SciFi, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 20 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.SciFi, enemyClass = NPC_Customization.NPCClass.Charger, waveSize = 1 }
           }
       ));
        waves.Add(CreateWave("Wave 34",
           new List<EnemyConfig>
           {
               new EnemyConfig { theme = NPC_Customization.NPCTheme.SciFi, enemyClass = NPC_Customization.NPCClass.Basic, waveSize = 60 },
                new EnemyConfig { theme = NPC_Customization.NPCTheme.SciFi, enemyClass = NPC_Customization.NPCClass.Runner, waveSize = 20 }
           }
           ));
        waves.Add(CreateWave("Wave 35",
           new List<EnemyConfig>
           {
               new EnemyConfig { theme = NPC_Customization.NPCTheme.SciFi, enemyClass = NPC_Customization.NPCClass.CyberGiant, waveSize = 1 }
           }
           ));


    }

    public void GenerateEndlessWave(List<Wave> waves, string waveName)
    {
        // Define the possible Roman enemy classes to choose from
        List<NPC_Customization.NPCClass> enemyClasses = new List<NPC_Customization.NPCClass>
        {
            NPC_Customization.NPCClass.Basic,
            NPC_Customization.NPCClass.Warrior,
            NPC_Customization.NPCClass.Charger,
            NPC_Customization.NPCClass.Bannerman,
            NPC_Customization.NPCClass.Runner,
            NPC_Customization.NPCClass.Tank,
            NPC_Customization.NPCClass.Bomber
        };

        // List to hold random enemy configurations for the new wave
        List<EnemyConfig> enemyConfigs = new List<EnemyConfig>();

        // Determine a random number of different enemy types in the wave (between 1 and 4 types)
        int enemyTypesCount = Random.Range(1, 5);

        for (int i = 0; i < enemyTypesCount; i++)
        {
            // Pick a random enemy class from the Roman theme
            NPC_Customization.NPCClass randomClass = enemyClasses[Random.Range(0, enemyClasses.Count)];

            // Randomly choose a wave size between 5 and 50
            int waveSize = Random.Range(5, 6);

            // Add a new enemy configuration with the random class and wave size
            enemyConfigs.Add(new EnemyConfig
            {
                theme = NPC_Customization.NPCTheme.Corrupted,
                enemyClass = randomClass,
                waveSize = waveSize
            });
        }

        // Add the generated wave to the list of waves
        waves.Add(CreateWave(waveName, enemyConfigs));
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
