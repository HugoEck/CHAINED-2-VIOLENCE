using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class KillAgent : Node
{


    public override NodeState Evaluate(BaseManager agent)
    {
        GameObject.Destroy(agent.gameObject);

        // DETTA KOMMER ATT ÄNDRAS TILL RUNNING IFALL VI VILL HA EN ANIMATION.
        return NodeState.SUCCESS;
    }
}
