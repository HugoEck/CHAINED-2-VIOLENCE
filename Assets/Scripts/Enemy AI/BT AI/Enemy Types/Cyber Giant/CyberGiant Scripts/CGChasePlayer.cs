using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGChasePlayer : Node
{
    public override NodeState Evaluate(BaseManager agent)
    {
        CyberGiantManager cg = agent as CyberGiantManager;

        agent.navigation.rotationSpeed = 360;
        SetAnimation(agent);

        if (cg.shieldWalkActive)
        {
            agent.animator.SetBool("CyberGiant_ShieldWalk", true);
            agent.animator.SetBool("CyberGiant_Walk", false);
        }
        else
        {
            agent.animator.SetBool("CyberGiant_Walk", true);
            agent.animator.SetBool("CyberGiant_ShieldWalk", false);
        }
        agent.targetedPlayer = agent.behaviorMethods.CalculateClosestTarget();
        //agent.behaviorMethods.RotateTowardsClosestPlayer();

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

            return NodeState.FAILURE;
        }


    }

    public void SetAnimation(BaseManager agent)
    {
        agent.animator.SetBool("CyberGiant_MissileRain", false);
        agent.animator.SetBool("CyberGiant_JumpEngage", false);
        agent.animator.SetBool("CyberGiant_OverheadSmash1", false);
        agent.animator.SetBool("CyberGiant_OverheadSmash2", false);
        agent.animator.SetBool("CyberGiant_Idle", false);
        agent.animator.SetBool("CyberGiant_Stagger", false);
    }
}
