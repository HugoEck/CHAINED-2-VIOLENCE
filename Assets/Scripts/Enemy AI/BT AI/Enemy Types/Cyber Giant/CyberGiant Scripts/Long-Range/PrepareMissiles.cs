using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareMissiles : Node
{
    private float prepDurationTime = 3.5f;
    private float currentTime = 0;


    public override NodeState Evaluate(BaseManager agent)
    {



        agent.navigation.isStopped = true;

        agent.animator.SetBool("CyberGiant_PrepareMissiles", true);
        agent.animator.SetBool("CyberGiant_Walk", false);

        CyberGiantManager cg = agent as CyberGiantManager;

        
        cg.missilePrepActivated = true;

        cg.currentTime += Time.deltaTime;

        cg.p1_LastPosition = agent.player1.transform.position;
        cg.p2_LastPosition = agent.player2.transform.position;
        cg.chain_LastPosition = agent.CalculateChainPosition();
        cg.chain_LastPosition = new Vector3(cg.chain_LastPosition.x, 0, cg.chain_LastPosition.z);

        RotateTowardsChain(agent, cg.chain_LastPosition);


        if (cg.currentTime > prepDurationTime)
        {
            cg.missilePrepActivated = false;
            cg.missileSent = true;
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
}
