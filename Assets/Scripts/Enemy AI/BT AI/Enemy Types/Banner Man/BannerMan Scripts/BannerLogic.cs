using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BannerLogic : MonoBehaviour
{
    [HideInInspector] public float buffMultiplier = 1.25f; 
    private HashSet<BaseManager> buffedEnemies = new HashSet<BaseManager>();

    private void OnTriggerEnter(Collider other)
    {
        BaseManager agent = other.GetComponent<BaseManager>();
        if (agent != null && !buffedEnemies.Contains(agent))
        {
            if(agent.enemyID != "BannerMan" && agent.enemyID != "Charger")
            {
                buffedEnemies.Add(agent);
                agent.behaviorMethods.BannerManBuff(buffMultiplier);
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        BaseManager agent = other.GetComponent<BaseManager>();
        if (agent != null && buffedEnemies.Contains(agent))
        {
            if (agent.enemyID != "BannerMan" && agent.enemyID != "Charger")
            {
                buffedEnemies.Remove(agent);
                agent.behaviorMethods.BannerManBuff(1 / buffMultiplier);
            }
        }
    }

    private void OnDestroy()
    {
        foreach (BaseManager agent in buffedEnemies)
        {
            if (agent != null) 
            {
                agent.behaviorMethods.BannerManBuff(1 / buffMultiplier); 
            }
        }
        buffedEnemies.Clear(); 
    }
}
