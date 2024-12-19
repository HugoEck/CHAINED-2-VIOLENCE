using UnityEngine;
using UnityEngine.UI;

public class WeaponUIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image weaponIcon; // Assign the icon image here
    [SerializeField] private Image noWeaponIcon; // Assign the "X" icon image here

    [Header("Gradient Colors")]
    [SerializeField] private Gradient durabilityGradient; // Customize the gradient in the inspector

    [Header("Player References")]
    [SerializeField] private WeaponManager player1WeaponManager; // Reference to Player 1's WeaponManager
    [SerializeField] private WeaponManager player2WeaponManager; // Reference to Player 2's WeaponManager

    private WeaponManager currentWeaponManager;

    private void Update()
    {
        // Determine which player's weapon manager to use
        currentWeaponManager = GetCurrentPlayerWeaponManager();

        if (currentWeaponManager != null)
        {
            UpdateWeaponUI();
        }
    }

    private void UpdateWeaponUI()
    {
        // Check if the player has a weapon
        if (currentWeaponManager.hasWeapon)
        {
            weaponIcon.gameObject.SetActive(true);
            noWeaponIcon.gameObject.SetActive(false);

            float durability = GetWeaponDurability();

            // Update the color based on durability
            weaponIcon.color = durabilityGradient.Evaluate(durability);

            // If the weapon is broken, show the "X"
            if (durability <= 0)
            {
                weaponIcon.gameObject.SetActive(false);
                noWeaponIcon.gameObject.SetActive(true);
            }
        }
        else
        {
            // Show the "X" if no weapon is equipped
            weaponIcon.gameObject.SetActive(false);
            noWeaponIcon.gameObject.SetActive(true);
        }
    }

    private WeaponManager GetCurrentPlayerWeaponManager()
    {
        // Logic to determine the current player; replace this with your player-switching logic
        // Example: Check input or active player indicator
        bool isPlayer1Active = true; // Replace with actual logic

        return isPlayer1Active ? player1WeaponManager : player2WeaponManager;
    }

    private bool PlayerHasWeapon()
    {
        return currentWeaponManager != null && currentWeaponManager.hasWeapon;
    }

    private float GetWeaponDurability()
    {
        if (currentWeaponManager != null && currentWeaponManager.hasWeapon)
        {
            Weapon weaponScript = currentWeaponManager.GetComponentInChildren<Weapon>();
            if (weaponScript != null)
            {
                return 0f; //weaponScript.durability / weaponScript.maxDurability;
            }
        }
        return 0f;
    }
}
