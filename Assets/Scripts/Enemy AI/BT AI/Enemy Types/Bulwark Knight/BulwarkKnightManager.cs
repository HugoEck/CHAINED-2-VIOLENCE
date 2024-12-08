using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulwarkKnightManager : BaseManager
{
    [Header("BULWARK KNIGHT MANAGER")]
    public GameObject shield;
    Transform[] children;

    void Start()
    {
        enemyID = "BulwarkKnight";
        animator.SetBool("BulwarkKnight_ShieldStand", true);

        children = GetComponentsInChildren<Transform>();
        shield = FindShieldObject()?.gameObject;

    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    Destroy(shield);
        //}
    }

    Transform FindShieldObject()
    {

        foreach (Transform child in children)
        {
            if (child.name == "BulwarkKnight_Shield")

                return child;
            
        }
        return null;
    }
}
