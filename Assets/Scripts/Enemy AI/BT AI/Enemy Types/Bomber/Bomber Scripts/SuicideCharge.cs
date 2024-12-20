using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideCharge : Node
{
    private bool runOnce = false;
    public override NodeState Evaluate(BaseManager agent)
    {
        SetAnimation(agent);
        BomberManager bomber = agent as BomberManager;

        agent.navigation.isStopped = false;

        agent.navigation.maxSpeed = bomber.sprintSpeed;
        agent.navigation.isStopped = false;
        agent.targetedPlayer = agent.behaviorMethods.CalculateClosestTarget();
        agent.navigation.destination = agent.targetedPlayer.position;
        
        if (agent.audioClipManager.bomberManLaugh != null && agent.audioClipManager.bomberManFuse != null && !runOnce)
        {
            SFXManager.instance.PlaySFXClip(agent.audioClipManager.bomberManFuse, agent.transform.transform, 0.7f);
            SFXManager.instance.PlaySFXClip(agent.audioClipManager.bomberManLaugh, agent.transform.transform, 1f);
            runOnce = true;
        }


        if (bomber.bombAnimationTimer < 0)
        {
            bomber.bombExploded = true;
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.RUNNING;
        }


    }

    private void SetAnimation(BaseManager agent)
    {
        agent.animator.SetBool("Bomber_Sprint", true);
        agent.animator.SetBool("Bomber_Idle", false);
        agent.animator.SetBool("Bomber_Chase", false);
        agent.animator.SetBool("Bomber_Scared", false);
        agent.animator.SetBool("Bomber_Electrocute", false);
    }
}
