using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongRangeConditions : Node
{
    float distance;

    public override NodeState Evaluate(BaseManager agent)
    {

        CyberGiantManager cg = agent as CyberGiantManager;

        agent.targetedPlayer = agent.behaviorMethods.CalculateClosestTarget();
        distance = Vector3.Distance(agent.transform.position, agent.targetedPlayer.position);

        if (cg.missileRainActive) // || annan long-range ability)
        {

            return NodeState.SUCCESS;
        }
        else if (cg.CheckIfAbilityInProgress() == false && CheckLongRangeDistance(cg) && cg.IsLongRangeAbilityReady())
        {        
            ChooseAbility(cg);
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }

    }

    private bool CheckLongRangeDistance(CyberGiantManager cg)
    {
        if (distance > cg.minLongRangeDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ChooseAbility(CyberGiantManager cg)
    {
        int randomNr = Random.Range(0, 1);

        if (randomNr == 0)
        {
            cg.missileRainActive = true;
        }
    }
}
