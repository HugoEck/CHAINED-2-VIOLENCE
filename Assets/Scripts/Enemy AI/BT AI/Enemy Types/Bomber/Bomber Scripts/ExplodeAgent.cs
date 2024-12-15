using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeAgent : Node
{
    private bool runOnce = false;
    public override NodeState Evaluate(BaseManager agent)
    {

        BomberManager bomber = agent as BomberManager;

        GameObject.Instantiate(bomber.explosionParticle, agent.transform.position, agent.transform.rotation);
        bomber.Explode();

        GameObject.Destroy(agent.gameObject);

        if (!runOnce)
        {
            if (GoldDropManager.Instance != null)
            {
                GoldDropManager.Instance.AddGold(agent.unitCost);
            }
            WaveManager.ActiveEnemies--;
            runOnce = true;
        }

        return NodeState.SUCCESS;
    }
}
