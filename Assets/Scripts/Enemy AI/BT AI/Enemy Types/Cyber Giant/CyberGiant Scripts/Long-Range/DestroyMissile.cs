using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMissile : MonoBehaviour
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

        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && player != null)
        {
            player.SetHealth(damage);
            SFXManager.instance.PlayRandomSFXClip(audioClipManager.bossExplosions, transform.transform, 1f);
            Destroy(gameObject);
        }
        else if (other.gameObject.layer != LayerMask.NameToLayer("Enemy"))
        {
            SFXManager.instance.PlayRandomSFXClip(audioClipManager.bossExplosions, transform.transform, 1f);
            Destroy(gameObject);
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
