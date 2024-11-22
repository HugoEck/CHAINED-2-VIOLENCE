using Pathfinding.Examples;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlaceFlag : Node
{

    private bool firstFlagPlaced = false; 
    public override NodeState Evaluate(BaseManager agent)
    {

        BannerManManager bm = agent as BannerManManager;

        GameObject rock = GameObject.Instantiate(bm.flagPrefab, agent.transform.position, Quaternion.identity);

        bm.nrFlagsLeft--;

        if (!firstFlagPlaced )
        {
            agent.targetedPlayer = agent.behaviorMethods.CalculateClosestTarget();
            bm.circleCenter = agent.targetedPlayer.transform.position;
            bm.currentVector = agent.transform.position - bm.circleCenter;
            firstFlagPlaced = true;

        }
        else
        {
            float tempX = bm.currentVector.x;
            bm.currentVector.x = bm.currentVector.z;
            bm.currentVector.z = -tempX;
        }

        bm.isNewFlagReady = false;
        bm.isNewDestinationCalculated = false;

        if (bm.nrFlagsLeft <= 0)
        {
            agent.currentHealth = 0;
        }


        return NodeState.SUCCESS;
    }
}
