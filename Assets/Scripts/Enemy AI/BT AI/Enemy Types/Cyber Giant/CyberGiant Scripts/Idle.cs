using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : Node
{
    public override NodeState Evaluate(BaseManager agent)
    {
        agent.navigation.rotationSpeed = 5;
        SetAnimation(agent);
        agent.navigation.isStopped = true;
        agent.targetedPlayer = agent.behaviorMethods.CalculateClosestTarget();
        agent.behaviorMethods.RotateTowardsClosestPlayer();


        return NodeState.SUCCESS;
    }
    private void SetAnimation(BaseManager agent)
    {
        agent.animator.SetBool("CyberGiant_Idle", true);
        agent.animator.SetBool("CyberGiant_MissileRain", false);
        agent.animator.SetBool("CyberGiant_Walk", false);
        agent.animator.SetBool("CyberGiant_JumpEngage", false);
        agent.animator.SetBool("CyberGiant_OverheadSmash1", false);
        agent.animator.SetBool("CyberGiant_OverheadSmash2", false);
    }
}
