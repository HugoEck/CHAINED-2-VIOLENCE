using System;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum WeaponType
    {
        Unarmed, // Don't set this for the weapons, it's just default value for the combo manager

        TwoHanded,
        OneHanded,
        ReallyBigTwoHanded,
        Polearm,
        Dagger,
        BigPen
    }
    [SerializeField] private GameObject OneHandSlashEffect;
    [SerializeField] private GameObject TwoHandSlashEffect;
    [SerializeField] private GameObject ReallyBigTwoHandSlashEffect;

    public WeaponType currentWeaponType = WeaponType.TwoHanded;
    public ComboAttackSO[] combos;
    public GameObject[] weaponSlashEffects;
    public Transform playerPosition;

    public int weaponId;
    public string weaponName;
    public float damage;
    public float durability;

    public float attackRange;
    public float knockback;
    public float stunDuration;

    private void Start()
    {
        AssignWeaponSlashEffect();
    }
    public void DecreaseDurability()
    {
        durability -= 1;
    }

    private void AssignWeaponSlashEffect()
    {
        switch (currentWeaponType)
        {
            case WeaponType.TwoHanded:
                weaponSlashEffects = ReturnAllSlashEffectChildren(TwoHandSlashEffect);
                break;
            case WeaponType.OneHanded:
                weaponSlashEffects = ReturnAllSlashEffectChildren(OneHandSlashEffect);
                break;
            case WeaponType.ReallyBigTwoHanded:
                weaponSlashEffects = ReturnAllSlashEffectChildren(ReallyBigTwoHandSlashEffect);
                break;
            case WeaponType.Polearm:
                // Uncomment if PolearmSlashEffect is added
                // weaponSlashEffects = ReturnAllSlashEffectChildren(PolearmSlashEffect);
                break;
            case WeaponType.Dagger:
                // Uncomment if DaggerSlashEffect is added
                // weaponSlashEffects = ReturnAllSlashEffectChildren(DaggerSlashEffect);
                break;
            case WeaponType.BigPen:
                // Uncomment if BigPenSlashEffect is added
                // weaponSlashEffects = ReturnAllSlashEffectChildren(BigPenSlashEffect);
                break;

            default:
                Debug.LogWarning("Unhandled weapon type: " + currentWeaponType);
                weaponSlashEffects = new GameObject[0];
                break;
        }
    }
    private GameObject[] ReturnAllSlashEffectChildren(GameObject weaponSlashObject)
    {
        // Create a list to store the children
        List<GameObject> allWeaponSlashObjectChildren = new List<GameObject>();

        // Loop through each child of the provided GameObject
        foreach (Transform child in weaponSlashObject.transform)
        {
            // Add the child GameObject to the list
            allWeaponSlashObjectChildren.Add(child.gameObject);
        }

        // Convert the list to an array and return it
        return allWeaponSlashObjectChildren.ToArray();
    }

}
