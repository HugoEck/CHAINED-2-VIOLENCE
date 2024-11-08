using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IsBombInRange : Node
{
    private float distance;
    public override NodeState Evaluate(BaseManager agent)
    {
        CyberGiantManager cg = agent as CyberGiantManager;

        agent.targetedPlayer = agent.behaviorMethods.CalculateClosestTarget();
        distance = Vector3.Distance(agent.transform.position, agent.targetedPlayer.position);

        if (distance < cg.minBombDistance)
        {
            return NodeState.FAILURE;
        }
        else
        {
            return NodeState.SUCCESS;
        }
        
    }
}