using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Animator leftDoorAnimator;  // Animator for the left door
    public Animator rightDoorAnimator; // Animator for the right door
    public Camera doorCamera;          // Camera that focuses on the doors
    public Camera mainCamera;          // The main gameplay camera
    public float animationDuration = 3.0f; // Adjust this to match the length of your door animation

    private AudioClipManager audioClipManager;

    void Start()
    {
        // Ensure the door camera is disabled at the start
        doorCamera.enabled = false;
        mainCamera.enabled = true;
        audioClipManager = FindObjectOfType<AudioClipManager>();
    }

    public void OnStartButtonPress()
    {
        // Switch to the door camera
        mainCamera.enabled = false;
        doorCamera.enabled = true;

        // Trigger the door animations via the Open trigger
        leftDoorAnimator.SetTrigger("Open");
        rightDoorAnimator.SetTrigger("Open");

        // Start a coroutine to play the sound after a delay
        StartCoroutine(PlaySoundWithDelay(0.5f, audioClipManager.doorOpen));

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

    private IEnumerator PlaySoundWithDelay(float delay, AudioClip clip)
    {
        yield return new WaitForSeconds(delay);
        SFXManager.instance.PlaySFXClip(clip, transform, 1.0f);
    }
}
