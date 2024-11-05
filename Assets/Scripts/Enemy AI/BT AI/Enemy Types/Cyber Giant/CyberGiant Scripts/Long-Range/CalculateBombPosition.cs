using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateBombPosition : Node
{

    Vector3 chainLastPosition;
    float shootForce = 45;

    public override NodeState Evaluate(BaseManager agent)
    {
        CyberGiantManager cg = agent as CyberGiantManager;

        chainLastPosition = agent.behaviorMethods.CalculateChainPosition();

        Vector3 directionXZ = new Vector3(chainLastPosition.x - cg.bombShootPoint.position.x, 0f,
            chainLastPosition.z - cg.bombShootPoint.position.z);

        float heightDifference = chainLastPosition.y - cg.bombShootPoint.position.y;

        float distanceXZ = directionXZ.magnitude;

        float timeToTarget = distanceXZ / shootForce;

        float velocityY = (heightDifference + 0.5f * Mathf.Abs(9.81f) * timeToTarget * timeToTarget) / timeToTarget;

        Vector3 velocityXZ = directionXZ.normalized * shootForce;


        Vector3 finalVelocity = new Vector3(velocityXZ.x, velocityY, velocityXZ.z);

        cg.SetBombVelocity(finalVelocity);

        return NodeState.SUCCESS;

    }
}
