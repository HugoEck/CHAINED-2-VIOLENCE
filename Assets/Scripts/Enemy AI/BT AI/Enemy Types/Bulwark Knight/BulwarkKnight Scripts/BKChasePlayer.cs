using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BKChasePlayer : Node
{
    public override NodeState Evaluate(BaseManager agent)
    {
        BulwarkKnightManager bk = agent as BulwarkKnightManager;

        SetAnimation(agent);
        if (bk.shieldBroken)
        {
            agent.animator.SetBool("BulwarkKnight_ShieldWalk", false);
            //agent.animator.SetBool("BulwarkKnight_SwordRun", true);
        }
        else
        {
            agent.animator.SetBool("BulwarkKnight_ShieldWalk", true);
            //agent.animator.SetBool("BulwarkKnight_SwordRun", false);
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
    private void SetAnimation(BaseManager agent)
    {
        agent.animator.SetBool("BulwarkKnight_Idle", false);
    }
}
