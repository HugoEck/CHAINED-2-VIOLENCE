using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class JumpEngage : Node
{
    float animationTotTime = 5.5f;
    float animationTimer = 5.5f;
    float jumpAnimationTime = 1;
    float distance;
    float jumpSpeed;
    Vector3 targetedPlayerLastPos;
    Vector3 jumpDirection;
    
    public override NodeState Evaluate(BaseManager agent)
    {
        agent.navigation.rotationSpeed = 360;
        SetAnimation(agent);

        CyberGiantManager cg = agent as CyberGiantManager;

        agent.navigation.isStopped = true;
        agent.targetedPlayer = agent.behaviorMethods.CalculateClosestTarget();

        distance = Vector3.Distance(agent.transform.position, agent.targetedPlayer.transform.position);

        if(animationTimer < 5.5f && animationTimer > 3.5f)
        {
            RotateTowardsPlayer(agent);
            targetedPlayerLastPos = agent.targetedPlayer.transform.position;
            agent.behaviorMethods.RotateTowardsClosestPlayer();
            jumpSpeed = distance / jumpAnimationTime;
            jumpDirection = (agent.targetedPlayer.position - agent.transform.position).normalized;
        }
        else if (animationTimer < 3.5f && animationTimer > 2.5f)
        {
            if(distance > cg.maxCloseRangeDistance - 7)
            {
                RotateTowardsPlayer(agent);
                agent.transform.position += jumpDirection * jumpSpeed * Time.deltaTime;
            }

        }

        animationTimer -= Time.deltaTime;

        if (animationTimer < 0)
        {
            animationTimer = animationTotTime;
            cg.jumpEngageActive = false;
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.RUNNING;
        }
        
    }
    private void RotateTowardsPlayer(BaseManager agent)
    {
        Vector3 direction = (agent.targetedPlayer.position - agent.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        Quaternion offsetRotation = Quaternion.Euler(0, 10, 0f);
        Quaternion finalRotation = lookRotation * offsetRotation;
        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, finalRotation, Time.deltaTime * agent.navigation.rotationSpeed);
    }

    private void SetAnimation(BaseManager agent)
    {
        agent.animator.SetBool("CyberGiant_JumpEngage", true);
        agent.animator.SetBool("CyberGiant_MissileRain", false);
        agent.animator.SetBool("CyberGiant_Walk", false);
        agent.animator.SetBool("CyberGiant_OverheadSmash1", false);
        agent.animator.SetBool("CyberGiant_OverheadSmash2", false);
        agent.animator.SetBool("CyberGiant_Idle", false);
    }
}
