using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class BaseManager : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public float damage, defense, speed, attackSpeed;
    public float attackRange;
    public float lastAttackedTime;
    public Transform player1;
    public Transform player2;

    public NavMeshAgent navMeshAgent;
    public PlayerManager playerManager1;
    public PlayerManager playerManager2;

    public virtual void Awake()
    {
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
        if (Vector3.Distance(this.transform.position, player1.position) < Vector3.Distance(this.transform.position, player2.position))
        {
            return player1;
        }
        else
        {
            return player2;
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
