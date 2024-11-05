using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeComplete : Node
{

    public override NodeState Evaluate(BaseManager agent)
    {

        ChargerManager charger = agent as ChargerManager;
        if (charger.hasAlreadyCharged == true)
        {

            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }
}
