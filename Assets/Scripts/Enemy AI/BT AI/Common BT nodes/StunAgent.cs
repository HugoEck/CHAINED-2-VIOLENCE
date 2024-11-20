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

        if (Time.time >= agent.chainEffects.stunStartTime + agent.chainEffects.stunDurationTime)
        {
            if (agent.chainEffects.stunType == "Ragdoll")
            {
                agent.behaviorMethods.ToggleRagdoll(false);
            }
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

    private void SetAnimation(BaseManager agent)
    {
        if (agent.chainEffects.stunType == "Ghost")
        {
            //IMPLEMENTERA GHOST ANIMATION HÄR
        }

        if (agent.chainEffects.stunType == "Shock")
        {
            //IMPLEMENTERA SHOCK ANIMATION HÄR
        }

        //STÄNG AV ALLA ANDRA ANIMATIONER
    }
}


