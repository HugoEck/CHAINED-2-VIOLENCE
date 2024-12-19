using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    [SerializeField] private AudioSource soundFXObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Play a specific audio clip
    public void PlaySFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        // Spawn SFX gameobject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        // Assign the audioClip
        audioSource.clip = audioClip;

        // Assign volume
        audioSource.volume = volume;

        // Play sound
        audioSource.Play();

        // Destroy clip after play
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }

    // Play a random audio clip from an array
    public void PlayRandomSFXClip(AudioClip[] audioClips, Transform spawnTransform, float volume)
    {
        // Validate the array
        if (audioClips == null || audioClips.Length == 0)
        {
            Debug.LogWarning("AudioClip array is null or empty.");
            return;
        }

        // Assign random index
        int rand = Random.Range(0, audioClips.Length);

        // Spawn SFX gameobject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        // Assign the audioClip
        audioSource.clip = audioClips[rand];

        // Assign volume
        audioSource.volume = volume;

        // Play sound
        audioSource.Play();

        // Destroy clip after play
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }

    // Play a specific audio clip by index from an array
    public void PlaySFXFromArray(AudioClip[] audioClips, int index, Transform spawnTransform, float volume)
    {
        // Validate index and array
        if (audioClips == null || index < 0 || index >= audioClips.Length)
        {
            Debug.LogWarning("Invalid index or audioClip array is null.");
            return;
        }

        // Spawn SFX gameobject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        // Assign the audioClip
        audioSource.clip = audioClips[index];

        // Assign volume
        audioSource.volume = volume;

        // Play sound
        audioSource.Play();

        // Destroy clip after play
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }
}
