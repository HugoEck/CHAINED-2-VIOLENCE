using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveChainTest : MonoBehaviour
{
    Rigidbody rb;
    public float moveSpeed = 5f; // Speed of movement

    private Vector3 direction;

    FixedJoint joint;
    bool hasConnected = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        joint = GetComponent<FixedJoint>();
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(!hasConnected)
    //    {
    //        if (collision.gameObject.tag == "ChainSegment")
    //        {
    //            Rigidbody otherRB = collision.gameObject.GetComponent<Rigidbody>();
    //            joint.connectedBody = otherRB;
    //        }

    //    }    
    //}

    private void Update()
    {
        // Reset direction every frame to prevent accumulating movement
        direction = Vector3.zero;

        // Check for input and set direction accordingly
        if (Input.GetKey(KeyCode.UpArrow))
        {
            direction += Vector3.forward; // Move forward (positive Z direction)
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            direction += Vector3.back; // Move backward (negative Z direction)
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            direction += Vector3.left; // Move left (negative X direction)
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            direction += Vector3.right; // Move right (positive X direction)
        }

        // Normalize direction to avoid diagonal speed boost
        direction = direction.normalized;

        // Apply movement to the Rigidbody
        rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
    }
}


