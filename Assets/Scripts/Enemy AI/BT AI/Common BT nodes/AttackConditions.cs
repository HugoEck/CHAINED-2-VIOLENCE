using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackConditions : Node
{
    float distance;
    public override NodeState Evaluate(BaseManager agent)
    {

        agent.targetedPlayer = agent.behaviorMethods.CalculateClosestTarget();
        distance = Vector3.Distance(agent.transform.position, agent.targetedPlayer.position);

        if (distance <= agent.attackRange)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }
}
