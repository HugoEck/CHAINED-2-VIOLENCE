using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareMissiles : Node
{
    private float prepDurationTime = 3.5f;


    public override NodeState Evaluate(BaseManager agent)
    {
        agent.navigation.rotationSpeed = 360;
        agent.navigation.isStopped = true;

        SetAnimation(agent);
        CyberGiantManager cg = agent as CyberGiantManager;

        


        cg.currentTime += Time.deltaTime;

        cg.p1_LastPosition = agent.player1.transform.position;
        cg.p2_LastPosition = agent.player2.transform.position;
        cg.chain_LastPosition = agent.behaviorMethods.CalculateChainPosition();
        cg.chain_LastPosition = new Vector3(cg.chain_LastPosition.x, 0, cg.chain_LastPosition.z);

        RotateTowardsChain(agent, cg.chain_LastPosition);


        if (cg.currentTime > prepDurationTime)
        {
            cg.missileRainActive = false;
            //cg.missileSent = true;
            cg.currentTime = 0;
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.RUNNING;
        }

    }
    private void RotateTowardsChain(BaseManager agent, Vector3 chainPosition)
    {
        Vector3 direction = (chainPosition - agent.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, lookRotation, Time.deltaTime * agent.navigation.rotationSpeed);
    }

    private void SetAnimation(BaseManager agent)
    {
        agent.animator.SetBool("CyberGiant_MissileRain", true);
        agent.animator.SetBool("CyberGiant_Walk", false);
        agent.animator.SetBool("CyberGiant_ShieldWalk", false);
        agent.animator.SetBool("CyberGiant_JumpEngage", false);
        agent.animator.SetBool("CyberGiant_OverheadSmash1", false);
        agent.animator.SetBool("CyberGiant_OverheadSmash2", false);
        agent.animator.SetBool("CyberGiant_Idle", false);
    }
}
