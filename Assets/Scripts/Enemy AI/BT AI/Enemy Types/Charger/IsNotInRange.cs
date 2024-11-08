using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IsNotInRange : Node
{
    private float distance;

    public override NodeState Evaluate(BaseManager agent)
    {
        targetedPlayer = agent.CalculateClosestTarget();
        distance = Vector3.Distance(agent.transform.position, targetedPlayer.position);

        if (distance > agent.attackRange)
        {

            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }
}
