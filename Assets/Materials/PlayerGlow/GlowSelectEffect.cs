using UnityEngine;

public class GlowEffectSelect : MonoBehaviour
{
    public Color GlowColor = Color.yellow;
    public float GlowThickness = 5f;

    private Material glowMaterial;

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            glowMaterial = renderer.material;
            glowMaterial.SetColor("_GlowColor", GlowColor);
            glowMaterial.SetFloat("_GlowThickness", GlowThickness);
        }
    }

    public void SetGlow(Color color, float thickness)
    {
        if (glowMaterial != null)
        {
            glowMaterial.SetColor("_GlowColor", color);
            glowMaterial.SetFloat("_GlowThickness", thickness);
        }
    }
}
