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

            GameObject.Instantiate(bomber.explosionParticle, agent.transform.position, agent.transform.rotation);
            bomber.Explode();

            
            Debug.Log("Hallå????");
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
