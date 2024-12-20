using UnityEngine;
using UnityEngine.UI;

public class ButtonInteraction : MonoBehaviour
{
    public GameObject player1; // Reference to the player's GameObject
    public GameObject player2;
    public GameObject interactPromptUI; // The UI element that prompts "Press E to Open Door"
    public DoorController doorController; // The script that controls the doors
    private AudioClipManager audioClipManager;

    public float interactionDistance = 3.0f; // How close the player needs to be to interact with the button

    private bool isPlayerInRange = false;
    private bool hasBeenPressed = false; // New variable to track if the button was pressed

    private bool isPlayer1PressingButton = false;
    private bool isPlayer2PressingButton = false;

    void Start()
    {
        // Ensure the prompt UI is hidden initially
        interactPromptUI.SetActive(false);
        audioClipManager = FindObjectOfType<AudioClipManager>();
    }

    void Update()
    {
        if (hasBeenPressed)
        {
            // If the button has been pressed, no further interactions should happen
            return;
        }

        // Measure the distance between the player and the button
        float distanceToPlayer1 = Vector3.Distance(player1.transform.position, transform.position);
        float distanceToPlayer2 = Vector3.Distance(player2.transform.position, transform.position);

        if (distanceToPlayer1 <= interactionDistance || distanceToPlayer2 <= interactionDistance)
        {
            // If the player is in range, show the prompt
            isPlayerInRange = true;
            interactPromptUI.SetActive(true);

            isPlayer1PressingButton = InputManager.Instance.GetInteractInput_P1();
            isPlayer2PressingButton = InputManager.Instance.GetInteractInput_P2();

            // When the player presses 'E' while in range, open the doors
            if (isPlayer1PressingButton || isPlayer2PressingButton)
            {
                if (audioClipManager.enterArenabutton != null)
                {
                SFXManager.instance.PlaySFXClip(audioClipManager.enterArenabutton, transform.transform, 1.0f);
                }
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
