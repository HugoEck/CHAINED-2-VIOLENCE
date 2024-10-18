using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderScript : MonoBehaviour
{
    public Material[] pirateMaterials;
    public Material[] farmMaterials;
    public Material[] romanMaterials;
    List<Material> allMaterials = new List<Material>();
    private Coroutine dissolveCoroutine;
    private void Start()
    {

        allMaterials.AddRange(pirateMaterials);
        allMaterials.AddRange(farmMaterials);
        allMaterials.AddRange(romanMaterials);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ChangeArena(romanMaterials);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            ChangeArena(farmMaterials);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            ChangeArena(pirateMaterials);
        }
    }
    public void ChangeArena(Material[] desiredArena)
    {
        if (dissolveCoroutine != null)
        {
            StopCoroutine(dissolveCoroutine);
        }

        dissolveCoroutine = StartCoroutine(ChangeDissolveOffset(desiredArena));
    }

    private IEnumerator ChangeDissolveOffset(Material[] desiredArena)
    {
        // Create a HashSet for faster lookup of desired materials
        HashSet<Material> desiredMaterialsSet = new HashSet<Material>(desiredArena);
        float duration = 10f; // Duration of the transition
        float timeElapsed = 0f;

        // Store initial offsets
        Vector3[] initialOffsets = new Vector3[allMaterials.Count];
        Vector3[] targetOffsets = new Vector3[allMaterials.Count];

        for (int i = 0; i < allMaterials.Count; i++)
        {
            initialOffsets[i] = allMaterials[i].GetVector("_DissolveOffest");
            if (desiredMaterialsSet.Contains(allMaterials[i]))
            {
                targetOffsets[i] = new Vector3(initialOffsets[i].x, 0, initialOffsets[i].z);
            }
            else
            {
                targetOffsets[i] = new Vector3(initialOffsets[i].x, 100, initialOffsets[i].z);
            }
        }

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(timeElapsed / duration);

            for (int i = 0; i < allMaterials.Count; i++)
            {
                Vector3 newOffset = Vector3.Lerp(initialOffsets[i], targetOffsets[i], t);
                allMaterials[i].SetVector("_DissolveOffest", newOffset);
            }

            yield return null; // Wait for the next frame
        }

        // Ensure final values are set
        for (int i = 0; i < allMaterials.Count; i++)
        {
            allMaterials[i].SetVector("_DissolveOffest", targetOffsets[i]);
        }
    }
}
