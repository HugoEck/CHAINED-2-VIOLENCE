using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : Node
{

    public override NodeState Evaluate(BaseManager agent)
    {
        SetAttackAnimation(agent);

        agent.targetedPlayer = agent.behaviorMethods.CalculateClosestTarget();

        Player playerManager = agent.behaviorMethods.GetCorrectPlayerManager(agent.targetedPlayer);

        if (agent.behaviorMethods.IsAttackAllowed())
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
        else if (agent.enemyID == "Swordsman")
        {
            agent.animator.SetBool("Swordsman_Chase", false);
            agent.animator.SetBool("Swordsman_Attack", true);
        }
        else if(agent.enemyID == "Charger")
        {
            agent.animator.SetBool("Charger_Chase", false);
            agent.animator.SetBool("Charger_Attack", true);
            agent.animator.SetBool("Charger_Sprint", false);
        }
        
    }
}
