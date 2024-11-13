using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ClassWeaponParent
{
    public PlayerCombat.PlayerClass playerClass;
    public Transform weaponsParent;
}

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private List<ClassWeaponParent> classWeaponsParentsList;
    private Dictionary<PlayerCombat.PlayerClass, Transform> classWeaponsParents = new Dictionary<PlayerCombat.PlayerClass, Transform>();

    private GameObject currentWeapon;
    private Dictionary<int, GameObject> weaponDictionary = new Dictionary<int, GameObject>();

    public bool hasWeapon => currentWeapon != null;

    private void Start()
    {
        // Convert List to Dictionary for faster lookups
        foreach (var entry in classWeaponsParentsList)
        {
            if (!classWeaponsParents.ContainsKey(entry.playerClass))
            {
                classWeaponsParents.Add(entry.playerClass, entry.weaponsParent);
            }
        }

        // Load weapons for the default class, if available
        if (classWeaponsParents.TryGetValue(PlayerCombat.PlayerClass.Default, out var defaultWeaponsParent))
        {
            LoadWeapons(defaultWeaponsParent);
        }
    }

    /// <summary>
    /// Loads weapons from the specified parent and populates weaponDictionary.
    /// </summary>
    private void LoadWeapons(Transform weaponsParent)
    {
        weaponDictionary.Clear();

        if (weaponsParent == null)
        {
            Debug.LogError("Weapons parent is not assigned in WeaponManager.");
            return;
        }

        foreach (Transform category in weaponsParent)
        {
            foreach (Transform weaponTransform in category)
            {
                GameObject weaponObject = weaponTransform.gameObject;
                Weapon weapon = weaponObject.GetComponent<Weapon>();

                if (weapon != null)
                {
                    weaponDictionary[weapon.weaponId] = weaponObject;
                    weaponObject.SetActive(false);  // Deactivate initially
                }
            }
        }
    }

    /// <summary>
    /// Call this method when the class changes to load weapons from the new class's weapon parent.
    /// </summary>
    public void OnClassSwitch(PlayerCombat.PlayerClass newClass)
    {
        if (classWeaponsParents.TryGetValue(newClass, out Transform newWeaponsParent))
        {
            LoadWeapons(newWeaponsParent);
            currentWeapon = null;
        }
        else
        {
            Debug.LogWarning("No weapon parent found for class: " + newClass);
        }
    }

    public void EquipWeapon(int weaponId)
    {
        if (currentWeapon != null)
        {
            currentWeapon.SetActive(false);
        }

        if (weaponDictionary.TryGetValue(weaponId, out currentWeapon))
        {
            currentWeapon.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"Weapon with ID {weaponId} not found in weaponDictionary.");
            currentWeapon = null;
        }
    }

    public void Attack()
    {
        if (currentWeapon != null)
        {
            Weapon weaponScript = currentWeapon.GetComponent<Weapon>();
            weaponScript.Attack();

            if (weaponScript.durability <= 0)
            {
                BreakWeapon();
            }
        }
    }

    private void BreakWeapon()
    {
        if (currentWeapon != null)
        {
            currentWeapon.SetActive(false);
            currentWeapon = null;
        }
    }

    public void PickupWeapon(int weaponId)
    {
        EquipWeapon(weaponId);
    }
}
