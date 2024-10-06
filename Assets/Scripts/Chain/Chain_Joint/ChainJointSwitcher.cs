using UnityEngine;
using System.Collections;

public class ChainJointSwitcher : MonoBehaviour
{
    public GameObject[] chainSegments;        // Array of chain segments (except the first and last ones)
    public Transform startPoint;              // Starting point (e.g., one player)
    public Transform endPoint;                // End point (e.g., the other player)
    public Rigidbody startPlayerRb;           // Rigidbody of the starting player
    public Rigidbody endPlayerRb;             // Rigidbody of the ending player

    public float jointSwitchDelay = 0.02f;    // Delay between switching joints to avoid abrupt changes

    // Switch all chain segments to use FixedJoint and align them in a straight line
    public void SwitchToFixedJointsAndAlign()
    {
        StartCoroutine(SwitchToFixedJointsCoroutine());
    }

    // Coroutine to switch to FixedJoints gradually and align chain segments
    private IEnumerator SwitchToFixedJointsCoroutine()
    {
        // Disable collisions temporarily
        DisableCollisions();

        // Disable physics and clear all forces
        FreezeRigidbodies();

        // Calculate the straight line between startPoint and endPoint
        Vector3 direction = (endPoint.position - startPoint.position).normalized;
        float segmentLength = Vector3.Distance(startPoint.position, endPoint.position) / (chainSegments.Length + 1);  // Divide line evenly among segments

        for (int i = 0; i < chainSegments.Length; i++)
        {
            GameObject segment = chainSegments[i];
            Rigidbody rb = segment.GetComponent<Rigidbody>();

            // Reposition the chain segments in a straight line
            Vector3 newPosition = startPoint.position + (direction * (segmentLength * (i + 1)));
            segment.transform.position = newPosition;

            // Align the chain segment with the direction between the players
            segment.transform.rotation = Quaternion.LookRotation(direction);

            // Remove HingeJoint if it exists and add FixedJoint
            HingeJoint hingeJoint = segment.GetComponent<HingeJoint>();
            if (hingeJoint != null)
            {
                Destroy(hingeJoint);  // Remove HingeJoint
            }

            // Add FixedJoint and connect to the previous segment or player
            AddFixedJoint(rb, i > 0 ? chainSegments[i - 1].GetComponent<Rigidbody>() : startPlayerRb);

            // Wait for a short delay to avoid abrupt joint changes
            yield return new WaitForSeconds(jointSwitchDelay);
        }

        // Add FixedJoint to the last segment connecting it to the end player
        AddFixedJoint(chainSegments[chainSegments.Length - 1].GetComponent<Rigidbody>(), endPlayerRb);

        // Re-enable collisions and unfreeze rigidbodies after the transition
        EnableCollisions();
        UnfreezeRigidbodies();

        Debug.Log("Switched to FixedJoints and aligned segments in a straight line.");
    }

    // Switch all chain segments back to HingeJoint
    public void SwitchToHingeJoints()
    {
        StartCoroutine(SwitchToHingeJointsCoroutine());
    }

    // Coroutine to switch back to HingeJoints gradually
    private IEnumerator SwitchToHingeJointsCoroutine()
    {
        // Disable collisions temporarily
        DisableCollisions();

        // Disable physics and clear all forces
        FreezeRigidbodies();

        foreach (GameObject segment in chainSegments)
        {
            Rigidbody rb = segment.GetComponent<Rigidbody>();

            // Remove FixedJoint if it exists and add HingeJoint
            FixedJoint fixedJoint = segment.GetComponent<FixedJoint>();
            if (fixedJoint != null)
            {
                Destroy(fixedJoint);  // Remove FixedJoint
            }

            // Add HingeJoint
            AddHingeJoint(rb);

            // Wait for a short delay to avoid abrupt joint changes
            yield return new WaitForSeconds(jointSwitchDelay);
        }

        // Re-enable collisions and unfreeze rigidbodies after the transition
        EnableCollisions();
        UnfreezeRigidbodies();

        Debug.Log("Switched to HingeJoints.");
    }

    // Helper method to add a FixedJoint
    void AddFixedJoint(Rigidbody rb, Rigidbody connectedBody)
    {
        FixedJoint fixedJoint = rb.gameObject.AddComponent<FixedJoint>();
        fixedJoint.connectedBody = connectedBody;
    }

    // Helper method to add a HingeJoint
    void AddHingeJoint(Rigidbody rb)
    {
        HingeJoint hingeJoint = rb.gameObject.AddComponent<HingeJoint>();
        // Optionally, you can adjust hinge settings if necessary
    }

    // Disable collisions between chain segments
    void DisableCollisions()
    {
        for (int i = 0; i < chainSegments.Length - 1; i++)
        {
            Collider currentCollider = chainSegments[i].GetComponent<Collider>();
            Collider nextCollider = chainSegments[i + 1].GetComponent<Collider>();

            if (currentCollider != null && nextCollider != null)
            {
                Physics.IgnoreCollision(currentCollider, nextCollider, true);
            }
        }

        Debug.Log("Collisions disabled between chain segments.");
    }

    // Enable collisions between chain segments
    void EnableCollisions()
    {
        for (int i = 0; i < chainSegments.Length - 1; i++)
        {
            Collider currentCollider = chainSegments[i].GetComponent<Collider>();
            Collider nextCollider = chainSegments[i + 1].GetComponent<Collider>();

            if (currentCollider != null && nextCollider != null)
            {
                Physics.IgnoreCollision(currentCollider, nextCollider, false);
            }
        }

        Debug.Log("Collisions re-enabled between chain segments.");
    }

    // Freeze all rigidbodies (set kinematic, stop movement, and disable forces)
    void FreezeRigidbodies()
    {
        foreach (GameObject segment in chainSegments)
        {
            Rigidbody rb = segment.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
                Debug.Log("CHAIN SEGMENTS FREEZED");
            }
        }

        if (startPlayerRb != null)
        {
            startPlayerRb.velocity = Vector3.zero;
            startPlayerRb.angularVelocity = Vector3.zero;
            startPlayerRb.isKinematic = true;
        }

        if (endPlayerRb != null)
        {
            endPlayerRb.velocity = Vector3.zero;
            endPlayerRb.angularVelocity = Vector3.zero;
            endPlayerRb.isKinematic = true;
        }

        Debug.Log("Rigidbodies frozen.");
    }

    // Unfreeze all rigidbodies (re-enable physics simulation)
    void UnfreezeRigidbodies()
    {
        foreach (GameObject segment in chainSegments)
        {
            Rigidbody rb = segment.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }
        }

        if (startPlayerRb != null)
        {
            startPlayerRb.isKinematic = false;
        }

        if (endPlayerRb != null)
        {
            endPlayerRb.isKinematic = false;
        }

        Debug.Log("Rigidbodies unfrozen.");
    }
}
