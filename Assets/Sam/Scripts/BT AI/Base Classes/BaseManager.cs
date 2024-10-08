using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;
using Unity.Netcode.Components;
public class BaseManager : NetworkBehaviour
{

    //Detta skript innehåller alla basvariabler samt metoder för alla andra managers.


    public float maxHealth;
    public float currentHealth;
    public float attack, defense, speed, attackSpeed;
    public float attackRange;
    public float unitCost;


    private GameObject player1;
    private GameObject player2;
    private float lastAttackedTime;
    private float timer = 5f;

    [Header("GE EJ VÄRDE")]

    public NavMeshAgent navMeshAgent;
    public PlayerManager playerManager1;
    public PlayerManager playerManager2;
    public Animator animator;
    public string enemyID;
    [HideInInspector] public bool activateDeathTimer = false;
    [HideInInspector] public bool agentIsDead = false;



    public virtual void Awake()
    {

        //gameObject.AddComponent<NetworkObject>();
        //gameObject.AddComponent<NetworkTransform>();
        player1 = GameObject.FindGameObjectWithTag("Player1");
        player2 = GameObject.FindGameObjectWithTag("Player2");
        animator = gameObject.GetComponent<Animator>();

        navMeshAgent = GetComponent<NavMeshAgent>();
        playerManager1 = player1.GetComponent<PlayerManager>();
        playerManager2 = player2.GetComponent<PlayerManager>();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            currentHealth = 0;
        }
        if (activateDeathTimer)
        {
            DeathTimer();
        }

    }

    public virtual void DealDamageToEnemy(float damage)
    {
        if (defense - damage < 0)
        {
            currentHealth = currentHealth + defense - damage;
        }

    }

    public virtual Transform CalculateClosestTarget()
    {
        if (Vector3.Distance(this.transform.position, player1.transform.position) < Vector3.Distance(this.transform.position, player2.transform.position))
        {
            return player1.transform;
        }
        else
        {
            return player2.transform;
        }
    }

    public virtual bool IsAttackAllowed()
    {

        if (Time.time > lastAttackedTime + attackSpeed)
        {
            lastAttackedTime = Time.time;
            return true;
        }
        else
        {
            return false;
        }
    }

    public PlayerManager GetCorrectPlayerManager(Transform player)
    {
        if (player == player1)
        {
            return playerManager1;
        }
        else
        {
            return playerManager2;
        }
    }

    public virtual void DeathTimer()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            agentIsDead = true;
        }
    }



}