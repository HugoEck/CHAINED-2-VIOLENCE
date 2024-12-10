using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IfShieldAttackChosen : Node
{
    public override NodeState Evaluate(BaseManager agent)
    {

        BulwarkKnightManager bk = agent as BulwarkKnightManager;

        if (bk.shieldBroken == false)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }

    }
}
