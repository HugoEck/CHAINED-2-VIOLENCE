using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfDead : Node
{

    private int deadThreshold = 0;
    

    public override NodeState Evaluate(BaseManager agent)
    {
        if (agent.currentHealth <= deadThreshold && !agent.agentIsDead)
        {
            agent.activateDeathTimer = true;
            agent.agentIsDead = true;

            if (agent.playerManager1.bHasPlayerEnteredCombat) { // JACK UI
                if (SummaryUIScript.Instance != null && SummaryUIScript.Instance.player1UIStats != null) {
                    SummaryUIScript.Instance.player1UIStats.totalKills++;
                }
            }
            else if (agent.playerManager2.bHasPlayerEnteredCombat) { // JACK UI
                if (SummaryUIScript.Instance != null && SummaryUIScript.Instance.player2UIStats != null) {
                    SummaryUIScript.Instance.player2UIStats.totalKills++;
                }
            }
            return NodeState.SUCCESS;
        }
        else
        {

            return NodeState.FAILURE;
        }
    }
}