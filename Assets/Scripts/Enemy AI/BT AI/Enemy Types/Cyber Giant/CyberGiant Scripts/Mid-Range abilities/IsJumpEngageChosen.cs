using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsJumpEngageChosen : Node
{
    public override NodeState Evaluate(BaseManager agent)
    {

        CyberGiantManager cg = agent as CyberGiantManager;

        if (cg.jumpEngageActive == true)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }
}
