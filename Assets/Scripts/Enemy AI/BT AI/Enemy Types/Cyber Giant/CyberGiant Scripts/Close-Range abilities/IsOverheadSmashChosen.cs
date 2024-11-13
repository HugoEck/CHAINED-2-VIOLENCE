using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsOverheadSmashChosen : Node
{
    public override NodeState Evaluate(BaseManager agent)
    {

        CyberGiantManager cg = agent as CyberGiantManager;

        if (cg.overheadSmashActive == true)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }
}
