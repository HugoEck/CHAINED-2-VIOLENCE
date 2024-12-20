using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveManager : MonoBehaviour
{
    public WaveManager waveManager;
    public CinemachineFreeLook cam;
    [SerializeField] itemAreaSpawner itemSpawner;

    float zoomedOutFov = 120;
    float normalFov = 40;

    [SerializeField] private AudioClip dissolveClip;

    public Material[] newMaterials;
    public Material[] activeMaterials;

    public Material[] pirateMaterials;
    public Material[] farmMaterials;
    public Material[] romanMaterials;
    public Material[] westernMaterials;
    public Material[] fantasyMaterials;
    public Material[] currentDayMaterials;
    public Material[] sciFiMaterials;
    public Material[] corruptedMaterials;
    private List<Material> allMaterials = new List<Material>();
    private Coroutine dissolveCoroutine;

    public GameObject romanArena;
    public GameObject fantasyArena;
    public GameObject pirateArena;
    public GameObject westernArena;
    public GameObject farmArena;
    public GameObject currentDayArena;
    public GameObject sciFiArena;
    public GameObject corruptedArena;

    public GameObject floor;
    public Material startFloorMaterial;
    public Material romanFloorMaterial;
    public Material fantasyFloorMaterial;
    public Material pirateFloorMaterial;
    public Material westernFloorMaterial;
    public Material farmFloorMaterial;
    public Material currentDayFloorMaterial;
    public Material sciFiFloorMaterial;
    public Material corruptedFloorMaterial;
    Renderer floorRenderer;
    Material currentFloorMaterial;

    private bool hasChangedToRoman = false;
    private bool hasChangedToFarm = false;
    private bool hasChangedToPirate = false;
    private bool hasChangedToWestern = false;
    private bool hasChangedToFantasy = false;
    private bool hasChangedToCurrentDay = false;
    private bool hasChangedToSciFi = false;
    private bool hasChangedToCorrupted = false;

    public bool isChangingArena = true;

    float initialOffsetsPirate = -10f;
    float initialOffsetsFarm = -3f;
    float initialOffsetsRoman = -0.1f;
    float initialOffsetsWestern = -1.2f;
    float initialOffsetsFantasy = -6.2f;
    float initialOffsetsCurrentDay = -3f;
    float initialOffsetsSciFi = -5f;
    float initialCorrupted = -100f;

    // Define the Y-level dissolve ranges for each arena
    private Dictionary<Material[], Vector2> materialYRanges = new Dictionary<Material[], Vector2>();

    private void Start()
    {
        cam = FindObjectOfType<CinemachineFreeLook>();
        floorRenderer = floor.GetComponent<Renderer>();
        currentFloorMaterial = romanFloorMaterial;

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
        materialYRanges.Add(sciFiMaterials, new Vector2(15, initialOffsetsSciFi)); // Roman Y range: 0.2 to -0.1
        materialYRanges.Add(corruptedMaterials, new Vector2(0.05f, initialCorrupted)); // Roman Y range: 0.2 to -0.1

    }

    private void Update()
    {
        //Debug.Log(WaveManager.currentWave);
        if (WaveManager.currentWave == 0 && !hasChangedToRoman)
        {
            isChangingArena = true;

            StartCoroutine(ZoomOutcamera());
            romanArena.SetActive(true);
            
            

            hasChangedToRoman = true;
        }
        if (WaveManager.currentWave == 5 && !hasChangedToFantasy)
        {
            isChangingArena = true;
            Debug.Log("now i change arena");

            fantasyArena.SetActive(true);
            ChangeArena(fantasyMaterials, 25f, initialOffsetsFantasy, romanArena);
            StartCoroutine(DissolveAndChangeMaterial(romanFloorMaterial, fantasyFloorMaterial));

            hasChangedToFantasy = true;
        }
        if (WaveManager.currentWave == 10 && !hasChangedToPirate)
        {
            isChangingArena = true;

            pirateArena.SetActive(true);
            ChangeArena(pirateMaterials, 25f, initialOffsetsPirate, fantasyArena);
            StartCoroutine(DissolveAndChangeMaterial(fantasyFloorMaterial, pirateFloorMaterial));

            hasChangedToPirate = true;
        }
        if (WaveManager.currentWave == 15 && !hasChangedToWestern)
        {
            isChangingArena = true;

            westernArena.SetActive(true);
            ChangeArena(westernMaterials, 25f, initialOffsetsWestern, pirateArena);
            StartCoroutine(DissolveAndChangeMaterial(pirateFloorMaterial, westernFloorMaterial));

            hasChangedToWestern = true;
        }
        if (WaveManager.currentWave == 20 && !hasChangedToFarm)
        {
            isChangingArena = true;

            farmArena.SetActive(true);
            ChangeArena(farmMaterials, 10f, initialOffsetsFarm, westernArena);
            StartCoroutine(DissolveAndChangeMaterial(westernFloorMaterial, farmFloorMaterial));
            hasChangedToFarm = true;
        }
        if (WaveManager.currentWave == 25 && !hasChangedToCurrentDay)
        {
            isChangingArena = true;

            currentDayArena.SetActive(true);
            ChangeArena(currentDayMaterials, 3f, initialOffsetsCurrentDay, farmArena);
            StartCoroutine(DissolveAndChangeMaterial(farmFloorMaterial, currentDayFloorMaterial));

            hasChangedToCurrentDay = true;
        }
        if (WaveManager.currentWave == 30 && !hasChangedToSciFi)
        {
            isChangingArena = true;

            sciFiArena.SetActive(true);
            ChangeArena(sciFiMaterials, 15, initialOffsetsSciFi, currentDayArena);
            StartCoroutine(DissolveAndChangeMaterial(currentDayFloorMaterial, sciFiFloorMaterial));

            hasChangedToSciFi = true;
        }
        if (WaveManager.currentWave == 36 && !hasChangedToCorrupted)
        {
            isChangingArena = true;

            corruptedArena.SetActive(true);
            ChangeArena(corruptedMaterials, 0.05f, initialCorrupted, sciFiArena);
            StartCoroutine(DissolveAndChangeCorruptedMaterial(sciFiFloorMaterial, corruptedFloorMaterial));

            hasChangedToCorrupted = true;
        }

    }
    public void ChangeArena(Material[] desiredArena, float stopY, float startY, GameObject oldArena)
    {
        if (dissolveCoroutine != null)
        {
            StopCoroutine(dissolveCoroutine);
        }
        dissolveCoroutine = StartCoroutine(HandleArenaTransition(desiredArena, stopY, startY, oldArena));
    }

    private IEnumerator HandleArenaTransition(Material[] desiredArena, float stopY, float startY, GameObject oldArena)
    {
        


        

        // Phase 1: Fully dissolve the active arena
        if (activeMaterials != null && activeMaterials.Length > 0)
        {
            SFXManager.instance.PlaySFXClip(dissolveClip, transform, 1f);
            yield return StartCoroutine(DissolveArena(activeMaterials, dissolveOut: true));
        }
        oldArena.SetActive(false);
        // Phase 2: Fully reveal the desired arena
        SFXManager.instance.PlaySFXClip(dissolveClip, transform, 1f);
        yield return StartCoroutine(DissolveArena(desiredArena, dissolveOut: false));
        isChangingArena = false;
        
        // Set the new materials as the currently active ones
        activeMaterials = desiredArena;
    }

    private IEnumerator ZoomIncamera()
    {
        float time = 0;

        while (time < 3)
        {
            cam.m_Lens.FieldOfView = Mathf.Lerp(zoomedOutFov, normalFov, time / 1.5f);
            yield return null;
            time += Time.deltaTime;
        }
    }
    private IEnumerator ZoomOutcamera()
    {
        float time = 0;

        while (time < 3)
        {
            cam.m_Lens.FieldOfView = Mathf.Lerp(normalFov, zoomedOutFov, time / 1.5f);
            yield return null;
            time += Time.deltaTime;
        }
        ChangeArena(romanMaterials, initialOffsetsRoman, 0.2f, sciFiArena);
        itemSpawner.SpawnRomanObjects();
        StartCoroutine(InitialDissolveAndChangeMaterial(romanFloorMaterial));
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

    private IEnumerator DissolveAndChangeMaterial(Material oldMaterial, Material newMaterial)
    {
        float dissolveValue;
        // Phase 1: Dissolve from 0 to 100
        currentFloorMaterial = oldMaterial;
            dissolveValue = 0;
            while (dissolveValue < 1)
            {
                dissolveValue += Time.deltaTime / 3f;
                UpdateDissolve(dissolveValue);
                yield return null;
            }
        
        // Change material after dissolve reaches 100%
        floorRenderer.material = newMaterial; // You can change this to any material you'd like

        currentFloorMaterial = newMaterial;
        oldMaterial.SetFloat("_Dissolve", 0);
        // Phase 2: Dissolve from 100 to 0
        dissolveValue = 1;
        while (dissolveValue > 0)
        {
            dissolveValue -= Time.deltaTime / 3f;
            UpdateDissolve(dissolveValue);
            yield return null;
        }

    }

    private IEnumerator DissolveAndChangeCorruptedMaterial(Material oldMaterial, Material newMaterial)
    {
        float dissolveValue;
        // Phase 1: Dissolve from 0 to 100
        currentFloorMaterial = oldMaterial;
        dissolveValue = 0;
        while (dissolveValue < 1)
        {
            dissolveValue += Time.deltaTime / 3f;
            UpdateDissolve(dissolveValue);
            yield return null;
        }

        // Change material after dissolve reaches 100%
        floorRenderer.material = newMaterial; // You can change this to any material you'd like

        currentFloorMaterial = newMaterial;
        oldMaterial.SetFloat("_Dissolve", 0);
        // Phase 2: Dissolve from 100 to 0
        dissolveValue = 1;
        while (dissolveValue > 0.3)
        {
            dissolveValue -= Time.deltaTime / 3f;
            UpdateDissolve(dissolveValue);
            yield return null;
        }

    }

    private IEnumerator InitialDissolveAndChangeMaterial(Material newMaterial)
    {
        float dissolveValue;
        // Phase 1: Dissolve from 0 to 100
        

        // Change material after dissolve reaches 100%
        floorRenderer.material = newMaterial; // You can change this to any material you'd like

        currentFloorMaterial = newMaterial;
        // Phase 2: Dissolve from 100 to 0
        dissolveValue = 1;
        while (dissolveValue > 0)
        {
            dissolveValue -= Time.deltaTime / 3f;
            UpdateDissolve(dissolveValue);
            yield return null;
        }

        yield return new WaitForSeconds(1.5f);
        StartCoroutine(ZoomIncamera());
    }

    private void UpdateDissolve(float dissolveValue)
    {
        // Ensure the material has a dissolve property to update

        currentFloorMaterial.SetFloat("_Dissolve", dissolveValue);

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

        romanFloorMaterial.SetFloat("_Dissolve", 1);
        fantasyFloorMaterial.SetFloat("_Dissolve", 1);
        pirateFloorMaterial.SetFloat("_Dissolve", 1);
        westernFloorMaterial.SetFloat("_Dissolve", 1);
        farmFloorMaterial.SetFloat("_Dissolve", 1);
        currentDayFloorMaterial.SetFloat("_Dissolve", 1);
        sciFiFloorMaterial.SetFloat("_Dissolve", 1);
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
