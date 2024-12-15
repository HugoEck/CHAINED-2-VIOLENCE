using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : Node
{
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
            playerManager.SetHealth(agent.attack);
            //Debug.Log("BK attacked");
        }
        else
        {
            agent.animator.SetBool("BulwarkKnight_SwordAttack", false);
            agent.animator.SetBool("BulwarkKnight_Idle", true);
        }

        return NodeState.RUNNING;

    }
    private void SetAnimation(BaseManager agent)
    {
        agent.animator.SetBool("BulwarkKnight_ShieldWalk", false);
        agent.animator.SetBool("BulwarkKnight_SwordRun", false);
        agent.animator.SetBool("BulwarkKnight_ShieldAttack", false);

    }

    private void RotateTowardsPlayer(BaseManager agent)
    {
        Vector3 direction = (agent.targetedPlayer.position - agent.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, lookRotation, Time.deltaTime * agent.navigation.rotationSpeed);
    }
}
