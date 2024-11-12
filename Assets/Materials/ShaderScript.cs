using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderScript : MonoBehaviour
{
    public WaveManager waveManager;

    public Material[] newMaterials;
    public Material[] activeMaterials;

    public Material[] pirateMaterials;
    public Material[] farmMaterials;
    public Material[] romanMaterials;
    public Material[] westernMaterials;
    public Material[] fantasyMaterials;
    public Material[] currentDayMaterials;
    public Material[] sciFiMaterials;
    private List<Material> allMaterials = new List<Material>();
    private Coroutine dissolveCoroutine;

    private bool hasChangedToRoman = false;
    private bool hasChangedToFarm = false;
    private bool hasChangedToPirate = false;
    private bool hasChangedToWestern = false;
    private bool hasChangedToFantasy = false;
    private bool hasChangedToCurrentDay = false;
    private bool hasChangedToSciFi = false;

    float initialOffsetsPirate = -10f;
    float initialOffsetsFarm = -0.3f;
    float initialOffsetsRoman = -0.1f;
    float initialOffsetsWestern = -1.2f;
    float initialOffsetsFantasy = -6.2f;
    float initialOffsetsCurrentDay = -0.1f;
    float initialOffsetsSciFi = -60f;

    // Define the Y-level dissolve ranges for each arena
    private Dictionary<Material[], Vector2> materialYRanges = new Dictionary<Material[], Vector2>();

    private void Start()
    {
        

        allMaterials.AddRange(pirateMaterials);
        allMaterials.AddRange(farmMaterials);
        allMaterials.AddRange(romanMaterials);
        allMaterials.AddRange(westernMaterials);
        allMaterials.AddRange(fantasyMaterials);
        allMaterials.AddRange(currentDayMaterials);
        allMaterials.AddRange(sciFiMaterials);

        // Define the Y dissolve ranges for each material set
        materialYRanges.Add(pirateMaterials, new Vector2(25f, initialOffsetsPirate));  // Pirate Y range: 25 to -10
        materialYRanges.Add(farmMaterials, new Vector2(10f, initialOffsetsFarm));    // Farm Y range (example): 1 to -0.5
        materialYRanges.Add(romanMaterials, new Vector2(0.2f, initialOffsetsRoman)); // Roman Y range: 0.2 to -0.1
        materialYRanges.Add(westernMaterials, new Vector2(25, initialOffsetsWestern)); // Roman Y range: 0.2 to -0.1
        materialYRanges.Add(fantasyMaterials, new Vector2(25f, initialOffsetsFantasy)); // Roman Y range: 0.2 to -0.1
        materialYRanges.Add(currentDayMaterials, new Vector2(3, initialOffsetsCurrentDay)); // Roman Y range: 0.2 to -0.1
        materialYRanges.Add(sciFiMaterials, new Vector2(110, initialOffsetsSciFi)); // Roman Y range: 0.2 to -0.1
    }

    private void Update()
    {
        Debug.Log(WaveManager.currentWave);
        if (WaveManager.currentWave == 1 && !hasChangedToRoman)
        {
            ChangeArena(romanMaterials, initialOffsetsRoman, 0.2f);
            hasChangedToRoman = true;
        }
        if (WaveManager.currentWave == 5 && !hasChangedToFantasy)
        {
            ChangeArena(fantasyMaterials, 25f, initialOffsetsFantasy);
            hasChangedToFantasy = true;
        }
        if (WaveManager.currentWave == 10 && !hasChangedToPirate)
        {
            ChangeArena(pirateMaterials, 25f, initialOffsetsPirate);
            hasChangedToPirate = true;
        }
        if (WaveManager.currentWave == 15 && !hasChangedToWestern)
        {
            ChangeArena(westernMaterials, 25f, initialOffsetsWestern);
            hasChangedToWestern = true;
        }
        if (WaveManager.currentWave == 20 && !hasChangedToWestern)
        {
            ChangeArena(currentDayMaterials, 25f, initialOffsetsCurrentDay);
            hasChangedToCurrentDay = true;
        }
        if (WaveManager.currentWave == 25 && !hasChangedToSciFi)
        {
            ChangeArena(sciFiMaterials, 25f, initialOffsetsSciFi);
            hasChangedToSciFi = true;
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
        float duration = 6f;
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

    private void OnDisable()
    {
        ResetMaterials();
    }

    public void ResetMaterials()
    {
        // Reset all material groups to their initial offsets
        ResetMaterialGroup(pirateMaterials, initialOffsetsPirate);
        ResetMaterialGroup(farmMaterials, initialOffsetsFarm);
        ResetMaterialGroup(romanMaterials, initialOffsetsRoman);
        ResetMaterialGroup(westernMaterials, initialOffsetsWestern);
        ResetMaterialGroup(fantasyMaterials, initialOffsetsFantasy);
        ResetMaterialGroup(currentDayMaterials, initialOffsetsCurrentDay);
        ResetMaterialGroup(sciFiMaterials, initialOffsetsSciFi);

        // Optionally reset your change flags
        hasChangedToRoman = false;
        hasChangedToFarm = false;
        hasChangedToPirate = false;
        hasChangedToWestern = false;
        hasChangedToFantasy = false;
        hasChangedToCurrentDay = false;
        hasChangedToSciFi = false;
    }

    private void ResetMaterialGroup(Material[] materials, float initialOffset)
    {
        if (materials != null && materials.Length > 0)
        {
            for (int i = 0; i < materials.Length; i++)
            {
                // Reset the "_DissolveOffest" parameter to the initial offset for each material
                materials[i].SetVector("_DissolveOffest", new Vector3(0, initialOffset, 0));
            }
        }
    }
}
