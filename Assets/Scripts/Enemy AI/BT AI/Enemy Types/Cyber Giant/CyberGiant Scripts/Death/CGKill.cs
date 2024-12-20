using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGKill : Node
{

    float deathTimer = 10;
    bool runOnce = false;

    public override NodeState Evaluate(BaseManager agent)
    {
        CyberGiantManager cg = agent as CyberGiantManager;

        cg.deathActive = true;
        agent.navigation.isStopped = true;

        SetAnimation(agent);

        deathTimer -= Time.deltaTime;

        if (deathTimer < 2.5f && runOnce == false)
        {
            cg.effectPrefab.SetActive(false);
            if (GoldDropManager.Instance != null)
            {
                GoldDropManager.Instance.AddGold(agent.unitCost);
            }

            runOnce = true;
        }


        if(deathTimer < 0)
        {
            WaveManager.ActiveEnemies--;
            GameObject.Destroy(agent.gameObject);
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.RUNNING;
        }


    }

    private void SetAnimation(BaseManager agent)
    {
        agent.animator.SetBool("CyberGiant_Death", true);
        agent.animator.SetBool("CyberGiant_Idle", false);
        agent.animator.SetBool("CyberGiant_MissileRain", false);
        agent.animator.SetBool("CyberGiant_Walk", false);
        agent.animator.SetBool("CyberGiant_ShieldWalk", false);
        agent.animator.SetBool("CyberGiant_JumpEngage", false);
        agent.animator.SetBool("CyberGiant_OverheadSmash1", false);
        agent.animator.SetBool("CyberGiant_OverheadSmash2", false);
        agent.animator.SetBool("CyberGiant_Stagger", false);

    }
}
