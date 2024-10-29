using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public Animator playgroundDoorAnimation; // Reference to the door's Animator component
    public string triggerName = "Open"; // The trigger name to activate the animation

    // Detect when something enters the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the zone is the player
        if (other.CompareTag("Player1"))
        {
            // Trigger the door animation
            playgroundDoorAnimation.SetTrigger(triggerName);
        }
    }
}
