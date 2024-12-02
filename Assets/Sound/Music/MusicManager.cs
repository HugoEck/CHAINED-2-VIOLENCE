using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    public AudioSource audioSource;
    public List<AudioClip> songList;
    int nextSongIndex = 0;
    int currentSongIndex = 0;
    float fadeOutTime = 3;

    WaveManager waveManager;

    private void Start()
    {
        audioSource.clip = songList[currentSongIndex];
        audioSource.loop = true;
        audioSource.Play();
        waveManager = FindObjectOfType<WaveManager>();
    }

    private void Update()
    {
        nextSongIndex = (int)waveManager.currentEra;

        if (nextSongIndex != currentSongIndex)
        {
            // Change the current song index
            currentSongIndex = nextSongIndex;

            // Start the fade-out and fade-in sequence
            StartCoroutine(ChangeMusic(songList[nextSongIndex]));
        }
    }

    private IEnumerator ChangeMusic(AudioClip nextClip)
    {
        // Fade out the current music
        yield return StartCoroutine(FadeOutMusic(fadeOutTime));

        // Set the new clip and fade it in
        audioSource.clip = nextClip;
        yield return StartCoroutine(FadeInMusic(nextClip ,fadeOutTime));
    }

    // Start is called before the first frame update
    private IEnumerator FadeOutMusic(float fadeDuration)
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            // Gradually decrease the volume over the duration of the fade
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        // Ensure the volume is set to 0 after fading out
        audioSource.volume = 0;
        audioSource.Stop(); // Stop the current song
    }

    // Coroutine to fade in the next music track
    private IEnumerator FadeInMusic(AudioClip nextClip, float fadeDuration)
    {
        // Set the audio source to the new track
        audioSource.clip = nextClip;
        audioSource.Play(); // Start playing the next track
        audioSource.volume = 0; // Start at 0 volume

        float startVolume = audioSource.volume;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            // Gradually increase the volume to 1 over the duration of the fade
            audioSource.volume = Mathf.Lerp(startVolume, 1, t / fadeDuration);
            yield return null;
        }

        // Ensure the volume is fully up after fading in
        audioSource.volume = 1;
    }
}
