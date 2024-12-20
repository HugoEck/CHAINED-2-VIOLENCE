using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfDead : Node
{

    private int deadThreshold = 0;
    

    public override NodeState Evaluate(BaseManager agent)
    {
        if (agent.currentHealth <= deadThreshold && !agent.agentIsDead)
        {
            agent.activateDeathTimer = true;
            
            return NodeState.SUCCESS;
        }
        else
        {

            return NodeState.FAILURE;
        }
    }
}