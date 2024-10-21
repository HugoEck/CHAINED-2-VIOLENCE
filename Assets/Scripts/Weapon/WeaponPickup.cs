using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public int weaponId;  // Unique ID of the weapon being picked up

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            WeaponManager weaponManager = other.GetComponent<WeaponManager>();
            if (weaponManager != null)
            {
                weaponManager.PickupWeapon(weaponId);  // Equip the weapon by ID
                Destroy(gameObject);  // Destroy the pickup object after it's collected
            }
        }
    }
}