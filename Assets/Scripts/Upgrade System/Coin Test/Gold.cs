using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    public GoldDropManager goldDropManager;

    void OnEnable()
    {
        if (goldDropManager == null)
        {
            goldDropManager = FindObjectOfType<GoldDropManager>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            Debug.Log("Gold picked up");

            goldDropManager.ReturnGold(gameObject);
        }
    }
}
