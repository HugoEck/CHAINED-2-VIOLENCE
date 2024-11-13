using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShootBomb : Node
{
    private float distance;
    public override NodeState Evaluate(BaseManager agent)
    {

        CyberGiantManager cg =  agent as CyberGiantManager;

        

        agent.targetedPlayer = agent.behaviorMethods.CalculateClosestTarget();
        distance = Vector3.Distance(agent.transform.position, agent.targetedPlayer.position);

        if (cg.IsBombReady() && distance > cg.minBombDistance)
        {
            //cg.bombReady = false;
            GameObject bomb = GameObject.Instantiate(cg.bombPrefab, cg.bombShootPoint.position, cg.bombShootPoint.rotation);
            Rigidbody rb = bomb.GetComponent<Rigidbody>();
            DestroyBomb db = bomb.GetComponent<DestroyBomb>();

            db.damage = cg.bombDamage;
            rb.velocity = cg.bombVelocity;
        }
        
        return NodeState.SUCCESS;
    }
}
