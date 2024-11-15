using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StaggerConditions : Node
{

    private bool stagger1Activated = false;
    private bool stagger2Activated = false;
    private bool stagger3Activated = false;


    public override NodeState Evaluate(BaseManager agent)
    {

        CyberGiantManager cg = agent as CyberGiantManager;

        if (cg.staggerActive)
        {
            return NodeState.SUCCESS;
        }


        if (agent.currentHealth < agent.maxHealth * )
        
        return NodeState.SUCCESS;
    }
    
}
