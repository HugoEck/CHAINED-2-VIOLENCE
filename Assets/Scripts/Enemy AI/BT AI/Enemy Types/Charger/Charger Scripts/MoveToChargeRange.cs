using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToChargeRange : Node
{
    public override NodeState Evaluate(BaseManager agent)
    {

        

        ChargerManager charger = agent as ChargerManager;

        agent.targetedPlayer = agent.behaviorMethods.CalculateClosestTarget();
        float distance = Vector3.Distance(agent.transform.position, agent.targetedPlayer.transform.position);

        if (distance > charger.chargingRange && charger.isAlreadyCharging == false)
        {
            agent.animator.SetBool("Charger_Chase", true);

            agent.navigation.isStopped = false;
            agent.navigation.destination = agent.targetedPlayer.position;

            return NodeState.RUNNING;
        }

        else 
        {
            charger.isAlreadyCharging = true;
            agent.navigation.isStopped = true;
            return NodeState.FAILURE;
        }


    }

    
}
