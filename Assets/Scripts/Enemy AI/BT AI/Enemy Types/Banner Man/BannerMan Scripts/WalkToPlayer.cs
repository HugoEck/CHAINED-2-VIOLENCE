using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkToPlayer : Node
{
    public override NodeState Evaluate(BaseManager agent)
    {
        agent.animator.SetBool("BannerMan_Running", true);
        agent.animator.SetBool("BannerMan_Scared", false);
        agent.animator.SetBool("BannerMan_Electrocute", false);
        
        BannerManManager bm = agent as BannerManManager;
        agent.navigation.isStopped = false;

        agent.targetedPlayer = agent.behaviorMethods.CalculateClosestTarget();
        agent.navigation.destination = agent.targetedPlayer.position;
        float distance = Vector3.Distance(agent.transform.position, agent.targetedPlayer.transform.position);

        if (distance > agent.attackRange)
        {

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
