using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class KillAgent : Node
{


    public override NodeState Evaluate(BaseManager agent)
    {

        if (agent.SetTimer(5))
        {
            GameObject.Destroy(agent.gameObject);
        }

        return NodeState.RUNNING;

        // DETTA KOMMER ATT ÄNDRAS TILL RUNNING IFALL VI VILL HA EN ANIMATION.
        //return NodeState.SUCCESS;
    }
}
