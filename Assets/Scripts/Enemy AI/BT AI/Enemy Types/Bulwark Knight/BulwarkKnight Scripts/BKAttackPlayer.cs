using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BKAttackPlayer : Node
{
    public override NodeState Evaluate(BaseManager agent)
    {
        agent.navigation.isStopped = true;
        BulwarkKnightManager bk = agent as BulwarkKnightManager;

        SetAnimation(agent);
        agent.behaviorMethods.RotateTowardsClosestPlayer();

        agent.targetedPlayer = agent.behaviorMethods.CalculateClosestTarget();
        agent.chosenPlayerManager = agent.behaviorMethods.GetCorrectPlayerManager(agent.targetedPlayer);

        if (agent.behaviorMethods.IsAttackAllowed())
        {
            SetAttackAnimation(agent, bk, "enable");
            //playerManager.SetHealth(agent.attack);
        }
        else
        {
            SetAttackAnimation(agent, bk, "disable");
        }

        return NodeState.RUNNING;

    }
    private void SetAnimation(BaseManager agent)
    {

        agent.animator.SetBool("BulwarkKnight_ShieldWalk", false);
        agent.animator.SetBool("BulwarkKnight_SwordRun", false);
        agent.animator.SetBool("BulwarkKnight_Electrocute", false);
        agent.animator.SetBool("BulwarkKnight_Scared", false);
        agent.animator.SetBool("BulwarkKnight_Rage", false);

    }

    private void SetAttackAnimation(BaseManager agent, BulwarkKnightManager bk, string type)
    {
        if (type == "enable")
        {
            agent.animator.SetBool("BulwarkKnight_ShieldIdle", false);
            agent.animator.SetBool("BulwarkKnight_SwordIdle", false);

            if (bk.shieldBroken)
            {
                agent.animator.SetBool("BulwarkKnight_SwordAttack", true);
                agent.animator.SetBool("BulwarkKnight_ShieldAttack", false);
            }
            else
            {
                agent.animator.SetBool("BulwarkKnight_SwordAttack", false);
                agent.animator.SetBool("BulwarkKnight_ShieldAttack", true);
            }
        }
        else if (type == "disable")
        {

            agent.animator.SetBool("BulwarkKnight_SwordAttack", false);
            agent.animator.SetBool("BulwarkKnight_ShieldAttack", false);

            if (bk.shieldBroken)
            {
                agent.animator.SetBool("BulwarkKnight_ShieldIdle", false);
                agent.animator.SetBool("BulwarkKnight_SwordIdle", true);
            }
            else
            {
                agent.animator.SetBool("BulwarkKnight_ShieldIdle", true);
                agent.animator.SetBool("BulwarkKnight_SwordIdle", false);
            }
        }
        else
        {
            Debug.Log("BK Attack animation error!");
        }
    }

    private void RotateTowardsPlayer(BaseManager agent)
    {
        Vector3 direction = (agent.targetedPlayer.position - agent.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, lookRotation, Time.deltaTime * agent.navigation.rotationSpeed);
    }
}
