using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : Node
{

    public override NodeState Evaluate(BaseManager agent)
    {
        SetAttackAnimation(agent);

        targetedPlayer = agent.CalculateClosestTarget();

        Player playerManager = agent.GetCorrectPlayerManager(targetedPlayer);

        if (agent.IsAttackAllowed())
        {
            playerManager.SetHealth(agent.attack);
        }

        return NodeState.RUNNING;
    }


    public void SetAttackAnimation(BaseManager agent)
    {
        if (agent.enemyID == "Plebian")
        {
            agent.animator.SetBool("Plebian_Chase", false);
            agent.animator.SetBool("Plebian_Attack", true);
        }
        else if (agent.enemyID == "Runner")
        {
            agent.animator.SetBool("Runner_Chase", false);
            agent.animator.SetBool("Runner_Attack", true);
        }
        
    }
}
