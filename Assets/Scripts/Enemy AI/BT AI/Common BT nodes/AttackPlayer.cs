using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : Node
{
    public override NodeState Evaluate(BaseManager agent)
    {

        SetAnimation(agent);

        agent.behaviorMethods.RotateTowardsClosestPlayer();

        agent.targetedPlayer = agent.behaviorMethods.CalculateClosestTarget();

        Player playerManager = agent.behaviorMethods.GetCorrectPlayerManager(agent.targetedPlayer);

        if (agent.behaviorMethods.IsAttackAllowed())
        {
            SetAttackAnimation(agent, "enable");

            playerManager.SetHealth(agent.attack);
        }
        else
        {
            SetAttackAnimation(agent, "disable");
        }

        return NodeState.RUNNING;
    }

    private void SetAnimation(BaseManager agent)
    {
        if (agent.enemyID == "Plebian")
        {
            agent.animator.SetBool("Plebian_Chase", false);
            agent.animator.SetBool("Plebian_Electrocute", false);
            agent.animator.SetBool("Plebian_Scared", false);
        }
        else if (agent.enemyID == "Runner")
        {
            agent.animator.SetBool("Runner_Chase", false);
            agent.animator.SetBool("Runner_Electrocute", false);
            agent.animator.SetBool("Runner_Scared", false);
        }
        else if (agent.enemyID == "Swordsman")
        {
            agent.animator.SetBool("Swordsman_Chase", false);
            agent.animator.SetBool("Swordsman_Electrocute", false);
            agent.animator.SetBool("Swordsman_Scared", false);
        }
        else if(agent.enemyID == "Charger")
        {
            agent.animator.SetBool("Charger_Chase", false);
            agent.animator.SetBool("Charger_Prepare", false);
            agent.animator.SetBool("Charger_Sprint", false);
        }
        
    }
    private void SetAttackAnimation(BaseManager agent, string type)
    {
        if (agent.enemyID == "Plebian")
        {
            if (type == "enable")
            {
                agent.animator.SetBool("Plebian_Idle", false);
                agent.animator.SetBool("Plebian_Attack", true);
            }
            else if (type == "disable")
            {
                agent.animator.SetBool("Plebian_Idle", true);
                agent.animator.SetBool("Plebian_Attack", false);
            }
            
        }
        else if (agent.enemyID == "Runner")
        {
            if (type == "enable")
            {
                agent.animator.SetBool("Runner_Idle", false);
                agent.animator.SetBool("Runner_Attack", true);
            }
            else if (type == "disable")
            {
                agent.animator.SetBool("Runner_Idle", true);
                agent.animator.SetBool("Runner_Attack", false);
            }
        }
        else if (agent.enemyID == "Swordsman")
        {
            if (type == "enable")
            {
                agent.animator.SetBool("Swordsman_Idle", false);
                agent.animator.SetBool("Swordsman_Attack", true);
            }
            else if (type == "disable")
            {
                agent.animator.SetBool("Swordsman_Idle", true);
                agent.animator.SetBool("Swordsman_Attack", false);
            }
        }
        else if (agent.enemyID == "Charger")
        {
            if (type == "enable")
            {
                agent.animator.SetBool("Charger_Idle", false);
                agent.animator.SetBool("Charger_Attack", true);
            }
            else if (type == "disable")
            {
                agent.animator.SetBool("Charger_Idle", true);
                agent.animator.SetBool("Charger_Attack", false);
            }
        }
    }
}
