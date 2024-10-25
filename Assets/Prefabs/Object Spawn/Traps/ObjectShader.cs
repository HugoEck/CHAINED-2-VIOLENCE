using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ObjectShader : MonoBehaviour
{
    public Material dissolveMaterial;  // The material with the dissolve shader

    private Renderer objectRenderer;
    private Coroutine dissolveCoroutine;

    public Vector2 dissolveYRange = new Vector2(-0.1f, 15f); // Y range for dissolving

    private void Start()
    {
        // Cache the renderer component
        objectRenderer = GetComponent<Renderer>();

        // Assign the dissolve material initially for spawning effect
        objectRenderer.material = dissolveMaterial;

        // Trigger the spawn effect
        StartSpawn();
    }

    public void StartSpawn()
    {
        if (dissolveCoroutine != null)
        {
            StopCoroutine(dissolveCoroutine);
        }

        // Start the dissolve process over a fixed duration (e.g., 3 seconds)
        dissolveCoroutine = StartCoroutine(DissolveObject(3f, dissolveYRange.x, dissolveYRange.y, false));  // From startY to endY
    }

    public void StartDespawn()
    {
        if (dissolveCoroutine != null)
        {
            StopCoroutine(dissolveCoroutine);
        }

        // Start the despawn (reverse dissolve) process over a fixed duration (e.g., 3 seconds)
        dissolveCoroutine = StartCoroutine(DissolveObject(3f, dissolveYRange.y, dissolveYRange.x, true));  // From endY to startY
    }

    private IEnumerator DissolveObject(float duration, float startY, float endY, bool destroyOnComplete)
    {
        float timeElapsed = 0f;

        // Get the initial dissolve offset (Y axis)
        Vector3 initialOffset = dissolveMaterial.GetVector("_DissolveOffest");  // Corrected name
        Vector3 startOffset = new Vector3(initialOffset.x, startY, initialOffset.z);
        Vector3 targetOffset = new Vector3(initialOffset.x, endY, initialOffset.z);

        // Set the initial dissolve offset
        dissolveMaterial.SetVector("_DissolveOffest", startOffset);  // Corrected name

        // Dissolve over time (over the fixed duration)
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(timeElapsed / duration);

            // Interpolate the dissolve offset over time
            Vector3 dissolveValue = Vector3.Lerp(startOffset, targetOffset, t);

            // Update the dissolve shader with the new offset value
            dissolveMaterial.SetVector("_DissolveOffest", dissolveValue);  // Corrected name

            yield return null;
        }

        // Ensure the final Y value is set
        dissolveMaterial.SetVector("_DissolveOffest", targetOffset);  // Corrected name
        //Debug.Log("Dissolve complete!");

        // Destroy the object if it's a despawn action
        if (destroyOnComplete)
        {
            Destroy(gameObject);
        }
    }
}