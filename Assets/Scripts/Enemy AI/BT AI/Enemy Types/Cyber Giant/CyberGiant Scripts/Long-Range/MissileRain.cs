using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class MissileRain : Node
{
    Vector3 p1_LastPosition;
    Vector3 p2_LastPosition;
    Vector3 chain_LastPosition;
    Vector3 p1_Velocity;
    Vector3 p2_Velocity;
    Vector3 chain_Velocity;

    float shootForce = 60;
    float animationTotTime = 4f;
    float animationTimer = 4f;
    int nrMissilesShot = 0;


    public override NodeState Evaluate(BaseManager agent)
    {

        CyberGiantManager cg = agent as CyberGiantManager;

        agent.navigation.rotationSpeed = 360;
        agent.navigation.isStopped = true;

        SetAnimation(agent);

        p1_LastPosition = agent.player1.transform.position;
        p2_LastPosition = agent.player2.transform.position;
        chain_LastPosition = agent.behaviorMethods.CalculateChainPosition();
        chain_LastPosition = new Vector3(chain_LastPosition.x, 0, chain_LastPosition.z);

        p1_Velocity = CalculateVelocity(cg, p1_LastPosition);
        p2_Velocity = CalculateVelocity(cg, p2_LastPosition);
        chain_Velocity = CalculateVelocity(cg, chain_LastPosition);

        RotateTowardsChain(agent, chain_LastPosition);

        if (animationTimer < 2f && nrMissilesShot < 3)
        {
            ShootMissile(cg, nrMissilesShot);
            nrMissilesShot++;
        }

        animationTimer -= Time.deltaTime;

        if (animationTimer < 0)
        {
            animationTimer = animationTotTime;
            cg.missileRainActive = false;
            nrMissilesShot = 0;
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.RUNNING;
        }

    }
    private void RotateTowardsChain(BaseManager agent, Vector3 chainPosition)
    {
        Vector3 direction = (chainPosition - agent.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, lookRotation, Time.deltaTime * agent.navigation.rotationSpeed);
    }

    private void SetAnimation(BaseManager agent)
    {
        agent.animator.SetBool("CyberGiant_MissileRain", true);
        agent.animator.SetBool("CyberGiant_Walk", false);
        agent.animator.SetBool("CyberGiant_ShieldWalk", false);
        agent.animator.SetBool("CyberGiant_JumpEngage", false);
        agent.animator.SetBool("CyberGiant_OverheadSmash1", false);
        agent.animator.SetBool("CyberGiant_OverheadSmash2", false);
        agent.animator.SetBool("CyberGiant_Idle", false);
    }

    private Vector3 CalculateVelocity(CyberGiantManager cg, Vector3 position)
    {
        Vector3 directionXZ = new Vector3(position.x - cg.missileShootPoint.position.x, 0f,
            position.z - cg.missileShootPoint.position.z);
        float heightDifference = position.y - cg.missileShootPoint.position.y;
        float distanceXZ = directionXZ.magnitude;
        float timeToTarget = distanceXZ / shootForce;
        float velocityY = (heightDifference + 0.5f * Mathf.Abs(9.81f) * timeToTarget * timeToTarget) / timeToTarget;
        Vector3 velocityXZ = directionXZ.normalized * shootForce;
        Vector3 finalVelocity = new Vector3(velocityXZ.x, velocityY, velocityXZ.z);

        return finalVelocity;
    }

    private void ShootMissile(CyberGiantManager cg, int missileID)
    {

        if (missileID == 0)
        {
            GameObject p1_Missile = GameObject.Instantiate(cg.missilePrefab, cg.missileShootPoint.position, cg.missileShootPoint.rotation);
            p1_Missile.transform.forward = (p1_LastPosition - p1_Missile.transform.position).normalized;
            Rigidbody rb_p1_Missile = p1_Missile.GetComponent<Rigidbody>();
            DestroyMissile dm_p1_Missile = p1_Missile.GetComponent<DestroyMissile>();
            dm_p1_Missile.damage = cg.missileDamage;
            rb_p1_Missile.velocity = p1_Velocity;
        }
        else if (missileID == 1)
        {
            GameObject p2_Missile = GameObject.Instantiate(cg.missilePrefab, cg.missileShootPoint.position, cg.missileShootPoint.rotation);
            p2_Missile.transform.forward = (p2_LastPosition - p2_Missile.transform.position).normalized;
            Rigidbody rb_p2_Missile = p2_Missile.GetComponent<Rigidbody>();
            DestroyMissile dm_p2_Missile = p2_Missile.GetComponent<DestroyMissile>();
            dm_p2_Missile.damage = cg.missileDamage;
            rb_p2_Missile.velocity = p2_Velocity;
        }
        else if (missileID == 2)
        {
            GameObject chain_Missile = GameObject.Instantiate(cg.missilePrefab, cg.missileShootPoint.position, cg.missileShootPoint.rotation);
            chain_Missile.transform.forward = (chain_LastPosition - chain_Missile.transform.position).normalized;
            Rigidbody rb_chain_Missile = chain_Missile.GetComponent<Rigidbody>();
            DestroyMissile dm_chain_Missile = chain_Missile.GetComponent<DestroyMissile>();
            dm_chain_Missile.damage = cg.missileDamage;
            rb_chain_Missile.velocity = chain_Velocity;
        }

    }
}