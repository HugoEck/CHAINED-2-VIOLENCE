using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class BKAttackConditions : Node
{

    float distance;

    public override NodeState Evaluate(BaseManager agent)
    {

        BulwarkKnightManager bk = agent as BulwarkKnightManager;

        return NodeState.SUCCESS;

    }
}
