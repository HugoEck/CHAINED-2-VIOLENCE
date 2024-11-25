using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGWeaponCollisionDetecter : MonoBehaviour
{
    public CyberGiantManager cg; 



    void Start()
    {
        cg = GetComponentInParent<CyberGiantManager>();
    }





    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Player1"))
        {
            cg.DealDamageWithWeapon("p1");
        }
        else if (collision.gameObject.CompareTag("Player2"))
        {
            cg.DealDamageWithWeapon("p2");
        }
        else if (collision.gameObject.CompareTag("Misc"))
        {
            Destroy(collision.gameObject);

        }
    }
}
