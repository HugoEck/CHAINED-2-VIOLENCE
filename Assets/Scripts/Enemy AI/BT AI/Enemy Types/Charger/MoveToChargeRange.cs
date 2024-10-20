using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToChargeRange : Node
{
    public override NodeState Evaluate(BaseManager agent)
    {
        Debug.Log("Entered MoveToChargeRange");
        ChargerManager charger = agent as ChargerManager;

        

        targetedPlayer = agent.CalculateClosestTarget();
        float distance = Vector3.Distance(agent.transform.position, targetedPlayer.transform.position);

        if (distance > charger.chargingRange && charger.isAlreadyCharging == false)
        {
            
            agent.navigation.isStopped = false;
            agent.navigation.destination = targetedPlayer.position;

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
