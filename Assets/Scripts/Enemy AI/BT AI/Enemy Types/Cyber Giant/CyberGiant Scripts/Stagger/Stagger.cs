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

        agent.defense = cg.energyShieldDefense;
        cg.energyShield.SetActive(true);




        SetAnimation(agent);
        agent.navigation.isStopped = true;


        if (animationTimer > 3f)
        {
            //animation1

        }
        else if (animationTimer < 3f && animationTimer > 0f)
        {
            //animation2
        }


        animationTimer -= Time.deltaTime;

        if (animationTimer < 0)
        {
            animationTimer = animationTotTime;
            cg.staggerActive = false;
            agent.defense = cg.baseDefense;
            cg.energyShield.SetActive(false);
            return NodeState.SUCCESS;

        }
        else
        {
            return NodeState.RUNNING;
        }

    }

    private void SetAnimation(BaseManager agent)
    {
        agent.animator.SetBool("CyberGiant_MissileRain", false);
        agent.animator.SetBool("CyberGiant_Walk", false);
        agent.animator.SetBool("CyberGiant_ShieldWalk", false);
        agent.animator.SetBool("CyberGiant_JumpEngage", false);
        agent.animator.SetBool("CyberGiant_Idle", false);

    }
}
