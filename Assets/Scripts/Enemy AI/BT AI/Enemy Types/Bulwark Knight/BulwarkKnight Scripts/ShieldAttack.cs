using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAttack : Node
{


    float distance;

    public override NodeState Evaluate(BaseManager agent)
    {

        BulwarkKnightManager bk = agent as BulwarkKnightManager;

        SetAnimation(agent);
        RotateTowardsPlayer(agent);

        agent.targetedPlayer = agent.behaviorMethods.CalculateClosestTarget();
        Player playerManager = agent.behaviorMethods.GetCorrectPlayerManager(agent.targetedPlayer);

        if (agent.behaviorMethods.IsAttackAllowed())
        {
            agent.animator.SetBool("BulwarkKnight_ShieldAttack", true);
            agent.animator.SetBool("BulwarkKnight_Idle", false);
            //playerManager.SetHealth(agent.attack);
            Debug.Log("Player attacked");
        }
        else
        {
            agent.animator.SetBool("BulwarkKnight_ShieldAttack", false);
            agent.animator.SetBool("BulwarkKnight_Idle", true);
        }

        return NodeState.RUNNING;

    }
    private void SetAnimation(BaseManager agent)
    {
        agent.animator.SetBool("BulwarkKnight_ShieldWalk", false);
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
}
