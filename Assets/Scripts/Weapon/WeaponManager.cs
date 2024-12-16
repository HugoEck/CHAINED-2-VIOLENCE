using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[Serializable]
public class ClassWeaponParent
{
    public PlayerCombat.PlayerClass playerClass;
    public Transform weaponsParent;
}

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private List<ClassWeaponParent> classWeaponsParentsList;
    private Dictionary<PlayerCombat.PlayerClass, Transform> classWeaponsParents = new Dictionary<PlayerCombat.PlayerClass, Transform>();
    private GameObject currentWeapon;
    private Dictionary<int, GameObject> weaponDictionary = new Dictionary<int, GameObject>();
    public bool hasWeapon => currentWeapon != null;

    public event Action<GameObject> OnWeaponEquipped;
    public event Action<GameObject> OnWeaponBroken;

    private float startDurability;
    private void Start()
    {
        foreach (var entry in classWeaponsParentsList)
        {
            if (!classWeaponsParents.ContainsKey(entry.playerClass))
            {
                classWeaponsParents.Add(entry.playerClass, entry.weaponsParent);
            }
            
        }
        if (classWeaponsParents.TryGetValue(playerCombat.currentPlayerClass, out var defaultWeaponsParent))
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
                    weaponObject.SetActive(false);
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
            startDurability = currentWeapon.GetComponent<Weapon>().durability;
            currentWeapon.SetActive(true);

            // Get the current weapon and apply combo stats to it from ComboAttack script
            OnWeaponEquipped?.Invoke(currentWeapon);
        }
        else
        {
            Debug.LogWarning($"Weapon with ID {weaponId} not found in weaponDictionary.");
            currentWeapon = null;
        }
    }

    public void ReduceWeaponDurability()
    {
        if (currentWeapon != null)
        {
            Weapon weaponScript = currentWeapon.GetComponent<Weapon>();
            weaponScript.DecreaseDurability();

            if (weaponScript.durability <= 0)
            {
                weaponScript.durability = startDurability;
                BreakWeapon();
            }
        }
    }

    private void BreakWeapon()
    {
        if (currentWeapon != null)
        {
            currentWeapon.SetActive(false);

            OnWeaponBroken?.Invoke(currentWeapon);

            currentWeapon = null;
        }
    }

    public void PickupWeapon(int weaponId)
    {
        EquipWeapon(weaponId);
    }

    public void DropWeapon()
    {
        if (currentWeapon == null)
        {
            Debug.LogWarning("No weapon equipped to drop.");
            return;
        }

        // Create a temporary dropped weapon
        GameObject droppedWeapon = Instantiate(currentWeapon, playerCombat.transform.position, Quaternion.identity);

        // Add Rigidbody and Collider for the dropped weapon to enable physics
        Rigidbody rb = droppedWeapon.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = droppedWeapon.AddComponent<Rigidbody>();
        }
        rb.isKinematic = false; // Enable physics

        Collider col = droppedWeapon.GetComponent<Collider>();
        if (col == null)
        {
            col = droppedWeapon.AddComponent<BoxCollider>();
        }

        // Apply a forward and upward force to "throw" the weapon
        Vector3 throwDirection = playerCombat.transform.forward + Vector3.up * 0.5f;
        rb.AddForce(throwDirection * 5f, ForceMode.Impulse); // Adjust throw force as needed

        // Deactivate the current weapon in the player's inventory
        currentWeapon.SetActive(false);
        OnWeaponBroken?.Invoke(currentWeapon);
        currentWeapon = null;

        // Destroy the dropped weapon after a delay to prevent clutter
        Destroy(droppedWeapon, 5f);
    }
}
