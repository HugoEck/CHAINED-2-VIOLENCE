using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillAgent : Node
{
    float deathDurationTime = 5;
    float deathTimerStart = 0;
    private bool isTimerInitialized = false;
    private bool giveGold = false;
    private bool runOnce = false;


    public override NodeState Evaluate(BaseManager agent)
    {

        if (SceneManager.GetActiveScene().name != "SamTestScene")
        {
            agent.behaviorMethods.ToggleRagdoll(true);
        }
        agent.rb.constraints = RigidbodyConstraints.None;
        agent.animator.enabled = false;
        agent.navigation.isStopped = true;

        if (runOnce == false)
        {
            if (GoldDropManager.Instance != null) 
            {
                GoldDropManager.Instance.AddGold(agent.unitCost);
                giveGold = true;
            }
            WaveManager.ActiveEnemies--;
            runOnce = true;
        }

        if (!isTimerInitialized)
        {
            deathTimerStart = Time.time;
            isTimerInitialized = true;
        }

        if (Time.time >= deathTimerStart + deathDurationTime)
        {
            GameObject.Destroy(agent.gameObject);
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.RUNNING;
        }


    }

}