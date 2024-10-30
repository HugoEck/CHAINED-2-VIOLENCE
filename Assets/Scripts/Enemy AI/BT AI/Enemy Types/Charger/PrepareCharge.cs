using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareCharge : Node
{
    
   public override NodeState Evaluate(BaseManager agent)
    {

        ChargerManager charger = agent as ChargerManager;

        if (charger.prepareChargeComplete == false)
        {
            agent.animator.SetBool("Charger_Prepare", true);
            agent.animator.SetBool("Charger_Chase", false);


            charger.activatePrepareChargeTimer = true;
            agent.navigation.isStopped = true;

            agent.targetedPlayer = agent.CalculateClosestTarget();
            agent.chainPosition = agent.CalculateChainPosition();
            RotateTowardsChain(agent, agent.chainPosition );
            charger.lastSavedPosition = new Vector3 (agent.transform.position.x, 0, agent.transform.position.z);


            return NodeState.RUNNING;
        }
        else
        {
            charger.prepareChargeComplete = true;
            return NodeState.SUCCESS;
        }
        
    }

    public Vector3 CalculateChainPosition(BaseManager agent)
    {

        Vector3 p1Position = agent.player1.transform.position;
        Vector3 p2Position = agent.player2.transform.position;
        Vector3 midPoint = (p1Position + p2Position) / 2;
        midPoint.y = 0;
        return midPoint;
    }

    private void RotateTowardsChain(BaseManager agent, Vector3 chainPosition)
    {
        Vector3 direction = (chainPosition - agent.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, lookRotation, Time.deltaTime * agent.navigation.rotationSpeed);
    }
}
