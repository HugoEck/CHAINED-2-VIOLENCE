using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI;
using UnityEngine.AI;

public class ChasePlayer : Node
{
    
    private NavMeshAgent navMeshAgent;

    public ChasePlayer ( BaseManager agent, NavMeshAgent navMeshAgent)
    {
        targetedPlayer = agent.CalculateClosestTarget();
        this.navMeshAgent = navMeshAgent;
    }

    public override NodeState Evaluate(BaseManager agent)
    {
        float distance = Vector3.Distance(agent.transform.position, targetedPlayer.transform.position);

        if(distance > 0.2)
        {
            //navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(targetedPlayer.position);
            return NodeState.RUNNING;
        }
        else
        {
            //navMeshAgent.isStopped= true;
            return NodeState.SUCCESS;
        }

        
    }
}
