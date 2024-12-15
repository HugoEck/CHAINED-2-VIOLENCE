using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class BKillNormalConditions : Node
{

    public override NodeState Evaluate(BaseManager agent)
    {

        BomberManager bomber = agent as BomberManager;

        if (bomber.deathBeforeActivation)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }

    }
}
