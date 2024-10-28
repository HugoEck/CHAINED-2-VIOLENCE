using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Obi;
using System.Runtime.CompilerServices;

public class UpgradeGameData : MonoBehaviour
{
    public static UpgradeGameData Instance { get; private set; }

    //public float playerHealth;
    //public float playerAttack;
    //public float playerSpeed;
    //public float chainLength;

    public float PlayerHealth { get; set; }
    public float PlayerAttack { get; set; }   
    public float PlayerSpeed { get; set; }

    public float ChainLength { get; set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
