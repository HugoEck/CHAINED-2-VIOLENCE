using UnityEngine;

public class TubeScript : MonoBehaviour
{
    public PlayerCombat.PlayerClass classType;
    public ClassSelector classSelector;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something entered the trigger");

        // Check if the collider belongs to Player1 or Player2
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            Debug.Log(other.tag + " entered the tube trigger for class: " + classType);
            classSelector.ShowClassPrompt(other.gameObject, classType);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Something exited the trigger");

        // Check if the collider belongs to Player1 or Player2
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            Debug.Log(other.tag + " exited the tube trigger for class: " + classType);
            classSelector.HideClassPrompt();
        }
    }
}
