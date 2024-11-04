using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsAgentStunned : Node
{

    public override NodeState Evaluate(BaseManager agent)
    {
        if (agent.chainEffects.stunActivated == true)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }
}
