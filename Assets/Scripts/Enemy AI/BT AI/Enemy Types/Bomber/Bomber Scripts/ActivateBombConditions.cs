using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ActivateBombConditions : Node
{
    float distance;
    bool runOnce = false;
    public override NodeState Evaluate(BaseManager agent)
    {
        BomberManager bomber = agent as BomberManager;

        agent.targetedPlayer = agent.behaviorMethods.CalculateClosestTarget();
        distance = Vector3.Distance(agent.transform.position, agent.targetedPlayer.transform.position);

        if (distance < agent.attackRange && !runOnce)
        {
            runOnce = true;
            bomber.idleActive = true;
            return NodeState.SUCCESS;
        }
        else if (bomber.idleActive)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }
}
