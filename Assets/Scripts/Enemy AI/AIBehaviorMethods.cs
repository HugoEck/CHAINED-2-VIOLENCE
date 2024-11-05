using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class AIBehaviorMethods
{
    BaseManager agent;
    float lastAttackedTime;
    
    public AIBehaviorMethods(BaseManager manager)
    {
        agent = manager;
    }

    public Transform CalculateClosestTarget()
    {
        if (Vector3.Distance(agent.transform.position, agent.player1.transform.position) < Vector3.Distance(agent.transform.position, agent.player2.transform.position))
        {
            return agent.player1.transform;
        }
        else if ((Vector3.Distance(agent.transform.position, agent.player2.transform.position) < Vector3.Distance(agent.transform.position, agent.player1.transform.position)))
        {
            return agent.player2.transform;
        }
        else
        {
            return null;
        }
    }

    public bool IsAttackAllowed()
    {

        if (Time.time > lastAttackedTime + agent.attackSpeed)
        {
            lastAttackedTime = Time.time;
            return true;
        }
        else
        {
            return false;
        }
    }

    public Player GetCorrectPlayerManager(Transform player)
    {
        if (player == agent.player1.transform)
        {
            return agent.playerManager1;
        }
        else
        {
            return agent.playerManager2;
        }
    }

    public Vector3 CalculateChainPosition()
    {

        Vector3 p1Position = agent.player1.transform.position;
        Vector3 p2Position = agent.player2.transform.position;
        Vector3 midPoint = (p1Position + p2Position) / 2;
        midPoint.y = 0;
        return midPoint;
    }

}
