using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public int weaponId;
    private WeaponSpawner weaponSpawner;

    private void Start()
    {
        weaponSpawner = FindObjectOfType<WeaponSpawner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            WeaponManager weaponManager = other.GetComponent<WeaponManager>();

            if (weaponManager != null && !weaponManager.hasWeapon)
            {
                weaponManager.PickupWeapon(weaponId);
                weaponSpawner.WeaponPickedUp(transform);
                Destroy(gameObject);
            }
        }
    }
}
