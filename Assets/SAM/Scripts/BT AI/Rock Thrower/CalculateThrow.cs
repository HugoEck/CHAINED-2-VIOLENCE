using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CalculateThrow : Node
{
    Transform playerLastPosition;
    float throwForce = 12;
    
    public override NodeState Evaluate(BaseManager agent)
    {
        RockThrowerManager rockThrower = agent as RockThrowerManager;

        if (rockThrower == null)
        {
            Debug.Log("Denna metod fungerar inte!");   
        }

        targetedPlayer = agent.CalculateClosestTarget();
        playerLastPosition = targetedPlayer;

        // Distance between start and target positions on the XZ plane
        Vector3 directionXZ = new Vector3(playerLastPosition.position.x - rockThrower.throwPoint.position.x, 0f,
            playerLastPosition.position.z - rockThrower.throwPoint.position.z);

        // Vertical distance (Y axis)
        float heightDifference = playerLastPosition.position.y - rockThrower.throwPoint.position.y;

        // Distance in the XZ plane
        float distanceXZ = directionXZ.magnitude;

        // Calculate the time to target based on distance and initial speed (horizontal)
        float timeToTarget = distanceXZ / throwForce;

        // Adjust the Y velocity to account for the time and gravity
        float velocityY = (heightDifference + 0.5f * Mathf.Abs(9.81f) * timeToTarget * timeToTarget) / timeToTarget;

        Vector3 velocityXZ = directionXZ.normalized * throwForce;

        Vector3 finalVelocity = new Vector3(velocityXZ.x, velocityY, velocityXZ.z);

        rockThrower.SetCalculatedVelocity(finalVelocity);

        return NodeState.SUCCESS;

    }
  

    
}
