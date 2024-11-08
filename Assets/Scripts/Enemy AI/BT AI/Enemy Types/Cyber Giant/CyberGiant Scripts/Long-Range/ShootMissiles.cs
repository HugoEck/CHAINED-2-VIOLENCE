using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootMissiles : Node
{
    private float distance;
    public override NodeState Evaluate(BaseManager agent)
    {

        CyberGiantManager cg = agent as CyberGiantManager;

        GameObject p1_Missile = GameObject.Instantiate(cg.missilePrefab, cg.missileShootPoint.position, cg.missileShootPoint.rotation);
        GameObject p2_Missile = GameObject.Instantiate(cg.missilePrefab, cg.missileShootPoint.position, cg.missileShootPoint.rotation);
        GameObject chain_Missile = GameObject.Instantiate(cg.missilePrefab, cg.missileShootPoint.position, cg.missileShootPoint.rotation);

        // Set rotation to point toward the target positions
        p1_Missile.transform.forward = (cg.p1_LastPosition - p1_Missile.transform.position).normalized;
        p2_Missile.transform.forward = (cg.p2_LastPosition - p2_Missile.transform.position).normalized;
        chain_Missile.transform.forward = (cg.chain_LastPosition - chain_Missile.transform.position).normalized;

        Rigidbody rb_p1_Missile = p1_Missile.GetComponent<Rigidbody>();
        Rigidbody rb_p2_Missile = p2_Missile.GetComponent<Rigidbody>();
        Rigidbody rb_chain_Missile = chain_Missile.GetComponent<Rigidbody>();

        DestroyMissile dm_p1_Missile = p1_Missile.GetComponent<DestroyMissile>();
        DestroyMissile dm_p2_Missile = p2_Missile.GetComponent<DestroyMissile>();
        DestroyMissile dm_chain_Missile = chain_Missile.GetComponent<DestroyMissile>();

        dm_p1_Missile.damage = cg.missileDamage;
        dm_p2_Missile.damage = cg.missileDamage;
        dm_chain_Missile.damage = cg.missileDamage;

        rb_p1_Missile.velocity = cg.p1_Velocity;
        rb_p2_Missile.velocity = cg.p2_Velocity;
        rb_chain_Missile.velocity = cg.chain_Velocity;

        //cg.missileReady = false;
        cg.abilityInProgress = false;


        return NodeState.SUCCESS;
    }
}
