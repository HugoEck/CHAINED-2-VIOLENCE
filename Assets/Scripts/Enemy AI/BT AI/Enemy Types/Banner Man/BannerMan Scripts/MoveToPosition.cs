using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPosition : Node
{
    public override NodeState Evaluate(BaseManager agent)
    {
        BannerManManager bm = agent as BannerManManager;
        agent.navigation.isStopped = false;
        agent.navigation.destination = bm.newDestination;
        float distance = Vector3.Distance(agent.transform.position, bm.newDestination);

        if (distance < 1)
        {


            return NodeState.RUNNING;
        }

        else
        {
            bm.isNewFlagReady = true;
            return NodeState.SUCCESS;
        }


    }
}
