using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI;
using UnityEngine.AI;

public class ChasePlayer : Node
{
    
    

    public override NodeState Evaluate(BaseManager agent)
    {
        targetedPlayer = agent.CalculateClosestTarget();
        float distance = Vector3.Distance(agent.transform.position, targetedPlayer.transform.position);
        
        if (distance > agent.attackRange)
        {
            agent.navMeshAgent.isStopped = false;
            agent.navMeshAgent.SetDestination(targetedPlayer.position);
            
            return NodeState.RUNNING;
        }
        
        else //Denna kan aldrig bli true eftersom att PlayerInRange hinner bli true före.
        {

            agent.navMeshAgent.isStopped = true;
            RotateTowardsPlayer(agent, targetedPlayer);
            return NodeState.SUCCESS;
        }

        
    }

    private void RotateTowardsPlayer(BaseManager agent, Transform targetedPlayer)
    {
        // Calculate the direction from the agent to the player
        Vector3 direction = (targetedPlayer.position - agent.transform.position).normalized;

        // Calculate the target rotation to face the player
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        // Smoothly rotate the agent towards the player
        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, lookRotation, Time.deltaTime * agent.navMeshAgent.angularSpeed);
    }
}
