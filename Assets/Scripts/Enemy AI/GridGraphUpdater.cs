using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGraphUpdater : MonoBehaviour
{
    public void UpdateGrid(Vector3 position, float updateRadius)
    {
        // Define the bounds around the obstacle to update
        Bounds updateBounds = new Bounds(position, Vector3.one * updateRadius);

        // Create the GraphUpdateObject
        GraphUpdateObject guo = new GraphUpdateObject(updateBounds);

        // Optional: Change properties for the update, like setting walkability
        guo.modifyWalkability = true; // Modify walkability based on conditions below
        guo.setWalkability = false; // Make area non-walkable if obstacle is added
        guo.updatePhysics = true; // Use physics to recheck the area

        // Apply the graph update
        AstarPath.active.UpdateGraphs(guo);
    }

    public void RemoveObstacleUpdate(Vector3 position, float updateRadius)
    {
        // Use similar update logic but set walkability back to true
        Bounds updateBounds = new Bounds(position, Vector3.one * updateRadius);
        GraphUpdateObject guo = new GraphUpdateObject(updateBounds);

        guo.modifyWalkability = true;
        guo.setWalkability = true; // Make area walkable again
        guo.updatePhysics = true;

        AstarPath.active.UpdateGraphs(guo);
    }
}
