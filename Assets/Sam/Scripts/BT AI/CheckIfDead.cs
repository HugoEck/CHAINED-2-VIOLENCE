using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfDead : Node
{

    private int deadThreshold = 0;
    
    public CheckIfDead(BaseManager agent)
    {

    }

    public override NodeState Evaluate(BaseManager agent)
    {
        if (agent.currentHealth <= deadThreshold)
        {
            
            return NodeState.SUCCESS;
        }
        else
        {
            
            return NodeState.FAILURE;
        }
    }
}
