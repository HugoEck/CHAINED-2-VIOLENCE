using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockDestroyer : MonoBehaviour
{

    [HideInInspector] public float damage;


    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();

        if (player != null)
        {
            player.SetHealth(damage);
        }
        if (other.gameObject.tag != "Rock" || other.gameObject.tag != "Enemy")
        {
            Destroy(gameObject);
        }
    }
}
