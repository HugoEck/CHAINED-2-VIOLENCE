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
        
        
        
    }

    public override NodeState Evaluate(BaseManager agent)
    {
        targetedPlayer = agent.CalculateClosestTarget();
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
