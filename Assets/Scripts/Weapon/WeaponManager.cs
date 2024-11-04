using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private GameObject[] availableWeapons; 
    private GameObject currentWeapon;
    private Dictionary<int, GameObject> weaponDictionary = new Dictionary<int, GameObject>();

    public bool hasWeapon => currentWeapon != null;

    private void Start()
    {

        foreach (GameObject weaponObject in availableWeapons)
        {
            Weapon weapon = weaponObject.GetComponent<Weapon>();
            if (weapon != null)
            {
                weaponDictionary[weapon.weaponId] = weaponObject;  
                weaponObject.SetActive(false); 
            }
        }
    }


    public void EquipWeapon(int weaponId)
    {
        
        if (currentWeapon != null)
        {
            currentWeapon.SetActive(false);
        }

        currentWeapon = weaponDictionary[weaponId];
        currentWeapon.SetActive(true);

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
        currentWeapon.SetActive(false); 
        currentWeapon = null; 
    }

    public void PickupWeapon(int weaponId)
    {
        EquipWeapon(weaponId);  
    }
}
