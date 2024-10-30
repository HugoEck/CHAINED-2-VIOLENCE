using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyberGiantManager : BaseManager
{
    //[HideInInspector]
    public float minimumBombDistance;
    [HideInInspector] public float bombPoint;
    [HideInInspector] public 

    private void Start()
    {
        enemyID = "CyberGiant";

        animator.SetBool("CyberGiant_Idle", true);

        LoadStats();
    }

    private void LoadStats()
    {

    }
}
