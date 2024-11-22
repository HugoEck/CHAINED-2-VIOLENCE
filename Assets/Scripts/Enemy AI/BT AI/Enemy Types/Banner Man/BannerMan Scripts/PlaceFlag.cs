using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceFlag : Node
{

    private bool firstFlagPlaced = false; 
    public override NodeState Evaluate(BaseManager agent)
    {

        BannerManManager bm = agent as BannerManManager;

        GameObject rock = GameObject.Instantiate(bm.flagPrefab, agent.transform.position, Quaternion.identity);

        if (!firstFlagPlaced )
        {
            agent.targetedPlayer = agent.behaviorMethods.CalculateClosestTarget();
            bm.circleCenter = agent.targetedPlayer.transform.position;
            bm.currentVector = agent.transform.position - bm.circleCenter;
            firstFlagPlaced = true;
        }

        bm.isNewFlagReady = false;
        bm.isNewDestinationCalculated = false;

        return NodeState.SUCCESS;
    }
}
