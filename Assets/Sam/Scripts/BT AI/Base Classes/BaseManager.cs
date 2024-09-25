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
    public float currentHealth;
    public float damage, defense, speed, attackSpeed;
    public float attackRange;

    //---------GE EJ NÅGOT VÄRDE INUTI INSPEKTORN-------------

    public NavMeshAgent navMeshAgent;
    public PlayerManager playerManager1;
    public PlayerManager playerManager2;

    //-----------------PRIVATE/PROTECTED----------------------

    private GameObject player1;
    private GameObject player2;
    private float lastAttackedTime;

    //--------------------------------------------------------

    public virtual void Awake()
    {
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerManager1 = player1.GetComponent<PlayerManager>();
        playerManager2 = player2.GetComponent<PlayerManager>();
        
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
