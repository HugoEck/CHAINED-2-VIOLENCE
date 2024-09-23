using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private GameObject player1;
    private GameObject player2;

    // Start is called before the first frame update
    void Start()
    {
        player1 = GameObject.FindGameObjectWithTag("Player1");
        player2 = GameObject.FindGameObjectWithTag("Player2");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Player1 Upgrades
    public void UpgradePlayer1Health()
    {
        player1.GetComponent<HealthUpgradeComponent>().Upgrade();
    }

    public void UpgradePlayer1Damage()
    {
        player1.GetComponent<DamageUpgradeComponent>().Upgrade();
    }

    // Player2 Upgrades
    public void UpgradePlayer2Health()
    {
        player2.GetComponent<HealthUpgradeComponent>().Upgrade();
    }

    public void UpgradePlayer2Damage()
    {
        player2.GetComponent<DamageUpgradeComponent>().Upgrade();
    }
}