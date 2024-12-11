using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class MidRangeConditions : Node
{
    float distance;

    public override NodeState Evaluate(BaseManager agent)
    {

        CyberGiantManager cg = agent as CyberGiantManager;


        agent.targetedPlayer = agent.behaviorMethods.CalculateClosestTarget();
        distance = Vector3.Distance(agent.transform.position, agent.targetedPlayer.position);

        if (cg.jumpEngageActive) // || andra mid-range abilites
        {

            return NodeState.SUCCESS;
        }
        else if (cg.CheckIfAbilityInProgress() == false && CheckMidRangeDistance(cg) && cg.IsMidRangeAbilityReady())
        {

            ChooseAbility(cg);

            return NodeState.SUCCESS;

        }
        else
        {
            return NodeState.FAILURE;
        }
    }
    private bool CheckMidRangeDistance(CyberGiantManager cg)
    {
        if (distance <= cg.maxMidRangeDistance && distance > cg.maxCloseRangeDistance)
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
            cg.jumpEngageActive = true;
        }
    }
}
