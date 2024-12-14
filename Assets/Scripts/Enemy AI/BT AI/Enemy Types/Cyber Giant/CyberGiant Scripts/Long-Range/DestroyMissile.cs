using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMissile : MonoBehaviour
{

    [HideInInspector] public float damage;
    private float destroyDelay = 0.1f;

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();

        if (player != null)
        {
            player.SetHealth(damage);
        }
        if (other.gameObject.tag != "Missile" && other.gameObject.tag != "Enemy" && other.gameObject.tag != "BossWeapon")
        {
            StartCoroutine(DestroyAfterDelay());
        }

    }


    private IEnumerator DestroyAfterDelay()
    {
        // Wait for a brief moment to allow for additional hits
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}