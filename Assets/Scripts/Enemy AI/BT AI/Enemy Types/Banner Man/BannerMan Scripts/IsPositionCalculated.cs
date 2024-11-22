using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsPositionCalculated : Node
{

    public override NodeState Evaluate(BaseManager agent)
    {

        BannerManManager bm = agent as BannerManManager;

        if ( bm.isNewDestinationCalculated == false)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }
}
