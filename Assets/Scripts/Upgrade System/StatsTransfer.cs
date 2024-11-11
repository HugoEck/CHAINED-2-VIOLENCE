using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsTransfer : MonoBehaviour
{
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;

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

    private bool _bHasPlayer1HpBeenSet = false;
    private bool _bHasPlayer2HpBeenSet = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            player1.GetComponent<Player>().PlayerSpawnedIn += OnPlayer1SpawnedIn;
            player2.GetComponent<Player>().PlayerSpawnedIn += OnPlayer2SpawnedIn;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        player1.GetComponent<Player>().PlayerSpawnedIn -= OnPlayer1SpawnedIn;
        player2.GetComponent<Player>().PlayerSpawnedIn -= OnPlayer2SpawnedIn;
    }

    private void OnPlayer2SpawnedIn(float maxHealth)
    {
        if(!_bHasPlayer2HpBeenSet)
        {
            Player2Health = maxHealth;
            _bHasPlayer2HpBeenSet = true;
        }    
    }

    private void OnPlayer1SpawnedIn(float maxHealth)
    {
        if(!_bHasPlayer1HpBeenSet)
        {
            Player1Health = maxHealth;
            _bHasPlayer1HpBeenSet = true;
        }
       
    }
}
