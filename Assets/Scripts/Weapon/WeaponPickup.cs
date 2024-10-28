using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public int weaponId;
    public Transform spawnPoint;  // Reference to the spawn point where this weapon was placed
    private WeaponSpawner weaponPlacer;  // Reference to the WeaponPlacer script

    private void Start()
    {
        weaponPlacer = FindObjectOfType<WeaponSpawner>();  // Get reference to WeaponPlacer in the scene
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            WeaponManager weaponManager = other.GetComponent<WeaponManager>();

            if (weaponManager != null && !weaponManager.hasWeapon)
            {
                weaponManager.PickupWeapon(weaponId);

                // Notify WeaponSpawner to free the spawn point
                WeaponSpawner spawner = FindObjectOfType<WeaponSpawner>();
                spawner.WeaponPickedUp(transform);  // Pass the weapon's current spawn point

                Destroy(gameObject);  // Destroy the weapon after pickup
            }
        }
    }
}