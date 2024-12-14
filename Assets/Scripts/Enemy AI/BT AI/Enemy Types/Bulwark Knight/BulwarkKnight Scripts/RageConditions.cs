using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageConditions : Node
{
    private bool rageActivated = false;

    public override NodeState Evaluate(BaseManager agent)
    {
        BulwarkKnightManager bk = agent as BulwarkKnightManager;
        
        if (agent.currentHealth <= agent.maxHealth * 0.5f && !rageActivated)
        {
            rageActivated = true;
            bk.rageActive = true;

            return NodeState.SUCCESS;
        }
        else if (bk.rageActive)
        {

            return NodeState.SUCCESS;

        }
        else
        {
            return NodeState.FAILURE;
        }
    }
}
