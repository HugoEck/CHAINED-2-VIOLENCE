using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyRock : MonoBehaviour
{
    [HideInInspector] public float damage;
    [SerializeField] LayerMask groundLayer;

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
        if(other.gameObject.layer == groundLayer)
        {
            Destroy(gameObject);
        }
    }
    
}
