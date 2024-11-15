using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChaseConditions : Node
{

    float distance;
    public override NodeState Evaluate(BaseManager agent)
    {

        CyberGiantManager cg = agent as CyberGiantManager;
        ChooseAbility(cg);

        agent.targetedPlayer = agent.behaviorMethods.CalculateClosestTarget();
        distance = Vector3.Distance(agent.transform.position, agent.targetedPlayer.position);

        if (distance > agent.attackRange)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }

    }

    private void ChooseAbility(CyberGiantManager cg)
    {
        if (distance > cg.maxMidRangeDistance)
        {
            cg.shieldWalkActive = true;
        }
        else
        {
            cg.shieldWalkActive = false;
        }
    }
}