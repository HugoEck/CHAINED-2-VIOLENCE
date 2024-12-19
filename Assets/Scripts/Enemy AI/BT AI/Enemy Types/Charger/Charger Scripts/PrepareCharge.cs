using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareCharge : Node
{
    
   public override NodeState Evaluate(BaseManager agent)
    {

        ChargerManager charger = agent as ChargerManager;


        if (charger.chargeRunActive)
        {
            return NodeState.SUCCESS;
        }

        SetAnimation(agent);

        charger.prepareChargeTimer -= Time.deltaTime;
        charger.lockTimer = false;

        agent.navigation.isStopped = true;

        agent.targetedPlayer = agent.behaviorMethods.CalculateClosestTarget();
        charger.chainPosition = agent.behaviorMethods.CalculateChainPosition();
        RotateTowardsChain(agent, charger.chainPosition);
        charger.lastSavedPosition = new Vector3(agent.transform.position.x, 0, agent.transform.position.z);


        if (charger.prepareChargeTimer < 0 )
        {
            charger.prepareChargeComplete = true;
            charger.prepareChargeActive = false;
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.RUNNING;
        }     
    }

    private void SetAnimation(BaseManager agent)
    {
        agent.animator.SetBool("Charger_Prepare", true);
        agent.animator.SetBool("Charger_Chase", false);
        agent.animator.SetBool("Charger_Attack", false);
        agent.animator.SetBool("Charger_Sprint", false);
        agent.animator.SetBool("Charger_Idle", false);
        agent.animator.SetBool("Charger_Electrocute", false);
        agent.animator.SetBool("Charger_Scared", false);
    }

    private void RotateTowardsChain(BaseManager agent, Vector3 chainPosition)
    {
        Vector3 direction = (chainPosition - agent.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, lookRotation, Time.deltaTime * agent.navigation.rotationSpeed);
    }
}
