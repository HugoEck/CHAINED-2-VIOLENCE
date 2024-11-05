using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInRange : Node
{

    float distance;

    public override NodeState Evaluate(BaseManager agent)
    {
        agent.targetedPlayer = agent.CalculateClosestTarget();
        distance = Vector3.Distance(agent.transform.position, agent.targetedPlayer.position);

        if (distance <= agent.attackRange)
        {
            agent.navigation.isStopped = true;
            RotateTowardsPlayer(agent, agent.targetedPlayer);
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }

    private void RotateTowardsPlayer(BaseManager agent, Transform targetedPlayer)
    {
        Vector3 direction = (targetedPlayer.position - agent.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, lookRotation, Time.deltaTime * agent.navigation.rotationSpeed);
    }

}
