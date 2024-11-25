using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathConditions : Node
{

    public override NodeState Evaluate(BaseManager agent)
    {

        if(agent.currentHealth <= 0)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }



    }
}
