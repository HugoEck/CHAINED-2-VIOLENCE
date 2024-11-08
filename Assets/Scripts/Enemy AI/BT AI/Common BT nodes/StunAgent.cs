using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunAgent : Node
{
    private bool isStunInitialized = false;


    public override NodeState Evaluate(BaseManager agent)
    {

        agent.navigation.isStopped = true;
        agent.chainEffects.stunActivated = true;

        if (!isStunInitialized)
        {
            agent.chainEffects.stunStartTime = Time.time;
            isStunInitialized = true;
        }

        // Check if the stun duration has elapsed
        if (Time.time >= agent.chainEffects.stunStartTime + agent.chainEffects.stunDurationTime)
        {
            agent.chainEffects.stunActivated = false;
            agent.navigation.isStopped = false;
            isStunInitialized = false;
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.RUNNING;
        }
    }
}


