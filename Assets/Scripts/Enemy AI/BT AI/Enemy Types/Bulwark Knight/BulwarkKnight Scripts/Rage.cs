using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rage : Node
{
    bool rageBuffActivated = false;
    public override NodeState Evaluate(BaseManager agent)
    {
        SetAnimation(agent);
        agent.navigation.isStopped = true;
        BulwarkKnightManager bk = agent as BulwarkKnightManager;

        agent.behaviorMethods.RotateTowardsClosestPlayer();

        if (rageBuffActivated == false)
        {
            rageBuffActivated= true;
            bk.BreakShield();
        }

        if (bk.rageAnimationTimer < 0)
        {
            bk.rageActive = false;
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.RUNNING;
        }

    }
    private void SetAnimation(BaseManager agent)
    {
        agent.animator.SetBool("BulwarkKnight_Rage", true);
        agent.animator.SetBool("BulwarkKnight_Electrocute", false);
        agent.animator.SetBool("BulwarkKnight_Scared", false);
        agent.animator.SetBool("BulwarkKnight_ShieldWalk", false);
        agent.animator.SetBool("BulwarkKnight_ShieldIdle", false);
        agent.animator.SetBool("BulwarkKnight_SwordIdle", false);
        agent.animator.SetBool("BulwarkKnight_ShieldAttack", false);
        agent.animator.SetBool("BulwarkKnight_SwordAttack", false);
        agent.animator.SetBool("BulwarkKnight_SwordRun", false);
        
    }
}
