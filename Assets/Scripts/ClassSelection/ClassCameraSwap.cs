using UnityEngine;

public class ClassCameraSwap : MonoBehaviour
{
    public Camera mainCamera;       // Reference to the main camera
    public Camera alternateCamera;  // Reference to the alternate camera

    void Start()
    {
        // Ensure that only the main camera is active initially
        mainCamera.enabled = true;
        alternateCamera.enabled = false;
    }

    // This function is called when the player enters the trigger zone
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1"))  // Make sure the object has the Player tag
        {
            // Switch to the alternate camera
            mainCamera.enabled = false;
            alternateCamera.enabled = true;
        }
    }

    // This function is called when the player exits the trigger zone
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player1"))  // Ensure the player is the one exiting
        {
            // Switch back to the main camera
            mainCamera.enabled = true;
            alternateCamera.enabled = false;
        }
    }
}
