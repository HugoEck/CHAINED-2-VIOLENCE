using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PusherManager : MonoBehaviour
{
    public float tiltAngle = 45f;      // The angle to which the Pusher will tilt
    public float tiltSpeed = 5f;       // The speed of the tilt
    private float waitTime = 2f;       // The time before the Pusher tilts'

    private bool isTilting = false;    // To track if the object is tilting
    private bool isReturning = false;  // To track if the object is returning


    private Quaternion targetRotation; // To store the target rotation
    private Quaternion originalRotation; // To store the original rotation

    void Start()
    {
        // Store the original rotation of the object
        originalRotation = transform.rotation;

        // Calculate the target rotation (tilting 45 degrees on the X axis)
        targetRotation = Quaternion.Euler(tiltAngle, originalRotation.eulerAngles.y, originalRotation.eulerAngles.z);

        // Start the timer to begin tilting after 2 seconds
        Invoke("StartTilting", waitTime);
    }

    void Update()
    {
        // If the object is tilting, smoothly rotate towards the target rotation
        if (isTilting)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * tiltSpeed);

            // Check if the object is close enough to the target rotation
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                isTilting = false;
                Invoke("StartReturning", waitTime); // Wait for 2 seconds before returning
                Debug.Log("Tilt complete. Returning soon.");
            }
        }

        // If the object is returning, smoothly rotate back to the original position
        if (isReturning)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, originalRotation, Time.deltaTime * tiltSpeed);

            // Check if the object is close enough to the original rotation
            if (Quaternion.Angle(transform.rotation, originalRotation) < 0.1f)
            {
                isReturning = false;
                Debug.Log("Returned to original position.");
            }
        }
    }

    void StartTilting()
    {
        // Start tilting the object
        Debug.Log("Starting tilt.");
        isTilting = true;
    }
    void StartReturning()
    {
        // Start returning the object to its original position
        Debug.Log("Starting return.");
        isReturning = true;
    }
}
