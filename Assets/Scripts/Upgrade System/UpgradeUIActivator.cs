using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Actives the Upgrade Menu UI when one of the player presses the E or X on controller near the NPC.
/// </summary>
public class UpgradeUIActivator : MonoBehaviour
{
    public GameObject upgradeUICanvas;

    private bool isPlayer1InRange = false;
    private bool isPlayer2InRange = false;

    void Start()
    {
        if (upgradeUICanvas != null)
        {
            upgradeUICanvas.SetActive(false);
        }
    }

    void Update()
    {
        ToggleUpgradeMenuUI();
    }

    private void ToggleUpgradeMenuUI()
    {
        if ((isPlayer1InRange || isPlayer2InRange) && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton2)))
        {
            bool newState = !upgradeUICanvas.activeSelf;
            upgradeUICanvas.SetActive(newState);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player1"))
        {
            isPlayer1InRange = true;
        }
        else if (other.gameObject.CompareTag("Player2"))
        {
            isPlayer2InRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player1"))
        {
            isPlayer1InRange = false;
        }
        else if (other.gameObject.CompareTag("Player2"))
        {
            isPlayer2InRange = false;
        }

        if (!isPlayer1InRange && !isPlayer2InRange)
        {
            upgradeUICanvas.SetActive(false);
        }
    }
}