using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockDestroyer : MonoBehaviour
{

    [HideInInspector] public float damage;


    private void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        PlayerManager player = collision.gameObject.GetComponent<PlayerManager>();

        if (player != null)
        {
            player.SetHealth(damage);
        }
        if(collision.gameObject.tag != "Rock")
        {
        Destroy(gameObject);
        }
    }
}
