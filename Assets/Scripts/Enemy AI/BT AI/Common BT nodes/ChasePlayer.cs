using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI;
using UnityEngine.AI;

public class ChasePlayer : Node
{
    

    public override NodeState Evaluate(BaseManager agent)
    {
        SetChaseAnimation(agent);

        agent.targetedPlayer = agent.behaviorMethods.CalculateClosestTarget();
        float distance = Vector3.Distance(agent.transform.position, agent.targetedPlayer.transform.position);
        
        if (distance > agent.attackRange)
        {
            agent.navigation.isStopped = false;
            agent.navigation.destination = agent.targetedPlayer.position;
            
            return NodeState.RUNNING;
        }
        
        else
        {
            agent.navigation.isStopped = true;
            return NodeState.SUCCESS;
        }

        
    }

    public void SetChaseAnimation(BaseManager agent)
    {
        if (agent.enemyID=="Plebian")
        {
            agent.animator.SetBool("Plebian_Chase", true);
            agent.animator.SetBool("Plebian_Attack", false);
            agent.animator.SetBool("Plebian_Electrocute", false);
            agent.animator.SetBool("Plebian_Scared", false);
        }
        else if (agent.enemyID == "Runner")
        {
            agent.animator.SetBool("Runner_Chase", true);
            agent.animator.SetBool("Runner_Attack", false);
            agent.animator.SetBool("Runner_Electrocute", false);
            agent.animator.SetBool("Runner_Scared", false);
        }
        else if (agent.enemyID == "Swordsman")
        {
            agent.animator.SetBool("Swordsman_Chase", true);
            agent.animator.SetBool("Swordsman_Attack", false);
            agent.animator.SetBool("Swordsman_Electrocute", false);
            agent.animator.SetBool("Swordsman_Scared", false);
        }
        else if(agent.enemyID == "RockThrower")
        {
            agent.animator.SetBool("RockThrower_Chase", true);
            agent.animator.SetBool("RockThrower_Attack", false);
            agent.animator.SetBool("RockThrower_Idle", false);
            agent.animator.SetBool("RockThrower_Electrocute", false);
            agent.animator.SetBool("RockThrower_Scared", false);
        }
        else if(agent.enemyID == "Charger")
        {
            agent.animator.SetBool("Charger_Chase", true);
            agent.animator.SetBool("Charger_Attack", false);
            agent.animator.SetBool("Charger_Sprint", false);
        }
        else if (agent.enemyID == "Bomber")
        {
            agent.animator.SetBool("Bomber_Chase", true);
            agent.animator.SetBool("Bomber_Idle", false);
            agent.animator.SetBool("Bomber_Sprint", false);
            agent.animator.SetBool("Bomber_Electrocute", false);
            agent.animator.SetBool("Bomber_Scared", false);
        }



    }
}
