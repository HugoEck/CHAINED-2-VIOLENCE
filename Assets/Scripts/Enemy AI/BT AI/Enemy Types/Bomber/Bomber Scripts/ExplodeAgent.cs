using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeAgent : Node
{
    private bool runOnce = false;
    public override NodeState Evaluate(BaseManager agent)
    {
        BomberManager bomber = agent as BomberManager;

        if (!runOnce)
        {
            runOnce = true;

            if (agent.playerManager1.bHasPlayerEnteredCombat) { 
                if (SummaryUIScript.Instance != null && SummaryUIScript.Instance.player1UIStats != null) {
                    SummaryUIScript.Instance.player1UIStats.totalKills++;
                }
            }
            else if (agent.playerManager2.bHasPlayerEnteredCombat) {
                if (SummaryUIScript.Instance != null && SummaryUIScript.Instance.player2UIStats != null) {
                    SummaryUIScript.Instance.player2UIStats.totalKills++;
                }
            }

            GameObject.Instantiate(bomber.explosionParticle, agent.transform.position, agent.transform.rotation);
            bomber.Explode();

            if (GoldDropManager.Instance != null)
            {
                GoldDropManager.Instance.AddGold(agent.unitCost);
            }
            WaveManager.ActiveEnemies--;
            
        }
        GameObject.Destroy(agent.gameObject);
        return NodeState.SUCCESS;
    }
}
