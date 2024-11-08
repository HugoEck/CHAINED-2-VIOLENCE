using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public int weaponId; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            WeaponManager weaponManager = other.GetComponent<WeaponManager>();
            if (weaponManager != null)
            {
                weaponManager.PickupWeapon(weaponId);
                Destroy(gameObject);  
            }
        }
    }
}