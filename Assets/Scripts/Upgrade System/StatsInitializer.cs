using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsInitializer : MonoBehaviour
{
    private static StatsInitializer instance;

    private GameObject player1;
    private GameObject player2;

    private Player player1Manager;
    private Player player2Manager;
    private PlayerCombat player1Combat;
    private PlayerCombat player2Combat;
    private PlayerMovement player1Movement;
    private PlayerMovement player2Movement;
    private AdjustChainLength adjustChainLength;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        player1 = GameObject.FindGameObjectWithTag("Player1");
        player2 = GameObject.FindGameObjectWithTag("Player2");

        if (player1 != null && player2 != null)
        {
            player1Manager = player1.GetComponent<Player>();
            player2Manager = player2.GetComponent<Player>();
            player1Combat = player1.GetComponent<PlayerCombat>();
            player2Combat = player2.GetComponent<PlayerCombat>();
            player1Movement = player1.GetComponent<PlayerMovement>();
            player2Movement = player2.GetComponent<PlayerMovement>();

            player1Manager.SetHealth(UpgradeGameData.Instance.PlayerHealth);
            player2Manager.SetHealth(UpgradeGameData.Instance.PlayerHealth);

            player1Combat.attackDamage = UpgradeGameData.Instance.PlayerAttack;
            player2Combat.attackDamage = UpgradeGameData.Instance.PlayerAttack;

            player1Movement.SetWalkingSpeed(UpgradeGameData.Instance.PlayerSpeed);
            player2Movement.SetWalkingSpeed(UpgradeGameData.Instance.PlayerSpeed);

            GameObject chainObject = GameObject.Find("Obi_Chain");
            if (chainObject != null)
            {
                adjustChainLength = chainObject.GetComponent<AdjustChainLength>();
                adjustChainLength.SetInitialChainLength(UpgradeGameData.Instance.ChainLength);
            }
        }
    }
}
