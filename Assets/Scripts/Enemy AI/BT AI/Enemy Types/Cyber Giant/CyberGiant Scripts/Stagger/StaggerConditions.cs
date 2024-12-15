using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StaggerConditions : Node
{

    private bool stagger1Activated = false;
    //private bool stagger2Activated = false;
    //private bool stagger3Activated = false;


    public override NodeState Evaluate(BaseManager agent)
    {

        CyberGiantManager cg = agent as CyberGiantManager;

        if (cg.staggerActive)
        {
            return NodeState.SUCCESS;
        }


        if (cg.CheckIfAbilityInProgress() == false && HealthCheck(agent) && stagger1Activated == false)
        {
            stagger1Activated = true;
            cg.staggerActive = true;
            return NodeState.SUCCESS;

        }
        else
        {
            return NodeState.FAILURE;
        }
        
    }

    private bool HealthCheck(BaseManager agent)
    {
        if (agent.currentHealth < agent.maxHealth * 0.5f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
