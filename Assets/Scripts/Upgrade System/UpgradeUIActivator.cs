using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// fix so it works for both players with networking etc...
/// </summary>
public class UpgradeUIActivator : MonoBehaviour
{
    public GameObject upgradeUICanvas;
    public float interactionDistance = 10f;

    private GameObject player1;
    private GameObject player2;

    // Start is called before the first frame update
    void Start()
    {
        player1 = GameObject.FindGameObjectWithTag("Player1");
        player2 = GameObject.FindGameObjectWithTag("Player2");

        upgradeUICanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ToggleUpgradeUICanvas();
    }

    private void ToggleUpgradeUICanvas()
    {
        float distanceP1 = Vector3.Distance(player1.transform.position, transform.position);
        float distanceP2 = Vector3.Distance(player2.transform.position, transform.position);

        if (distanceP1 <= interactionDistance)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                bool isActive = upgradeUICanvas.activeSelf;
                upgradeUICanvas.SetActive(!isActive);
            }
        }

    }


}
