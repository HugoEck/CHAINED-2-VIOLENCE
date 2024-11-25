using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsPlayerReached : Node
{

    public override NodeState Evaluate(BaseManager agent)
    {

        BannerManManager bm = agent as BannerManManager;

        if (bm.hasAlreadyReachedPlayer)
        {

            return NodeState.FAILURE;
        }

        else 
        {
            return NodeState.SUCCESS;
        }


    }
}
