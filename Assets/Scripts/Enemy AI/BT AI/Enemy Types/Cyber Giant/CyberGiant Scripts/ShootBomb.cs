using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBomb : Node
{
    public override NodeState Evaluate(BaseManager agent)
    {

        CyberGiantManager cg =  agent as CyberGiantManager;

        if (cg.IsAttackAllowed())
        {
            GameObject bomb = GameObject.Instantiate(cg.bombPrefab, cg.bombShootPoint.position, cg.bombShootPoint.rotation);
            Rigidbody rb = bomb.GetComponent<Rigidbody>();
            DestroyBomb db = bomb.GetComponent<DestroyBomb>();

            db.damage = cg.bombDamage;
            rb.velocity = cg.bombVelocity;
        }
        
        return NodeState.SUCCESS;
    }
}
