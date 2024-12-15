using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideCharge : Node
{
    //float bombTimer = 5;
    public override NodeState Evaluate(BaseManager agent)
    {
        BomberManager bomber = agent as BomberManager;
        agent.navigation.isStopped = false;
        //Debug.Log("Entered suicide sprint!");

        agent.navigation.maxSpeed = bomber.sprintSpeed;
        agent.navigation.isStopped = false;
        agent.targetedPlayer = agent.behaviorMethods.CalculateClosestTarget();
        agent.navigation.destination = agent.targetedPlayer.position;

        //bombTimer -= Time.deltaTime;

        if (bomber.bombAnimationTimer < 0)
        {
            bomber.bombExploded = true;
            //Debug.Log("BOOOOM!!!");
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.RUNNING;
        }

 
    }
}
