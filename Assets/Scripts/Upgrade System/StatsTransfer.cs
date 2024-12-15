using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsTransfer : MonoBehaviour
{
    public static StatsTransfer Instance { get; private set; }
    //[SerializeField] 
    private GameObject player1;
    //[SerializeField]
    private GameObject player2;

    private PlayerAttributes player1Attributes;
    private PlayerAttributes player2Attributes;

    private bool _bHasPlayer1HpBeenSet = false;
    private bool _bHasPlayer2HpBeenSet = false;

    public static float CurrentChainLength { get; set; }

    public static float Player1AttackDamage { get; set; }
    public static float Player2AttackDamage { get; set; }
    public static float Player1MaxHealth { get; set; }
    public static float Player2MaxHealth { get; set; }
    public static float Player1WalkingSpeed { get; set; }
    public static float Player2WalkingSpeed { get; set; }

    //-------------- Upgraded stats only --------------
    public static float Player1UpgradedMaxHP { get; set; }
    public static float Player2UpgradedMaxHP { get; set; }
    public static float Player1UpgradedSpeed { get; set; }
    public static float Player2UpgradedSpeed { get; set; }
    public static float Player1UpgradedAttackDamage { get; set; }
    public static float Player2UpgradedAttackDamage { get; set; }

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

    private void Start()
    {

    }

    private void OnDestroy()
    {
        player1.GetComponent<Player>().PlayerSpawnedIn -= OnPlayer1SpawnedIn;
        player2.GetComponent<Player>().PlayerSpawnedIn -= OnPlayer2SpawnedIn;
        SaveStatsPlayer1(player1Attributes);
        SaveStatsPlayer2(player2Attributes);
    }

    private void Update()
    {
        if (player1 == null || player2 == null)
        {
            GameObject player1Object = GameObject.FindGameObjectWithTag("Player1");
            GameObject player2Object = GameObject.FindGameObjectWithTag("Player2");

            if (player1Object != null && player2Object != null)
            {
                player1 = player1Object;
                player2 = player2Object;

                player1Attributes = player1.GetComponent<PlayerAttributes>();
                player2Attributes = player2.GetComponent<PlayerAttributes>();

                if (player1Attributes != null)
                {
                    ApplyStatsPlayer1();
                    //Debug.Log("JACK STATS 1 APPLIED" + player1Attributes.maxHP + ("HP UPGRADE P1q = ") + Player1UpgradedMaxHP);
                }
                else
                {
                    //Debug.LogWarning("NO PLAYER1 ATTRIBUTES");
                }

                if (player2Attributes != null)
                {
                    ApplyStatsPlayer2();
                   //Debug.Log("JACK STATS 2 APPLIED" + player2Attributes.maxHP + ("HP UPGRADE P2 = ") + Player2UpgradedMaxHP);
                }
                else
                {
                    //Debug.LogWarning("NO PLAYER2 ATTRIBUTES");
                }
            }
        }
    }
    public void ApplyStatsPlayer1()
    {
        if (player1Attributes != null)
        {
            player1Attributes.attackDamage = Player1AttackDamage + Player1UpgradedAttackDamage;
            player1Attributes.maxHP = Player1MaxHealth + Player1UpgradedMaxHP;
            player1Attributes.movementSpeed = Player1WalkingSpeed + Player1UpgradedSpeed;
        }
    }

    public void ApplyStatsPlayer2()
    {
        if (player2Attributes != null)
        {
            player2Attributes.attackDamage = Player2AttackDamage + Player2UpgradedAttackDamage;
            player2Attributes.maxHP = Player2MaxHealth + Player2UpgradedMaxHP;
            player2Attributes.movementSpeed = Player2WalkingSpeed + Player2UpgradedSpeed;
        }
    }

    public void SaveStatsPlayer1(PlayerAttributes player1AttributesTMP)
    {
        player1Attributes = player1AttributesTMP;
        if (player1Attributes != null)
        {
            Player1AttackDamage = player1Attributes.attackDamage;
            Player1MaxHealth = player1Attributes.maxHP;
            Player1WalkingSpeed = player1Attributes.movementSpeed;
        }
    }

    public void SaveStatsPlayer2(PlayerAttributes player2AttributesTMP)
    {
        player2Attributes = player2AttributesTMP;

        if (player2Attributes != null)
        {
            Player2AttackDamage = player2Attributes.attackDamage;
            Player2MaxHealth = player2Attributes.maxHP;
            Player2WalkingSpeed = player2Attributes.movementSpeed;
        }
    }
    public void SaveStatsPlayer1Upgrades(PlayerAttributes player1qAttributesTMP)
    {
        player1Attributes = player1qAttributesTMP;

        Player1UpgradedAttackDamage = player1Attributes._upgradeAttackDamage;
        Player1UpgradedMaxHP = player1Attributes._upgradeMaxHP;
        Player1UpgradedSpeed = player1Attributes._upgradeMovementSpeed;
    }

    public void SaveStatsPlayer2Upgrades(PlayerAttributes player2AttributesTMP)
    {
        player2Attributes = player2AttributesTMP;

        Player2UpgradedAttackDamage = player2Attributes._upgradeAttackDamage;
        Player2UpgradedMaxHP = player2Attributes._upgradeMaxHP;
        Player2UpgradedSpeed = player2Attributes._upgradeMovementSpeed;
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
            Player1MaxHealth = maxHealth;
            _bHasPlayer1HpBeenSet = true;
        }
    }
}
