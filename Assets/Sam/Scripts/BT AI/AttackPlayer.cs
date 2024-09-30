using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : Node
{

    public override NodeState Evaluate(BaseManager agent)
    {
        targetedPlayer = agent.CalculateClosestTarget();

        PlayerManager playerManager = agent.GetCorrectPlayerManager(targetedPlayer);

        if (agent.IsAttackAllowed())
        {
            playerManager.SetHealth(agent.damage);
        }

        return NodeState.RUNNING;
    }
}
