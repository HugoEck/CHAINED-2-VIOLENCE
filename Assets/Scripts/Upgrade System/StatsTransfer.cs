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

    //public static float Player1Health { get; set; }
    //public static float Player2Health { get; set; }
    public static float Player1MaxHealth { get; set; }
    public static float Player2MaxHealth { get; set; }
    public static float Player1WalkingSpeed { get; set; }
    public static float Player2WalkingSpeed { get; set; }
    public static float CurrentChainLength { get; set; }

    private PlayerAttributes player1Attributes;
    private PlayerAttributes player2Attributes;

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
            if (player1 != null) player1Attributes = player1.GetComponent<PlayerAttributes>();
            if (player2 != null) player2Attributes = player2.GetComponent<PlayerAttributes>();        
        }

        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //ApplyStats();
        //Player1AttackDamage = player1Attributes.attackDamage;
        //Player1MaxHealth = player1Attributes.maxHP;
        //Player1WalkingSpeed = player1Attributes.movementSpeed;

        //Player2AttackDamage = player2Attributes.attackDamage;
        //Player2MaxHealth = player2Attributes.maxHP;
        //Player2WalkingSpeed = player2Attributes.movementSpeed;

    }

    private void OnDestroy()
    {
        player1.GetComponent<Player>().PlayerSpawnedIn -= OnPlayer1SpawnedIn;
        player2.GetComponent<Player>().PlayerSpawnedIn -= OnPlayer2SpawnedIn;
        SaveStats();
    }

    public void ApplyStats()
    {
        if (player1Attributes != null)
        {
            player1Attributes.AdjustAttackDamage(Player1AttackDamage);
            player1Attributes.AdjustMaxHP(Player1MaxHealth);
            player1Attributes.AdjustMovementSpeed(Player1WalkingSpeed);
        }

        if (player2Attributes != null)
        {
            player2Attributes.AdjustAttackDamage(Player2AttackDamage);
            player2Attributes.AdjustMaxHP(Player2MaxHealth);
            player2Attributes.AdjustMovementSpeed(Player2WalkingSpeed);
        }
    }

    public void SaveStats()
    {
        if (player1Attributes != null)
        {
            Player1AttackDamage = player1Attributes.attackDamage;
            Player1MaxHealth = player1Attributes.maxHP;
            Player1WalkingSpeed = player1Attributes.movementSpeed;
        }

        if (player2Attributes != null)
        {
            Player2AttackDamage = player2Attributes.attackDamage;
            Player2MaxHealth = player2Attributes.maxHP;
            Player2WalkingSpeed = player2Attributes.movementSpeed;
        }
    }


    private void OnPlayer2SpawnedIn(float maxHealth)
    {
        if (!_bHasPlayer2HpBeenSet)
        {
            Player2MaxHealth = maxHealth;
            _bHasPlayer2HpBeenSet = true;
        }
    }

    private void OnPlayer1SpawnedIn(float maxHealth)
    {
        if (!_bHasPlayer1HpBeenSet)
        {
            Player2MaxHealth = maxHealth; // Used to be playerhealth??
            _bHasPlayer1HpBeenSet = true;
        }

    }
}
