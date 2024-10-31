using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateMissilePosition : Node
{
    
    float shootForce = 50;

    public override NodeState Evaluate(BaseManager agent)
    {
        CyberGiantManager cg = agent as CyberGiantManager;

        cg.p1_Velocity = CalculateVelocity(cg, cg.p1_LastPosition);
        cg.p2_Velocity = CalculateVelocity(cg, cg.p2_LastPosition);
        cg.chain_Velocity = CalculateVelocity(cg, cg.chain_LastPosition);

        return NodeState.SUCCESS;

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
}
