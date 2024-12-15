using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BChaseConditions : Node
{

    public override NodeState Evaluate(BaseManager agent)
    {
        
        BomberManager bomber = agent as BomberManager;

        if (!bomber.bombExploded)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }


    }
}
