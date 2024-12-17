using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VignetteEffect : MonoBehaviour
{
    [Header("Volume Settings")]
    public Volume postProcessingVolume; // Reference to the Volume
    private Vignette vignette;

    [Header("Vignette Effect Settings")]
    public float vignetteFadeDuration = 0.5f; // Duration to fade in/out
    public float holdDuration = 0.5f;         // Time to hold the effect
    public float maxIntensity = 0.5f;         // Maximum vignette intensity

    private Coroutine currentVignetteCoroutine;

    private void Start()
    {
        // Retrieve the Vignette component from the Volume Profile
        if (postProcessingVolume.profile.TryGet(out vignette))
        {
            vignette.intensity.Override(0); // Ensure it's invisible at the start
            vignette.color.Override(Color.black); // Default to black
        }
        else
        {
            Debug.LogError("Vignette not found in Volume Profile! Ensure it's added to the Volume.");
        }
    }

    // Call this method when Player 1 takes damage
    public void PlayerOneTakesDamage()
    {
        if (vignette != null)
        {
            TriggerVignetteEffect(Color.red); // Fade vignette to red
        }
    }

    // Call this method when Player 2 takes damage
    public void PlayerTwoTakesDamage()
    {
        if (vignette != null)
        {
            TriggerVignetteEffect(Color.red); // Fade vignette to red
        }
    }

    private void TriggerVignetteEffect(Color color)
    {
        if (currentVignetteCoroutine != null)
        {
            StopCoroutine(currentVignetteCoroutine);
        }

        // Set the vignette color
        vignette.color.Override(color);

        // Start the fade effect
        currentVignetteCoroutine = StartCoroutine(FadeVignette());
    }

    private IEnumerator FadeVignette()
    {
        float timer = 0f;

        // Fade In
        while (timer < vignetteFadeDuration)
        {
            vignette.intensity.Override(Mathf.Lerp(0, maxIntensity, timer / vignetteFadeDuration));
            timer += Time.deltaTime;
            yield return null;
        }
        vignette.intensity.Override(maxIntensity);

        // Hold effect
        yield return new WaitForSeconds(holdDuration);

        // Fade Out
        timer = 0f;
        while (timer < vignetteFadeDuration)
        {
            vignette.intensity.Override(Mathf.Lerp(maxIntensity, 0, timer / vignetteFadeDuration));
            timer += Time.deltaTime;
            yield return null;
        }
        vignette.intensity.Override(0);

        // Reset the color to default (black)
        vignette.color.Override(Color.black);
    }
}
