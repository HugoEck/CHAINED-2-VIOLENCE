using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBomb : MonoBehaviour
{
    [HideInInspector] public float damage;
    private float destroyDelay = 0.1f;

    private AudioClipManager audioClipManager;

    private void Awake()
    {
        audioClipManager = FindAnyObjectByType<AudioClipManager>();
    }

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
        if (audioClipManager.bossExplosions != null)
        {

        SFXManager.instance.PlayRandomSFXClip(audioClipManager.bossExplosions, transform.transform, 1f);
        }

        // Wait for a brief moment to allow for additional hits
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}

//[HideInInspector] public float damage;
//bool bombExploded = false;

//private float destroyDelay = 0.2f;

//private void OnTriggerEnter(Collider other)
//{

//    Player player = other.gameObject.GetComponent<Player>();

//    if (player != null)
//    {
//        player.SetHealth(damage);
//    }

//    if (other.gameObject.tag != "Missile" || other.gameObject.tag != "Enemy")
//    {
//        bombExploded = true;
//    }


//}

//private void Update()
//{
//    if (bombExploded)
//    {
//        destroyDelay -= Time.deltaTime;
//        if (destroyDelay <= 0)
//        {
//            Destroy(gameObject);
//        }
//    }
//}
