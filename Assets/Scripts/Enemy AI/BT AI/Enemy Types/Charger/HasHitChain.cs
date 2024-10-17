using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasHitChain : Node
{
    public override NodeState Evaluate(BaseManager agent)
    {
        return NodeState.SUCCESS;
    }
}
