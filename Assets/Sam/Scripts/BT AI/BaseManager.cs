using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class BaseManager : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public int damage, defense, speed;
    public float attackRange;
    public Transform player1;
    public Transform player2;


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

    
    
}
