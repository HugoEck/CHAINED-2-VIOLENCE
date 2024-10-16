using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class KillAgent : Node
{


    public override NodeState Evaluate(BaseManager agent)
    {
        WaveManager.ActiveEnemies--;
        agent.animator.enabled = false;
        agent.navMeshAgent.isStopped = true;

        

        if (agent.agentIsDead)
        {
            GameObject.Destroy(agent.gameObject);
        }

        return NodeState.RUNNING;
    }
}