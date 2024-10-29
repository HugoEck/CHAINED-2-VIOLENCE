using UnityEngine;
using UnityEngine.UI;

public class ClassSelector : MonoBehaviour
{
    public GameObject player1; // Reference to Player 1 object
    public GameObject player2; // Reference to Player 2 object
    public GameObject uiPrompt; // UI Text object for "Switch to: Class [E]"

    private GameObject activePlayer; // The player currently in range of a tube
    private PlayerCombat.PlayerClass targetClass; // Class associated with the nearby tube

    void Start()
    {
        uiPrompt.SetActive(false); // Ensure UI is hidden at the start
    }

    // Called when a player enters the trigger of a class tube
    public void ShowClassPrompt(GameObject player, PlayerCombat.PlayerClass classType)
    {
        activePlayer = player;
        targetClass = classType;

        // Show prompt and update text
        uiPrompt.SetActive(true);
        uiPrompt.GetComponent<Text>().text = "Switch to: " + targetClass + " [E]";
        Debug.Log("Showing class prompt for class: " + targetClass);
    }

    // Called when player exits the trigger zone
    public void HideClassPrompt()
    {
        activePlayer = null;
        uiPrompt.SetActive(false);
        Debug.Log("Hiding class prompt");
    }

    void Update()
    {
        if (activePlayer != null && Input.GetKeyDown(KeyCode.E))
        {
            SwitchClass(activePlayer, targetClass);
        }
    }

    void SwitchClass(GameObject player, PlayerCombat.PlayerClass newClass)
    {
        // Find the Classes parent GameObject
        Transform classesParent = player.transform.Find("Classes:");
        if (classesParent == null)
        {
            Debug.LogError("Classes parent not found under player object!");
            return;
        }

        bool hasClassBeenSet = false;

        // Update the player's current class in ClassManager
        if (player == player1 && newClass != ClassManager._currentPlayer2Class)
        {
            player.GetComponent<PlayerCombat>().SetCurrentPlayerClass(newClass);
            hasClassBeenSet = true;
            
        }
        else if (player == player2 && newClass != ClassManager._currentPlayer1Class)
        {
            player.GetComponent<PlayerCombat>().SetCurrentPlayerClass(newClass);
            hasClassBeenSet = true;
        }

        if(hasClassBeenSet)
        {
            // Iterate over each child under Classes and enable the matching class model
            foreach (Transform classTransform in classesParent)
            {
                bool shouldEnable = classTransform.name == newClass.ToString();
                classTransform.gameObject.SetActive(shouldEnable);

                Debug.Log($"Setting {classTransform.name} to {(shouldEnable ? "enabled" : "disabled")}");
            }
        }     

        // Hide UI prompt after switching
        HideClassPrompt();
    }
}
