using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : Node
{
    private int damage;

    public AttackPlayer(BaseManager agent)
    {
        damage = agent.damage;
        targetedPlayer = agent.CalculateClosestTarget();

    }

    public override NodeState Evaluate(BaseManager agent)
    {
        agent.transform.localScale = new Vector3(3,3,3);
        return NodeState.RUNNING;
    }
}
