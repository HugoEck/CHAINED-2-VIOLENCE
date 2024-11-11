using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsTransfer : MonoBehaviour
{
    public static StatsTransfer Instance { get; private set; }

    public static float Player1AttackDamage { get; set; }
    public static float Player2AttackDamage { get; set; }

    public static float Player1Health { get; set; }
    public static float Player2Health { get; set; }
    public static float Player1MaxHealth { get; set; }
    public static float Player2MaxHealth { get; set; }
    public static float Player1WalkingSpeed { get; set; }
    public static float Player2WalkingSpeed { get; set; }
    public static float CurrentChainLength { get; set; }



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
