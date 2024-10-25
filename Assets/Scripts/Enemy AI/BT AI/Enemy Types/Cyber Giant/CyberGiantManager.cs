using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyberGiantManager : BaseManager
{

    private void Start()
    {
        enemyID = "CyberGiant";

        animator.SetBool("CyberGiant_Idle", true);
    }
}
