using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BKChasePlayer : Node
{
    public override NodeState Evaluate(BaseManager agent)
    {
        agent.navigation.isStopped = false;
        BulwarkKnightManager bk = agent as BulwarkKnightManager;

        SetAnimation(agent);
        if (bk.shieldBroken)
        {
            agent.animator.SetBool("BulwarkKnight_ShieldWalk", false);
            agent.animator.SetBool("BulwarkKnight_SwordRun", true);
        }
        else
        {
            agent.animator.SetBool("BulwarkKnight_ShieldWalk", true);
            agent.animator.SetBool("BulwarkKnight_SwordRun", false);
        }
        agent.targetedPlayer = agent.behaviorMethods.CalculateClosestTarget();

        float distance = Vector3.Distance(agent.transform.position, agent.targetedPlayer.transform.position);

        if (distance > agent.attackRange)
        {
            agent.navigation.destination = agent.targetedPlayer.position;
            return NodeState.RUNNING;
        }

        else
        {
            return NodeState.SUCCESS;
        }

    }
    private void SetAnimation(BaseManager agent)
    {
        agent.animator.SetBool("BulwarkKnight_ShieldIdle", false);
        agent.animator.SetBool("BulwarkKnight_SwordIdle", false);
        agent.animator.SetBool("BulwarkKnight_ShieldAttack", false);
        agent.animator.SetBool("BulwarkKnight_SwordAttack", false);
        agent.animator.SetBool("BulwarkKnight_Electrocute", false);
        agent.animator.SetBool("BulwarkKnight_Scared", false);
        agent.animator.SetBool("BulwarkKnight_Rage", false);
    }
}
