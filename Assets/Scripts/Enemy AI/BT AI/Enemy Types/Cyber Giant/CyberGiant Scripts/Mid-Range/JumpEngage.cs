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
        agent.animator.SetBool("CyberGiant_JumpEngage", true);
        agent.animator.SetBool("CyberGiant_PrepareMissiles", false);
        agent.animator.SetBool("CyberGiant_Walk", false);

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
            //jumpDirection = agent.targetedPlayer.position.normalized;
            jumpDirection = (agent.targetedPlayer.position - agent.transform.position).normalized;
        }
        else if (animationTimer < 3.5f && animationTimer > 2.5f)
        {
            if(distance > cg.maxCloseRangeDistance)
            {
                RotateTowardsPlayer(agent);
                agent.transform.position += jumpDirection * jumpSpeed * Time.deltaTime;
            }

        }

        animationTimer -= Time.deltaTime;
        //Debug.Log(animationTimer);
        if (animationTimer < 0)
        {
            animationTimer = animationTotTime;
            cg.JumpEngageActive = false;
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.RUNNING;
        }
        
    }

    //private void RotateTowardsPlayer(BaseManager agent)
    //{
    //    Vector3 direction = (agent.targetedPlayer.position - agent.transform.position).normalized;

    //    Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

    //    agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, lookRotation, Time.deltaTime * agent.navigation.rotationSpeed);
    //}
    private void RotateTowardsPlayer(BaseManager agent)
    {
        // Calculate the direction vector to the targeted player
        Vector3 direction = (agent.targetedPlayer.position - agent.transform.position).normalized;

        // Calculate the rotation that looks at the targeted player
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        // Apply an additional rotation offset on the Z-axis
        Quaternion offsetRotation = Quaternion.Euler(0, 10, 0f); // Adjust -5f as needed for the tilt to the left

        // Combine the lookRotation with the offset
        Quaternion finalRotation = lookRotation * offsetRotation;

        // Smoothly rotate towards the final rotation with a Slerp
        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, finalRotation, Time.deltaTime * agent.navigation.rotationSpeed);
    }
}
