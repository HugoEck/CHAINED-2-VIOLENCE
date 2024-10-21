using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private GameObject[] availableWeapons;  // All pre-loaded weapons attached to the player
    private GameObject currentWeapon;  // Currently equipped weapon
    private Dictionary<int, GameObject> weaponDictionary = new Dictionary<int, GameObject>();  // Mapping weaponId to weapon GameObject

    private void Start()
    {
        // Initialize weapon dictionary and disable all weapons at start
        foreach (GameObject weaponObject in availableWeapons)
        {
            Weapon weapon = weaponObject.GetComponent<Weapon>();
            if (weapon != null)
            {
                weaponDictionary[weapon.weaponId] = weaponObject;  // Map weaponId to the weapon GameObject
                weaponObject.SetActive(false);  // Disable all weapons initially
            }
        }
    }

    // Equip weapon by ID
    public void EquipWeapon(int weaponId)
    {
        if (!weaponDictionary.ContainsKey(weaponId))
        {
            Debug.LogError("Weapon ID not found: " + weaponId);
            return;
        }

        // Disable current weapon
        if (currentWeapon != null)
        {
            currentWeapon.SetActive(false);
        }

        // Equip new weapon by ID
        currentWeapon = weaponDictionary[weaponId];
        currentWeapon.SetActive(true);
        Debug.Log("Equipped weapon: " + currentWeapon.GetComponent<Weapon>().weaponName);
    }

    // Attack with the currently equipped weapon
    public void Attack()
    {
        if (currentWeapon != null)
        {
            Weapon weaponScript = currentWeapon.GetComponent<Weapon>();
            weaponScript.Attack();

            if (weaponScript.durability <= 0)
            {
                BreakWeapon();  // WeaponManager handles the breaking
            }
        }
        else
        {
            Debug.Log("No weapon equipped!");
        }
    }

    private void BreakWeapon()
    {
        Debug.Log(currentWeapon.GetComponent<Weapon>().weaponName + " broke!");
        currentWeapon.SetActive(false);  // Disable the broken weapon
        currentWeapon = null;  // No weapon equipped after breaking
    }

    // Pick up weapon by ID
    public void PickupWeapon(int weaponId)
    {
        EquipWeapon(weaponId);  // Equip the weapon based on its ID
    }
}
