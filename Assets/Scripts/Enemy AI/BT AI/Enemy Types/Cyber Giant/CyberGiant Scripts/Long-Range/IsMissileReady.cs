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

        agent.targetedPlayer = agent.CalculateClosestTarget();
        distance = Vector3.Distance(agent.transform.position, agent.targetedPlayer.position);

        if (cg.missilePrepActivated)
        {
            
            return NodeState.SUCCESS;
        }
        else if (cg.IsMissileReady() == true && distance > cg.minimumMissileDistance)
        {
            cg.missileSent = false;
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
        
     

    }
}
