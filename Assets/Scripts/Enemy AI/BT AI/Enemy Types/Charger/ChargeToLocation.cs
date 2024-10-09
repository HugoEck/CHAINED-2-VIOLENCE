using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeToLocation : Node
{
    public override NodeState Evaluate(BaseManager agent)
    {
        return NodeState.SUCCESS;
    }
}
