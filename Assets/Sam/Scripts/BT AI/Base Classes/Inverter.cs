using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Inverter : Node
{
    protected List<Node> nodes = new List<Node>();

    public Inverter(List<Node> nodes)
    {
        this.nodes = nodes;
    }

    public override NodeState Evaluate(BaseManager agent)
    {

        foreach (Node node in nodes)
        {
            switch (node.Evaluate(agent))
            {
                case NodeState.RUNNING:
                    _nodeState = NodeState.RUNNING;
                    break;

                case NodeState.SUCCESS:
                    _nodeState = NodeState.FAILURE;
                    break;

                case NodeState.FAILURE:
                    _nodeState = NodeState.SUCCESS;
                    break;

                default:
                    break;
            }

        }
        _nodeState = NodeState.FAILURE;
        return _nodeState;
    }
}
