using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : Node
{

    public override NodeState Evaluate(BaseManager agent)
    {
        targetedPlayer = agent.CalculateClosestTarget();
        agent.AttackPlayer(targetedPlayer);
        
        return NodeState.RUNNING;
    }
}
