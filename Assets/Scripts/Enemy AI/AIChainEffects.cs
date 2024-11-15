using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChainEffects
{

    BaseManager agent;

    public AIChainEffects(BaseManager manager)
    {
        agent = manager;
    }

    public float stunStartTime;
    public float stunDurationTime = 0;
    public bool stunActivated = false;
    public string stunType = null;

    public void ActivateGhostChainEffect(float durationTime)
    {
        stunActivated = true;
        stunDurationTime = durationTime;
        stunStartTime = Time.time;
        stunType = "Ghost";
    }

    public void ActivateShockChainEffect(float durationTime)
    {
        stunActivated = true;
        stunDurationTime = durationTime;
        stunStartTime = Time.time;
        stunType = "Shock";
    }

    public void ActivateRagdollStun(float durationTime)
    {
        stunActivated = true;
        stunDurationTime = durationTime;
        stunStartTime = Time.time;
        stunType = "Ragdoll";
        agent.behaviorMethods.ToggleRagdoll(true);
    }


}
