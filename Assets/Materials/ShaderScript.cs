using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderScript : MonoBehaviour
{


    public Material[] newMaterials;
    public Material[] activeMaterials;

    public Material[] pirateMaterials;
    public Material[] farmMaterials;
    public Material[] romanMaterials;
    private List<Material> allMaterials = new List<Material>();
    private Coroutine dissolveCoroutine;

    // Define the Y-level dissolve ranges for each arena
    private Dictionary<Material[], Vector2> materialYRanges = new Dictionary<Material[], Vector2>();

    private void Start()
    {
        //OLD

        allMaterials.AddRange(pirateMaterials);
        allMaterials.AddRange(farmMaterials);
        allMaterials.AddRange(romanMaterials);

        // Define the Y dissolve ranges for each material set
        materialYRanges.Add(pirateMaterials, new Vector2(25f, -10f));  // Pirate Y range: 25 to -10
        materialYRanges.Add(farmMaterials, new Vector2(1f, -1.5f));    // Farm Y range (example): 1 to -0.5
        materialYRanges.Add(romanMaterials, new Vector2(0.2f, -0.1f)); // Roman Y range: 0.2 to -0.1
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ChangeArena(romanMaterials, -0.1f, 0.2f);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            ChangeArena(farmMaterials, 0.7f, -0.2f);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            ChangeArena(pirateMaterials, 25f, -10f);
        }
    }
    public void ChangeArena(Material[] desiredArena, float stopY, float startY)
    {
        if (dissolveCoroutine != null)
        {
            StopCoroutine(dissolveCoroutine);
        }

        dissolveCoroutine = StartCoroutine(HandleArenaTransition(desiredArena, stopY, startY));
    }

    private IEnumerator HandleArenaTransition(Material[] desiredArena, float stopY, float startY)
    {
        // Phase 1: Fully dissolve the active arena
        if (activeMaterials != null && activeMaterials.Length > 0)
        {
            yield return StartCoroutine(DissolveArena(activeMaterials, dissolveOut: true));
        }

        // Phase 2: Fully reveal the desired arena
        yield return StartCoroutine(DissolveArena(desiredArena, dissolveOut: false));

        // Set the new materials as the currently active ones
        activeMaterials = desiredArena;
    }

    private IEnumerator DissolveArena(Material[] arenaMaterials, bool dissolveOut)
    {
        float duration = 3f;
        float timeElapsed = 0f;

        // Get the Y range for the arena (target range)
        Vector2 yRange = materialYRanges[arenaMaterials];
        float startY = dissolveOut ? yRange.x : yRange.y; // Start dissolve from max (dissolve out) or min (dissolve in)
        float endY = dissolveOut ? yRange.y : yRange.x;   // End dissolve at min (dissolve out) or max (dissolve in)

        Vector3[] initialOffsets = new Vector3[arenaMaterials.Length];
        Vector3[] targetOffsets = new Vector3[arenaMaterials.Length];

        // Store initial Y offsets and set target offsets for dissolving
        for (int i = 0; i < arenaMaterials.Length; i++)
        {
            initialOffsets[i] = arenaMaterials[i].GetVector("_DissolveOffest");
            targetOffsets[i] = new Vector3(initialOffsets[i].x, endY, initialOffsets[i].z);
        }

        // Dissolve over time
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(timeElapsed / duration);

            for (int i = 0; i < arenaMaterials.Length; i++)
            {
                Vector3 newOffset = Vector3.Lerp(initialOffsets[i], targetOffsets[i], t);
                arenaMaterials[i].SetVector("_DissolveOffest", newOffset);
            }

            yield return null; // Wait for the next frame
        }

        // Ensure final Y values are set after the dissolve
        for (int i = 0; i < arenaMaterials.Length; i++)
        {
            arenaMaterials[i].SetVector("_DissolveOffest", targetOffsets[i]);
        }
    }
}
