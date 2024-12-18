using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StunAgent : Node
{
    private bool isStunInitialized = false;


    public override NodeState Evaluate(BaseManager agent)
    {

        agent.navigation.isStopped = true;
        agent.chainEffects.stunActivated = true;
        SetAnimation(agent);

        if (!isStunInitialized)
        {
            agent.chainEffects.stunStartTime = Time.time;
            isStunInitialized = true;
        }

        if (Time.time >= agent.chainEffects.stunStartTime + agent.chainEffects.stunDurationTime)
        {
            if (agent.chainEffects.stunType == "Ragdoll")
            {
                agent.behaviorMethods.ToggleRagdoll(false);
            }

            if (SceneManager.GetActiveScene().name != "SamTestScene")
            {
                agent.transform.position = new Vector3(agent.transform.position.x, 1.5f, agent.transform.position.z);
            }
            agent.chainEffects.stunActivated = false;
            agent.navigation.isStopped = false;
            isStunInitialized = false;

            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.RUNNING;
        }
    }

    private void SetAnimation(BaseManager agent)
    {
        if (agent.enemyID == "Plebian")
        {
            SetAnimationPlebian(agent);
        }
        else if (agent.enemyID == "Runner")
        {
            SetAnimationRunner(agent);
        }
        else if (agent.enemyID == "RockThrower")
        {
            SetAnimationRockThrower(agent);
        }
        else if (agent.enemyID == "Swordsman")
        {
            SetAnimationSwordsman(agent);
        }
        else if (agent.enemyID == "BannerMan")
        {
            SetAnimationBannerMan(agent);
        }
        else if (agent.enemyID == "BulwarkKnight")
        {
            SetAnimationBulwarkKnight(agent);
        }
        else if (agent.enemyID == "Bomber")
        {
            SetAnimationBomber(agent);
        }
    }

    private void SetAnimationPlebian(BaseManager agent)
    {
        if (agent.chainEffects.stunType == "Ghost")
        {
            agent.animator.SetBool("Plebian_Scared", true);
            agent.animator.SetBool("Plebian_Electrocute", false);
        }

        else if (agent.chainEffects.stunType == "Shock")
        {
            agent.animator.SetBool("Plebian_Electrocute", true);
            agent.animator.SetBool("Plebian_Scared", false);
        }

        agent.animator.SetBool("Plebian_Attack", false);
        agent.animator.SetBool("Plebian_Chase", false);
        agent.animator.SetBool("Plebian_Idle", false);
    }

    private void SetAnimationRunner(BaseManager agent)
    {
        if (agent.chainEffects.stunType == "Ghost")
        {
            agent.animator.SetBool("Runner_Scared", true);
            agent.animator.SetBool("Runner_Electrocute", false);
        }

        else if (agent.chainEffects.stunType == "Shock")
        {
            agent.animator.SetBool("Runner_Scared", false);
            agent.animator.SetBool("Runner_Electrocute", true);
        }

        agent.animator.SetBool("Runner_Attack", false);
        agent.animator.SetBool("Runner_Chase", false);
        agent.animator.SetBool("Runner_Idle", false);
    }

    private void SetAnimationRockThrower(BaseManager agent)
    {
        if (agent.chainEffects.stunType == "Ghost")
        {
            agent.animator.SetBool("RockThrower_Scared", true);
            agent.animator.SetBool("RockThrower_Electrocute", false);
        }

        else if (agent.chainEffects.stunType == "Shock")
        {
            agent.animator.SetBool("RockThrower_Scared", false);
            agent.animator.SetBool("RockThrower_Electrocute", true);
        }

        agent.animator.SetBool("RockThrower_Attack", false);
        agent.animator.SetBool("RockThrower_Chase", false);
        agent.animator.SetBool("RockThrower_Idle", false);
    }
    private void SetAnimationSwordsman(BaseManager agent)
    {
        if (agent.chainEffects.stunType == "Ghost")
        {
            agent.animator.SetBool("Swordsman_Scared", true);
            agent.animator.SetBool("Swordsman_Electrocute", false);
        }

        else if (agent.chainEffects.stunType == "Shock")
        {
            agent.animator.SetBool("Swordsman_Scared", false);
            agent.animator.SetBool("Swordsman_Electrocute", true);
        }

        agent.animator.SetBool("Swordsman_Attack", false);
        agent.animator.SetBool("Swordsman_Chase", false);
        agent.animator.SetBool("Swordsman_Idle", false);

    }
    private void SetAnimationBannerMan(BaseManager agent)
    {
        if (agent.chainEffects.stunType == "Ghost")
        {
            agent.animator.SetBool("BannerMan_Scared", true);
            agent.animator.SetBool("BannerMan_Electrocute", false);
        }

        else if (agent.chainEffects.stunType == "Shock")
        {
            agent.animator.SetBool("BannerMan_Scared", false);
            agent.animator.SetBool("BannerMan_Electrocute", true);
        }

        agent.animator.SetBool("BannerMan_Running", false);
    }

    private void SetAnimationBulwarkKnight(BaseManager agent)
    {
        if (agent.chainEffects.stunType == "Ghost")
        {
            agent.animator.SetBool("BulwarkKnight_Scared", true);
            agent.animator.SetBool("BulwarkKnight_Electrocute", false);
        }

        else if (agent.chainEffects.stunType == "Shock")
        {
            agent.animator.SetBool("BulwarkKnight_Scared", false);
            agent.animator.SetBool("BulwarkKnight_Electrocute", true);
        }

        agent.animator.SetBool("BulwarkKnight_ShieldWalk", false);
        agent.animator.SetBool("BulwarkKnight_SwordRun", false);
        agent.animator.SetBool("BulwarkKnight_ShieldIdle", false);
        agent.animator.SetBool("BulwarkKnight_SwordIdle", false);
        agent.animator.SetBool("BulwarkKnight_ShieldAttack", false);
        agent.animator.SetBool("BulwarkKnight_SwordAttack", false);
        agent.animator.SetBool("BulwarkKnight_Rage", false);

    }

    private void SetAnimationBomber(BaseManager agent)
    {
        if (agent.chainEffects.stunType == "Ghost")
        {
            agent.animator.SetBool("Bomber_Scared", true);
            agent.animator.SetBool("Bomber_Electrocute", false);
        }

        else if (agent.chainEffects.stunType == "Shock")
        {
            agent.animator.SetBool("Bomber_Scared", false);
            agent.animator.SetBool("Bomber_Electrocute", true);
        }

        agent.animator.SetBool("Bomber_Chase", false);
        agent.animator.SetBool("Bomber_Sprint", false);
        agent.animator.SetBool("Bomber_Idle", false);

    }
}


