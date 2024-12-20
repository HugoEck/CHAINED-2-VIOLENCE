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

            if (agent.audioClipManager.bomberManExplosion != null)
            {
            SFXManager.instance.PlayRandomSFXClip(agent.audioClipManager.bossExplosions, agent.transform.transform, 1f);
            }
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
