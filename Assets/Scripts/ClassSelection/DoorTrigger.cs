using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public Animator doorAnimator;  // Reference to the door's Animator component
    public string openTriggerName = "Open";  // The trigger name to activate the opening animation
    public string closeTriggerName = "Close";  // The trigger name to activate the closing animation

    // Detect when something enters the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1"))
        {
            // Trigger the door opening animation
            doorAnimator.SetTrigger(openTriggerName);
        }
    }

    // Detect when something exits the trigger zone
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player1"))
        {
            // Trigger the door closing animation
            doorAnimator.SetTrigger(closeTriggerName);
        }
    }
}
