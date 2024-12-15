using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BChargeConditions : Node
{
    float distance;
    public override NodeState Evaluate(BaseManager agent)
    {
        BomberManager bomber = agent as BomberManager;

        if (bomber.bombActivated && bomber.bombExploded != true)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }

}
