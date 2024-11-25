using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPosition : Node
{
    float stuckTimerValue = 20f;
    float currentTimer = 20f;
    Transform closestPlayer;
    public override NodeState Evaluate(BaseManager agent)
    {
        BannerManManager bm = agent as BannerManManager;
        agent.navigation.isStopped = false;
        agent.navigation.destination = bm.newDestination;
        float distance = Vector3.Distance(agent.transform.position, bm.newDestination);

        currentTimer -= Time.deltaTime;


        if (currentTimer < 0)
        {
            currentTimer = stuckTimerValue;
            closestPlayer = agent.behaviorMethods.CalculateClosestTarget();
            bm.newDestination = (agent.transform.position + closestPlayer.transform.position) / 2;
            
            return NodeState.SUCCESS;
        }
        else if (distance > 1)
        {
            return NodeState.RUNNING;
        }
        else
        {
            currentTimer = stuckTimerValue;
            bm.isNewFlagReady = true;
            return NodeState.SUCCESS;
        }


    }
}
