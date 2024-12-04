using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChainEffects
{
    BaseManager agent;

    public AIChainEffects(BaseManager manager)
    {
        agent = manager;
    }

    public float stunStartTime;
    public float stunDurationTime = 0;
    public bool stunActivated = false;
    public string stunType = null;

    public void ActivateGhostChainEffect(float durationTime)
    {
        if (agent.enemyID != "Charger" && agent.enemyID != "CyberGiant")
        {
            stunActivated = true;
            stunDurationTime = durationTime;
            stunStartTime = Time.time;
            stunType = "Ghost";
        }
    }

    public void ActivateShockChainEffect(float durationTime)
    {
        if (agent.enemyID != "Charger" && agent.enemyID != "CyberGiant")
        {
            stunActivated = true;
            stunDurationTime = durationTime;
            stunStartTime = Time.time;
            stunType = "Shock";
        }
    }

    public void ActivateRagdollStun(float durationTime, GameObject affectingObject, float knockbackForce)
    {
        if (agent.enemyID != "Charger" && agent.enemyID != "CyberGiant")
        {
            stunActivated = true;
            stunDurationTime = durationTime;
            stunStartTime = Time.time;
            stunType = "Ragdoll";
            agent.behaviorMethods.ToggleRagdoll(true);

            Transform spine = agent.transform.Find("Root/Hips/Spine_01/Spine_02");
            if (spine == null)
            {
                Debug.Log(":(");
                return;
            }

            Vector3 directionAwayFromPlayer = spine.position - affectingObject.transform.position;
            directionAwayFromPlayer.y = 0;  
            directionAwayFromPlayer.Normalize();  

          
            Rigidbody spineRb = spine.GetComponent<Rigidbody>();
            if (spineRb != null)
            {
                spineRb.AddForce(directionAwayFromPlayer * knockbackForce, ForceMode.Impulse);
            }
        }
    }
    public void ActivateKnockbackStun(float durationTime, GameObject affectingObject, float knockbackForce)
    {
        if (agent.enemyID != "Charger" && agent.enemyID != "CyberGiant")
        {
            stunActivated = true;
            stunDurationTime = durationTime;
            stunStartTime = Time.time;
            stunType = "Knockback";

            
            

            Vector3 directionAwayFromPlayer = agent.transform.position - affectingObject.transform.position;
            directionAwayFromPlayer.y = 0;  
            directionAwayFromPlayer.Normalize();  

            Rigidbody Rb = agent.GetComponent<Rigidbody>();
            if (Rb != null)
            {
                
                Rb.AddForce(directionAwayFromPlayer * knockbackForce, ForceMode.Impulse);
            }
        }
    }


}
