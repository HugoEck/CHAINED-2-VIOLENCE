using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VignetteEffect : MonoBehaviour
{
    public Image player1Vignette; // Drag the Player 1 vignette image here
    public Image player2Vignette; // Drag the Player 2 vignette image here

    public float fadeDuration = 1f; // Duration for fade in/out

    private void Start()
    {
        // Initialize the vignette images as fully transparent
        SetAlpha(player1Vignette, 0f);
        SetAlpha(player2Vignette, 0f);
    }

    public void TriggerVignette(int playerID)
    {
        if (playerID == 1)
        {
            StartCoroutine(FadeVignette(player1Vignette));
        }
        else if (playerID == 2)
        {
            StartCoroutine(FadeVignette(player2Vignette));
        }
    }

    private IEnumerator FadeVignette(Image vignette)
    {
        // Fade in
        yield return StartCoroutine(FadeImage(vignette, 0f, 1f));

        // Pause at full opacity
        yield return new WaitForSeconds(0.5f);

        // Fade out
        yield return StartCoroutine(FadeImage(vignette, 1f, 0f));
    }

    private IEnumerator FadeImage(Image image, float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            SetAlpha(image, alpha);
            yield return null;
        }

        SetAlpha(image, endAlpha);
    }

    private void SetAlpha(Image image, float alpha)
    {
        if (image == null) return;
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
}
