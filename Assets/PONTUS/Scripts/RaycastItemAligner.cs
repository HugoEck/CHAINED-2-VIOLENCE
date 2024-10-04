using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastItemAligner : MonoBehaviour
{
    public float raycastDistance = 100f;
    public float overlapTestBoxSize = 1f;
    public LayerMask spawnedObjectLayer;

    public void AlignItem(GameObject item)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance))
        {
            Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            // Create an overlap box to check for nearby objects at the hit position
            Vector3 overlapTestBoxScale = new Vector3(overlapTestBoxSize, overlapTestBoxSize, overlapTestBoxSize);
            Collider[] collidersInsideOverlapBox = new Collider[10];
            int numberOfCollidersFound = Physics.OverlapBoxNonAlloc(hit.point, overlapTestBoxScale, collidersInsideOverlapBox, spawnRotation, spawnedObjectLayer);

            // Only position the item if no other objects were found
            if (numberOfCollidersFound == 0)
            {
                item.transform.position = hit.point;
                item.transform.rotation = spawnRotation;
            }
            else
            {
                // Log information or handle the case where overlap occurs
                Debug.Log("Overlap detected, skipping this spawn.");
            }
        }
    }
}
