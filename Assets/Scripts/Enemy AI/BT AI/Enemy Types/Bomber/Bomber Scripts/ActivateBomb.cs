using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActivateBomb : Node
{
    public override NodeState Evaluate(BaseManager agent)
    {
        BomberManager bomber = agent as BomberManager;
        agent.navigation.isStopped = true;

        //Debug.Log("Entered Idle!");

        if (bomber.bombAnimationTimer < 5)
        {
            bomber.idleActive = false;
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.RUNNING;
        }
    }
}
