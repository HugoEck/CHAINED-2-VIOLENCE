using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class BaseManager : MonoBehaviour
{

    //Detta skript innehåller alla basvariabler samt metoder för alla andra managers.



    //---------GE NÅGOT VÄRDE INUTI INSPEKTORN----------------

    public float maxHealth;
    public float damage;
    public float defense;
    public float speed;
    public float attackSpeed;
    public float attackRange;

    //---------GE EJ NÅGOT VÄRDE INUTI INSPEKTORN-------------

    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public PlayerManager playerManager1;
    [HideInInspector] public PlayerManager playerManager2;
    [HideInInspector] public float currentHealth;

    private GameObject player1;
    private GameObject player2;
    private float lastAttackedTime;


    public virtual void Awake()
    {

        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerManager1 = player1.GetComponent<PlayerManager>();
        playerManager2 = player2.GetComponent<PlayerManager>();
        currentHealth = maxHealth;
        navMeshAgent.speed = speed;

    }

    
    public virtual void SetHealth(float damage)
    {
        currentHealth -= damage;
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
    public virtual void AttackPlayer(Transform player)
    {
        PlayerManager playerManager= GetCorrectPlayerManager(player);

        if (Time.time > lastAttackedTime + attackSpeed)
        {
            playerManager.SetHealth(damage);
            lastAttackedTime = Time.time;
        }
    } 
    
    protected PlayerManager GetCorrectPlayerManager(Transform player)
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
}
