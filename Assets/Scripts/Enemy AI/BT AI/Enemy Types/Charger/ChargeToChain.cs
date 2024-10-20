using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChargeToChain : Node
{

    public override NodeState Evaluate(BaseManager agent)
    {

        ChargerManager charger = agent as ChargerManager;


        if (charger.hasAlreadyCharged == false)
        {
            charger.activateChargingTimer = true;
            agent.navigation.isStopped = true;
            Vector3 direction = (charger.chainPosition - charger.lastSavedPosition).normalized;
            agent.transform.position += direction * charger.chargingSpeed * Time.deltaTime;

            float distance = Vector3.Distance(agent.transform.position, charger.chainPosition);
            return NodeState.RUNNING;
        }
        else
        {
            return NodeState.SUCCESS;
        }

    }
}
