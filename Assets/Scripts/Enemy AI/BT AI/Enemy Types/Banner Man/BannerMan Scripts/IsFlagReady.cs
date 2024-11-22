using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsFlagReady : Node
{
    public override NodeState Evaluate(BaseManager agent)
    {

        BannerManManager bm = agent as BannerManManager;

        if (bm.isNewFlagReady)
        {

            return NodeState.SUCCESS;
        }

        else
        {
            return NodeState.FAILURE;
        }


    }
}
