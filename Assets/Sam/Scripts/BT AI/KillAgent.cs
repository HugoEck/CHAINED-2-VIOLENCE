using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class KillAgent : Node
{
    BaseManager agent;


    //public KillAgent(BaseManager agent)
    //{
    //    //this.agent = agent;
    //}

    public override NodeState Evaluate(BaseManager agent)
    {
        GameObject.Destroy(agent.gameObject);

        return NodeState.SUCCESS;
    }
}
