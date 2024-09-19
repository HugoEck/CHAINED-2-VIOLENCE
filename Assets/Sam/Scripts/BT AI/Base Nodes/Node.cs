using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public abstract class Node 
{
    protected NodeState _nodeState;
    protected Vector3 currentPosition;
    protected Transform targetedPlayer;
    protected int test;
    public NodeState nodeState {  get { return _nodeState; } }

    public abstract NodeState Evaluate(BaseManager agent);
}

public enum NodeState
{
    RUNNING, SUCCESS, FAILURE,
}