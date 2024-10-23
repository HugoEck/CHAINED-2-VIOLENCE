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
            if (weaponManager != null)
            {
                weaponManager.PickupWeapon(weaponId);

                // Notify the WeaponPlacer that the spawn point is now free
                weaponPlacer.WeaponPickedUp(spawnPoint);

                Destroy(gameObject);  
            }
        }
    }
}