using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculatePosition : Node
{

    public override NodeState Evaluate(BaseManager agent)
    {

        BannerManManager bm = agent as BannerManManager;

        bm.circleRadius -= 1;

        float nextX = bm.currentVector.z; 
        float nextZ = -bm.currentVector.x; 
        Vector3 rotatedVector = new Vector3(nextX, 0, nextZ);

        bm.newDestination = bm.circleCenter + rotatedVector.normalized * bm.circleRadius;
        bm.newDestination.y = 0;
        bm.isNewDestinationCalculated = true;

        return NodeState.SUCCESS;
    }
}
