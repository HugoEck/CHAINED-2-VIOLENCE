using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class IsMissileReady : Node
{
    float distance;
    
    public override NodeState Evaluate(BaseManager agent)
    {

        CyberGiantManager cg = agent as CyberGiantManager;

        agent.targetedPlayer = agent.behaviorMethods.CalculateClosestTarget();
        distance = Vector3.Distance(agent.transform.position, agent.targetedPlayer.position);

        if (cg.missileRainActive) // || annan range ability)
        {
            
            return NodeState.SUCCESS;
        }
        else if (cg.CheckIfAbilityInProgress() == false && cg.IsLongRangeAbilityReady() && CheckLongRangeDistance(cg))
        {
            cg.abilityInProgress = true;
            cg.missileReady = false;
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }

    }

    public bool CheckLongRangeDistance(CyberGiantManager cg)
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
}
