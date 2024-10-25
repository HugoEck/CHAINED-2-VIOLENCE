using UnityEngine;
using UnityEngine.UI;

public class ButtonInteraction : MonoBehaviour
{
    public GameObject player; // Reference to the player's GameObject
    public GameObject interactPromptUI; // The UI element that prompts "Press E to Open Door"
    public DoorController doorController; // The script that controls the doors

    public float interactionDistance = 3.0f; // How close the player needs to be to interact with the button

    private bool isPlayerInRange = false;
    private bool hasBeenPressed = false; // New variable to track if the button was pressed

    void Start()
    {
        // Ensure the prompt UI is hidden initially
        interactPromptUI.SetActive(false);
    }

    void Update()
    {
        if (hasBeenPressed)
        {
            // If the button has been pressed, no further interactions should happen
            return;
        }

        // Measure the distance between the player and the button
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance <= interactionDistance)
        {
            // If the player is in range, show the prompt
            isPlayerInRange = true;
            interactPromptUI.SetActive(true);

            // When the player presses 'E' while in range, open the doors
            if (Input.GetKeyDown(KeyCode.E))
            {
                doorController.OnStartButtonPress(); // Trigger door animation and camera switch
                interactPromptUI.SetActive(false); // Hide the prompt after pressing 'E'
                hasBeenPressed = true; // Mark the button as pressed so it can't be pressed again
            }
        }
        else
        {
            // If the player is out of range, hide the prompt
            isPlayerInRange = false;
            interactPromptUI.SetActive(false);
        }
    }
}
