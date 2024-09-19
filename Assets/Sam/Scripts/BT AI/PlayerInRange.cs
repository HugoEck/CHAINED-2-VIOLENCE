using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInRange : Node
{
    private float attackRange;
    private Transform body;

    public PlayerInRange(BaseManager agent)
    {
        attackRange = agent.attackRange;
        
        targetedPlayer = agent.CalculateClosestTarget();
    }

    public override NodeState Evaluate(BaseManager agent)
    {
        float distance = Vector3.Distance(agent.transform.position, targetedPlayer.position);

        if (distance <= attackRange)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }

}
