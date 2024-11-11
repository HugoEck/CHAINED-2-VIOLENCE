using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static NPC_Customization;

public class ClassSelector : MonoBehaviour
{
    [SerializeField] private GameObject uiPrompt; // UI Text object for "Switch to: Class [E]"

    private GameObject activePlayer; // The player currently in range of a tube
    private PlayerCombat.PlayerClass targetClass; // Class associated with the nearby tube

    public event Action<GameObject, PlayerCombat.PlayerClass> OnClassSwitched;

    private bool _bIsPlayer1ChoosingClass = false;
    private bool _bIsPlayer2ChoosingClass = false;

    private int _playerId = 0;

    void Start()
    {
        uiPrompt.SetActive(false); // Ensure UI is hidden at the start

        _playerId = GetComponent<Player>()._playerId;
    }

    private void OnTriggerEnter(Collider other)
    {
       
        PlayerCombat.PlayerClass classType;

        if(other.CompareTag("Tank"))
        {
            classType = PlayerCombat.PlayerClass.Tank;
            ShowClassPrompt(other.gameObject, classType);
        }
        else if(other.CompareTag("Warrior"))
        {
            classType = PlayerCombat.PlayerClass.Warrior;
            ShowClassPrompt(other.gameObject, classType);
        }
        else if(other.CompareTag("Ranged"))
        {
            classType = PlayerCombat.PlayerClass.Ranged;
            ShowClassPrompt(other.gameObject, classType);
        }
        else if(other.CompareTag("Support"))
        {
            classType = PlayerCombat.PlayerClass.Support;
            ShowClassPrompt(other.gameObject, classType);
        }
        else
        {
            return;
        }

    }

    private void OnTriggerExit(Collider other)
    {

        PlayerCombat.PlayerClass classType;

        if (other.CompareTag("Tank"))
        {
            classType = PlayerCombat.PlayerClass.Tank;
            Debug.Log(other.tag + " exited the tube trigger for class: " + classType);
        }
        else if (other.CompareTag("Warrior"))
        {
            classType = PlayerCombat.PlayerClass.Warrior;
            Debug.Log(other.tag + " exited the tube trigger for class: " + classType);
        }
        else if (other.CompareTag("Ranged"))
        {
            classType = PlayerCombat.PlayerClass.Ranged;
            Debug.Log(other.tag + " exited the tube trigger for class: " + classType);
        }
        else if (other.CompareTag("Support"))
        {
            classType = PlayerCombat.PlayerClass.Support;
            Debug.Log(other.tag + " exited the tube trigger for class: " + classType);
        }
        else
        {
            return;
        }
        HideClassPrompt();
       
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
        
        if(_playerId == 1)
        {
            if(_bIsPlayer1ChoosingClass = InputManager.Instance.GetInteractInput_P1())
            {
                SwitchClass(gameObject, targetClass);
                Debug.Log(gameObject.tag + " Changed class");
            }
        }
        else if(_playerId == 2)
        {
            if(_bIsPlayer2ChoosingClass = InputManager.Instance.GetInteractInput_P2())
            {
                SwitchClass(gameObject, targetClass);
                Debug.Log(gameObject.tag + " Changed class");
            }
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

        if(_playerId == 1 && newClass != ClassManager._currentPlayer2Class)
        {
            player.GetComponent<PlayerCombat>().SetCurrentPlayerClass(newClass);
            hasClassBeenSet = true;
            Debug.Log("Has class been set");
        }
        else if(_playerId == 2 && newClass != ClassManager._currentPlayer1Class)
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
            OnClassSwitched?.Invoke(player, newClass);
        }     

        // Hide UI prompt after switching
        HideClassPrompt();
    }
}
