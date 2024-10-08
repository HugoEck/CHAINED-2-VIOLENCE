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
        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            player.SetHealth(damage);
        }
        if(collision.gameObject.tag != "Rock" || collision.gameObject.tag != "Enemy")
        {
            Destroy(gameObject);
        }
    }
}
