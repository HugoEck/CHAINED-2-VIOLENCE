using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.LightAnchor;

public class OverheadSmash : Node
{
    float animationTotTime = 4.8f;
    float animationTimer = 4.8f;

    public override NodeState Evaluate(BaseManager agent)
    {
        agent.navigation.rotationSpeed = 10;
        CyberGiantManager cg = agent as CyberGiantManager;
        SetAnimation(agent);
        agent.navigation.isStopped = true;
        agent.targetedPlayer = agent.behaviorMethods.CalculateClosestTarget();

        if (animationTimer > 4f)
        {
            agent.behaviorMethods.RotateTowardsClosestPlayer();
            agent.animator.SetBool("CyberGiant_OverheadSmash1", true);
            agent.animator.SetBool("CyberGiant_OverheadSmash2", false);
   
        }
        else if(animationTimer < 3.2f && animationTimer > 2f)
        {
            agent.animator.SetBool("CyberGiant_OverheadSmash2", true);
            agent.animator.SetBool("CyberGiant_OverheadSmash1", false);
            agent.behaviorMethods.RotateTowardsClosestPlayer();
        }


        animationTimer -= Time.deltaTime;

        if (animationTimer < 0)
        {
            agent.navigation.rotationSpeed = 360;
            animationTimer = animationTotTime;
            cg.overheadSmashActive = false;
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.RUNNING;
        }
        
    }

    private void SetAnimation(BaseManager agent)
    {
        agent.animator.SetBool("CyberGiant_MissileRain", false);
        agent.animator.SetBool("CyberGiant_Walk", false);
        agent.animator.SetBool("CyberGiant_ShieldWalk", false);
        agent.animator.SetBool("CyberGiant_JumpEngage", false);
        agent.animator.SetBool("CyberGiant_Idle", false);

    }
    
}
