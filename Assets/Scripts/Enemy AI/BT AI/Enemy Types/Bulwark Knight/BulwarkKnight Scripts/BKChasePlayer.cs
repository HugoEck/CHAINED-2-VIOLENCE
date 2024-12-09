using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BKChasePlayer : Node
{
    public override NodeState Evaluate(BaseManager agent)
    {
        BulwarkKnightManager bk = agent as BulwarkKnightManager;

        if (bk.shieldBroken)
        {
            //Running Animation
        }
        else
        {
            //Shield Animation
        }
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

            return NodeState.SUCCESS;
        }


    }
}
