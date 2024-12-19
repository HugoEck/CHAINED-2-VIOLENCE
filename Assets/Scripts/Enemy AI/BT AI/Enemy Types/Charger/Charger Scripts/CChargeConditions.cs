using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CChargeConditions : Node
{
    float distance;
    public override NodeState Evaluate(BaseManager agent)
    {
        ChargerManager charger = agent as ChargerManager;

        agent.targetedPlayer = agent.behaviorMethods.CalculateClosestTarget();
        distance = Vector3.Distance(agent.transform.position, agent.targetedPlayer.transform.position);

        if (!charger.prepareChargeActive &&!charger.chargeRunActive && !charger.prepareChargeComplete &&  distance < charger.chargingRange)
        {
            charger.prepareChargeActive = true;
            return NodeState.SUCCESS;
        }
        else if (charger.prepareChargeActive || charger.chargeRunActive)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }

    }
}
