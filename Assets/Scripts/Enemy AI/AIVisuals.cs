using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIVisuals : MonoBehaviour
{

    // FLASHING STUFF
    [HideInInspector] private Material material; // Assign the material in the Inspector
    [HideInInspector] private Color flashColor = Color.white; // The color you want it to flash
    [HideInInspector] private float flashDuration = 0.1f; // Duration of the flash
    [HideInInspector] private Color originalColor;
    [HideInInspector] public bool isFlashing = false;
    Renderer renderer;

    private float elapsedTime = 0f;
    private bool isLerpingToFlashColor = false;

    BaseManager agent;

    public AIVisuals(BaseManager agent)
    {
        this.agent = agent;
    }

    public void InitializeVisuals(BaseManager agent)
    {
        //FLASHING ENEMIES  -------------------------------------------------------
        renderer = agent.GetComponentInChildren<Renderer>(false);
        foreach (Transform child in agent.transform)
        {
            // Skip if the GameObject is inactive or is the "Root" GameObject
            if (!child.gameObject.activeInHierarchy || child.name == "Root")
            {
                continue;
            }

            // Get the Renderer component from active, direct children
            renderer = child.GetComponent<Renderer>();
            if (renderer != null)
            {
                material = renderer.material;
                break;
            }
        }
        material.EnableKeyword("_EMISSION");
        originalColor = material.GetColor("_EmissionColor");
        //--------------------------------------------------------------------------
    }
    public void ActivateVisuals()
    {
        if (!isFlashing)
        {     
                // Start flashing logic
                isFlashing = true;
                elapsedTime = 0f;
                isLerpingToFlashColor = true;
        }
    }

    public void FlashColor()
    {
        // Only update if flashing is active
        if (isFlashing)
        {
            elapsedTime += Time.deltaTime;

            if (isLerpingToFlashColor)
            {
                // Lerp towards the flash color
                float lerpFactor = elapsedTime / flashDuration;
                material.SetColor("_EmissionColor", Color.Lerp(originalColor, flashColor, lerpFactor));

                if (elapsedTime >= flashDuration)
                {
                    // After reaching the flash color, start lerping back to the original color
                    isLerpingToFlashColor = false;
                    elapsedTime = 0f;
                }
            }
            else
            {
                // Lerp back towards the original color
                float lerpFactor = elapsedTime / flashDuration;
                material.SetColor("_EmissionColor", Color.Lerp(flashColor, originalColor, lerpFactor));

                if (elapsedTime >= flashDuration)
                {
                    // Once back to original color, stop flashing
                    isFlashing = false;
                    material.SetColor("_EmissionColor", originalColor);
                    material.DisableKeyword("_EMISSION");
                }
            }
        }
    }
}





