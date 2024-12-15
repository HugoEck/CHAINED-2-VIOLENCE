using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionConditions : Node
{
    public override NodeState Evaluate(BaseManager agent)
    {

        BomberManager bomber = agent as BomberManager;

        if (bomber.deathAfterActivation)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.SUCCESS;
        }


    }
}
