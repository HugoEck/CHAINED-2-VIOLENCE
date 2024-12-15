using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActivateBomb : Node
{
    public override NodeState Evaluate(BaseManager agent)
    {

        SetAnimation(agent);
        agent.behaviorMethods.RotateTowardsClosestPlayer();
        BomberManager bomber = agent as BomberManager;
        agent.navigation.isStopped = true;
        bomber.bombActivated = true;


        if (bomber.bombAnimationTimer < 5)
        {
            bomber.idleActive = false;
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.RUNNING;
        }
    }


    private void SetAnimation(BaseManager agent)
    {
        agent.animator.SetBool("Bomber_Idle", true);
        agent.animator.SetBool("Bomber_Chase", false);
        agent.animator.SetBool("Bomber_Sprint", false);
        agent.animator.SetBool("Bomber_Scared", false);
        agent.animator.SetBool("Bomber_Electrocute", false);
    }
}
