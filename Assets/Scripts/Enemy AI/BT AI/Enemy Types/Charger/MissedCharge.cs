using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissedCharge : Node
{

    public override NodeState Evaluate(BaseManager agent)
    {
        return NodeState.SUCCESS;
    }
}
