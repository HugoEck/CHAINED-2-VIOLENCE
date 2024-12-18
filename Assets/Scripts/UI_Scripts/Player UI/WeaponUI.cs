using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{

    private GameObject player1;
    private GameObject player2;
    private WeaponManager player1WeaponManager;
    private WeaponManager player2WeaponManager;

    [SerializeField] private Image player1WeaponImage; // UI Image for Player 1's weapon
    [SerializeField] private Image player2WeaponImage; // UI Image for Player 2's weapon

    [SerializeField] private Sprite defaultSprite; // Sprite to indicate no weapon is equipped

    [Header("Weapon Type Sprites")]
    [SerializeField] private Sprite unarmedSprite;
    [SerializeField] private Sprite twoHandedSprite;
    [SerializeField] private Sprite oneHandedSprite;
    [SerializeField] private Sprite reallyBigTwoHandedSprite;
    [SerializeField] private Sprite polearmSprite;
    [SerializeField] private Sprite daggerSprite;
    [SerializeField] private Sprite bigPenSprite;

    private Dictionary<Weapon.WeaponType, Sprite> weaponTypeSprites;

    private void Awake()
    {
        player1 = GameObject.FindGameObjectWithTag("Player1");
        player2 = GameObject.FindGameObjectWithTag("Player2");

        // Initialize the dictionary mapping WeaponType to Sprites
        weaponTypeSprites = new Dictionary<Weapon.WeaponType, Sprite>
        {
            { Weapon.WeaponType.Unarmed, unarmedSprite },
            { Weapon.WeaponType.TwoHanded, twoHandedSprite },
            { Weapon.WeaponType.OneHanded, oneHandedSprite },
            { Weapon.WeaponType.ReallyBigTwoHanded, reallyBigTwoHandedSprite },
            { Weapon.WeaponType.Polearm, polearmSprite },
            { Weapon.WeaponType.Dagger, daggerSprite },
            { Weapon.WeaponType.BigPen, bigPenSprite }
        };
    }

    private void Update()
    {
        // Dynamically assign player weapon managers if not already assigned
        if (player1WeaponManager == null && player1 != null)
        {
            player1WeaponManager = player1.GetComponent<WeaponManager>();
            if (player1WeaponManager != null)
            {
                SubscribeToPlayerEvents(player1WeaponManager, player1WeaponImage);
            }
        }

        if (player2WeaponManager == null && player2 != null)
        {
            player2WeaponManager = player2.GetComponent<WeaponManager>();
            if (player2WeaponManager != null)
            {
                SubscribeToPlayerEvents(player2WeaponManager, player2WeaponImage);
            }
        }
    }

    private void SubscribeToPlayerEvents(WeaponManager weaponManager, Image weaponImage)
    {
        weaponManager.OnWeaponEquipped += (weapon) => UpdateWeaponUI(weapon, weaponImage);
        weaponManager.OnWeaponBroken += (weapon) => ShowDefaultWeapon(weaponImage);
    }

    private void OnDisable()
    {
        // Unsubscribe from Player 1 events
        if (player1WeaponManager != null)
        {
            player1WeaponManager.OnWeaponEquipped -= (weapon) => UpdateWeaponUI(weapon, player1WeaponImage);
            player1WeaponManager.OnWeaponBroken -= (weapon) => ShowDefaultWeapon(player1WeaponImage);
        }

        // Unsubscribe from Player 2 events
        if (player2WeaponManager != null)
        {
            player2WeaponManager.OnWeaponEquipped -= (weapon) => UpdateWeaponUI(weapon, player2WeaponImage);
            player2WeaponManager.OnWeaponBroken -= (weapon) => ShowDefaultWeapon(player2WeaponImage);
        }
    }

    private void Start()
    {
        // Initialize both players' UI with default weapon state
        ShowDefaultWeapon(player1WeaponImage);
        ShowDefaultWeapon(player2WeaponImage);
    }

    private void UpdateWeaponUI(GameObject weapon, Image weaponImage)
    {
        if (weapon == null)
        {
            ShowDefaultWeapon(weaponImage);
            return;
        }

        Weapon weaponScript = weapon.GetComponent<Weapon>();
        if (weaponScript != null)
        {
            if (weaponTypeSprites.TryGetValue(weaponScript.currentWeaponType, out Sprite weaponSprite))
            {
                weaponImage.sprite = weaponSprite;
                weaponImage.enabled = true;
            }
            else
            {
                Debug.LogWarning("Sprite not found for weapon type: " + weaponScript.currentWeaponType);
                ShowDefaultWeapon(weaponImage);
            }
        }
        else
        {
            ShowDefaultWeapon(weaponImage);
        }
    }

    private void ShowDefaultWeapon(Image weaponImage)
    {
        weaponImage.sprite = defaultSprite;
        weaponImage.enabled = true;
    }
}