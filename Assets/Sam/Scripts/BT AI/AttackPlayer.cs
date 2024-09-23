using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : Node
{
    //private float damage;
    //private float attackSpeed;

    //public AttackPlayer(BaseManager agent)
    //{
    //    //damage = agent.damage;
    //    //attackSpeed = agent.attackSpeed;
        
    //}

    public override NodeState Evaluate(BaseManager agent)
    {
        targetedPlayer = agent.CalculateClosestTarget();
        agent.AttackPlayer(targetedPlayer);
        
        return NodeState.RUNNING;
    }
}
