using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChainEffects
{
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


}
