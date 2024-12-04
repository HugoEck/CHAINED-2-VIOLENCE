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

        //RotateTowardsPlayer(agent); Funkar inte med fixedEvaluate

        if (agent.behaviorMethods.IsAttackAllowed())
        {
            playerManager.SetHealth(agent.attack);
        }

        return NodeState.RUNNING;
    }

    private void RotateTowardsPlayer(BaseManager agent)
    {
        // Calculate the direction from the agent to the player
        Vector3 direction = (agent.targetedPlayer.position - agent.transform.position).normalized;

        // Calculate the target rotation to face the player
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        // Smoothly rotate the agent towards the player
        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, lookRotation, Time.deltaTime * agent.navigation.rotationSpeed);
    }
    public void SetAttackAnimation(BaseManager agent)
    {
        if (agent.enemyID == "Plebian")
        {
            agent.animator.SetBool("Plebian_Attack", true);
            agent.animator.SetBool("Plebian_Chase", false);
            agent.animator.SetBool("Plebian_Electrocute", false);
            agent.animator.SetBool("Plebian_Scared", false);
        }
        else if (agent.enemyID == "Runner")
        {
            agent.animator.SetBool("Runner_Chase", false);
            agent.animator.SetBool("Runner_Attack", true);
            agent.animator.SetBool("Runner_Electrocute", false);
            agent.animator.SetBool("Runner_Scared", false);
        }
        else if (agent.enemyID == "Swordsman")
        {
            agent.animator.SetBool("Swordsman_Chase", false);
            agent.animator.SetBool("Swordsman_Attack", true);
            agent.animator.SetBool("Swordsman_Electrocute", false);
            agent.animator.SetBool("Swordsman_Scared", false);
        }
        else if(agent.enemyID == "Charger")
        {
            agent.animator.SetBool("Charger_Chase", false);
            agent.animator.SetBool("Charger_Attack", true);
            agent.animator.SetBool("Charger_Sprint", false);
        }
        
    }
}
