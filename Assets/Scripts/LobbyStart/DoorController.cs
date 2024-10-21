using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Animator leftDoorAnimator;  // Animator for the left door
    public Animator rightDoorAnimator; // Animator for the right door
    public Camera doorCamera;          // Camera that focuses on the doors
    public Camera mainCamera;          // The main gameplay camera
    public float animationDuration = 3.0f; // Adjust this to match the length of your door animation

    void Start()
    {
        // Ensure the door camera is disabled at the start
        doorCamera.enabled = false;
        mainCamera.enabled = true;
    }

    public void OnStartButtonPress()
    {
        // Switch to the door camera
        mainCamera.enabled = false;
        doorCamera.enabled = true;

        // Trigger the door animations via the Open trigger
        leftDoorAnimator.SetTrigger("Open");
        rightDoorAnimator.SetTrigger("Open");

        // Start a coroutine to switch back to the main camera after the animation
        StartCoroutine(SwitchBackToMainCamera());
    }


    private IEnumerator SwitchBackToMainCamera()
    {
        // Wait for the door animation to finish
        yield return new WaitForSeconds(animationDuration);

        // Switch back to the main camera
        doorCamera.enabled = false;
        mainCamera.enabled = true;
    }
}
