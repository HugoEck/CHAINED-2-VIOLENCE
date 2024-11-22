using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkToPlayer : Node
{
    public override NodeState Evaluate(BaseManager agent)
    {
        BannerManManager bm = agent as BannerManManager;

        agent.targetedPlayer = agent.behaviorMethods.CalculateClosestTarget();
        float distance = Vector3.Distance(agent.transform.position, agent.targetedPlayer.transform.position);

        if (distance > agent.attackRange)
        {
            agent.navigation.isStopped = false;
            agent.navigation.destination = agent.targetedPlayer.position;
            return NodeState.RUNNING;
        }

        else
        {
            agent.navigation.isStopped = true;
            bm.hasAlreadyReachedPlayer = true;
            return NodeState.SUCCESS;
        }


    }
}
