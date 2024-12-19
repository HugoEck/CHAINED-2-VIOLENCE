using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotAlreadyCharged : Node
{

    public override NodeState Evaluate(BaseManager agent)
    {
        ChargerManager charger = agent as ChargerManager;

        if (charger.hasAlreadyCharged == false)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
        
    }
}
