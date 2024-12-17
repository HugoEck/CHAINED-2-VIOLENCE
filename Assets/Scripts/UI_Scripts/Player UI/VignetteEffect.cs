using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VignetteEffect : MonoBehaviour
{
    public Volume postProcessingVolume; // Reference to the Volume
    private Vignette vignette;

    [Header("Vignette Settings")]
    public float vignetteFadeDuration = 0.5f; // Duration for fade in/out
    public float holdDuration = 0.5f;         // Duration to hold intensity
    public float maxIntensity = 0.5f;         // Maximum vignette intensity

    private Coroutine vignetteCoroutine;

    private void Start()
    {
        // Retrieve the Vignette override from the Volume profile
        if (postProcessingVolume.profile.TryGet(out vignette))
        {
            Debug.Log("Vignette found in Volume Profile!");

            // Enable override states for color and intensity
            vignette.intensity.overrideState = true;
            vignette.color.overrideState = true;

            // Set default values
            vignette.intensity.value = 0;
            vignette.color.value = Color.black;
        }
        else
        {
            Debug.LogError("Vignette effect not found in Volume Profile!");
        }
    }

    public void PlayerOneTakesDamage()
    {
        if (vignette == null) return;

        Debug.Log("Player 1 vignette triggered.");
        TriggerVignetteEffect();
    }

    public void PlayerTwoTakesDamage()
    {
        if (vignette == null) return;

        Debug.Log("Player 2 vignette triggered.");
        TriggerVignetteEffect();
    }

    private void TriggerVignetteEffect()
    {
        if (vignetteCoroutine != null)
        {
            StopCoroutine(vignetteCoroutine);
        }
        vignetteCoroutine = StartCoroutine(FadeVignette());
    }

    private IEnumerator FadeVignette()
    {
        float timer = 0f;

        // Set vignette color to red
        vignette.color.value = Color.red;

        // Fade In
        while (timer < vignetteFadeDuration)
        {
            vignette.intensity.value = Mathf.Lerp(0, maxIntensity, timer / vignetteFadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        vignette.intensity.value = maxIntensity;

        // Hold the effect
        yield return new WaitForSeconds(holdDuration);

        // Fade Out
        timer = 0f;
        while (timer < vignetteFadeDuration)
        {
            vignette.intensity.value = Mathf.Lerp(maxIntensity, 0, timer / vignetteFadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        vignette.intensity.value = 0;

        Debug.Log("Vignette effect completed.");
    }
}
