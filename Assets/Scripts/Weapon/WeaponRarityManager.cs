using UnityEngine;
using HighlightPlus;

public class WeaponRarityHandler : MonoBehaviour
{
    public enum WeaponRarity
    {
        Common,
        Rare,
        Epic,
        Legendary
    }

    [SerializeField] WeaponRarity weaponRarity;
    private HighlightEffect highlightEffect;
    private Light weaponLight;
    private ParticleSystem weaponParticles;

    private void Awake()
    {
        highlightEffect = GetComponent<HighlightEffect>();
        weaponLight = GetComponentInChildren<Light>();
        weaponParticles = GetComponentInChildren<ParticleSystem>();

        UpdateGlowColor();
    }

    public void UpdateGlowColor()
    {
        Color glowColor = GetGlowColor();

        if (highlightEffect != null)
        {
            highlightEffect.SetGlowColor(glowColor);
        }
        if (weaponLight != null)
        {
            weaponLight.color = glowColor;
        }
        if (weaponParticles != null)
        {
            var mainModule = weaponParticles.main;
            mainModule.startColor = glowColor;
        }
    }

    private Color GetGlowColor()
    {
        switch (weaponRarity)
        {
            case WeaponRarity.Common:
                return Color.green;
            case WeaponRarity.Rare:
                return Color.blue;
            case WeaponRarity.Epic:
                return Color.magenta;
            case WeaponRarity.Legendary:
                return Color.yellow;
            default:
                return Color.white;
        }
    }
}
