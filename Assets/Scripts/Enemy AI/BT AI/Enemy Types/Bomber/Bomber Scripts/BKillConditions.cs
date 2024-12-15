using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BKillConditions : Node
{
    public override NodeState Evaluate(BaseManager agent)
    {

        BomberManager bomber = agent as BomberManager;

        if (agent.currentHealth <= 0 || bomber.bombExploded)
        {
            ChooseKillPath(bomber);
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }


    }

    private void ChooseKillPath(BomberManager bomber)
    {

        if (bomber.bombActivated)
        {
            bomber.deathAfterActivation = true;
        }
        else
        {
            bomber.deathBeforeActivation = true;
        }
    }
}
