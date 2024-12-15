using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideCharge : Node
{
    public override NodeState Evaluate(BaseManager agent)
    {

        SetAnimation(agent);
        BomberManager bomber = agent as BomberManager;
        agent.navigation.isStopped = false;

        agent.navigation.maxSpeed = bomber.sprintSpeed;
        agent.navigation.isStopped = false;
        agent.targetedPlayer = agent.behaviorMethods.CalculateClosestTarget();
        agent.navigation.destination = agent.targetedPlayer.position;


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
