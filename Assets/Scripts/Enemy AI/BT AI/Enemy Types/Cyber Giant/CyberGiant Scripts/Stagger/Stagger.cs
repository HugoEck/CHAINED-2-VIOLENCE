using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stagger : Node
{

    float animationTotTime = 6f;
    float animationTimer = 6f;

    public override NodeState Evaluate(BaseManager agent)
    {
        CyberGiantManager cg = agent as CyberGiantManager;

        cg.energyShield.SetActive(true);
        cg.damageCollider.radius = 1.4f;
        cg.c_collider.radius = 1.4f;
        cg.defense = cg.energyShieldDefense;


        SetAnimation(agent);
        agent.navigation.isStopped = true;


        animationTimer -= Time.deltaTime;

        if (animationTimer < 0)
        {

            cg.energyShield.SetActive(false);
            cg.damageCollider.radius = 0.75f;
            cg.c_collider.radius = 0.75f;
            cg.defense = cg.baseDefense;
            animationTimer = animationTotTime;
            cg.staggerActive = false;

            return NodeState.SUCCESS;

        }
        else
        {
            return NodeState.RUNNING;
        }

    }

    private void SetAnimation(BaseManager agent)
    {
        agent.animator.SetBool("CyberGiant_Stagger", true);
        agent.animator.SetBool("CyberGiant_MissileRain", false);
        agent.animator.SetBool("CyberGiant_Walk", false);
        agent.animator.SetBool("CyberGiant_ShieldWalk", false);
        agent.animator.SetBool("CyberGiant_JumpEngage", false);
        agent.animator.SetBool("CyberGiant_Idle", false);
        agent.animator.SetBool("CyberGiant_OverheadSmash1", false);
        agent.animator.SetBool("CyberGiant_OverheadSmash2", false);
        agent.animator.SetBool("CyberGiant_Death", false);

    }
}
