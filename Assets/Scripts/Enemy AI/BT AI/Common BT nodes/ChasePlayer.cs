using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI;
using UnityEngine.AI;

public class ChasePlayer : Node
{
    

    public override NodeState Evaluate(BaseManager agent)
    {
        agent.navigation.rotationSpeed = 360;
        SetAnimation(agent);
       
        agent.targetedPlayer = agent.behaviorMethods.CalculateClosestTarget();
        float distance = Vector3.Distance(agent.transform.position, agent.targetedPlayer.transform.position);
        
        if (distance > agent.attackRange)
        {
            agent.navigation.isStopped = false;
            agent.navigation.destination = agent.targetedPlayer.position;
            
            return NodeState.RUNNING;
        }
        
        else //Denna kan aldrig bli true eftersom detta �r sista BT branch
        {
            agent.navigation.isStopped = true;

            return NodeState.SUCCESS;
        }

        
    }

    private void RotateTowardsPlayer(BaseManager agent, Transform targetedPlayer)
    {
        // Calculate the direction from the agent to the player
        Vector3 direction = (targetedPlayer.position - agent.transform.position).normalized;

        // Calculate the target rotation to face the player
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        // Smoothly rotate the agent towards the player
        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, lookRotation, Time.deltaTime * agent.navigation.rotationSpeed);
    }

    public void SetAnimation(BaseManager agent)
    {
        if (agent.enemyID=="Plebian")
        {
            agent.animator.SetBool("Plebian_Chase", true);
            agent.animator.SetBool("Plebian_Attack", false);
        }
        else if (agent.enemyID == "Runner")
        {
            agent.animator.SetBool("Runner_Chase", true);
            agent.animator.SetBool("Runner_Attack", false);
        }
        else if (agent.enemyID == "Swordsman")
        {
            agent.animator.SetBool("Swordsman_Chase", true);
            agent.animator.SetBool("Swordsman_Attack", false);
        }
        else if(agent.enemyID == "RockThrower")
        {
            agent.animator.SetBool("RockThrower_Chase", true);
            agent.animator.SetBool("RockThrower_Attack", false);
            agent.animator.SetBool("RockThrower_Idle", false);
        }
        else if(agent.enemyID == "Charger")
        {
            agent.animator.SetBool("Charger_Chase", true);
            agent.animator.SetBool("Charger_Attack", false);
            agent.animator.SetBool("Charger_Sprint", false);
        }
    }
}
