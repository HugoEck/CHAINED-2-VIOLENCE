using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Actives the Upgrade Menu UI when one of the player presses the E or X on controller near the NPC. Press again to close.
/// </summary>
public class UpgradeUIActivator : MonoBehaviour
{
    public GameObject upgradeUICanvas;
    public float interactionDistance = 10f;

    private GameObject player1;
    private GameObject player2;
    private bool isWithinRange;

    void Start()
    {
        player1 = GameObject.FindGameObjectWithTag("Player1");
        player2 = GameObject.FindGameObjectWithTag("Player2");

        if (upgradeUICanvas != null)
        {
            upgradeUICanvas.SetActive(false);
        }
    }

    void Update()
    {
        if (player1 == null || player2 == null)
        {
            player1 = GameObject.FindGameObjectWithTag("Player1");
            player2 = GameObject.FindGameObjectWithTag("Player2");
        }
        ToggleUpgradeMenuUI();
    }

    private bool CheckPlayerDistance(GameObject player)
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        return distance <= interactionDistance;
    }

    private void ToggleUpgradeMenuUI()
    {
        if (player1 != null && player2 != null)
        {
            isWithinRange = CheckPlayerDistance(player1) || CheckPlayerDistance(player2);

            if (isWithinRange && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton2)))
            {
                bool newState = !upgradeUICanvas.activeSelf;
                upgradeUICanvas.SetActive(newState);
            }
        }
    }
}
