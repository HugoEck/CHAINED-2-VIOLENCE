using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipManager : MonoBehaviour
{
    #region Player

    [Header("Player")]

    [SerializeField] public AudioClip playerDeath;
    [SerializeField] public AudioClip playerRespawn;
    [SerializeField] public AudioClip[] takeDamagePlayer;

    #endregion

    #region Combo-System

    [Header("Player Combo-System")]
    [SerializeField] public AudioClip[] tankUnarmedSounds;
    [SerializeField] public AudioClip[] warriorUnarmedSounds;
    [SerializeField] public AudioClip[] rangedUnarmedSounds;
    [SerializeField] public AudioClip[] supportUnarmedSounds;
    [SerializeField] public AudioClip[] twoHandedWeaponSounds;
    [SerializeField] public AudioClip[] oneHandedWeaponSounds;
    [SerializeField] public AudioClip[] reallyBigTwoHandedWeaponSounds;
    [SerializeField] public AudioClip[] polearmWeaponSounds;
    [SerializeField] public AudioClip[] daggerWeaponSounds;
    [SerializeField] public AudioClip[] bigPenWeaponSounds;

    #endregion

    #region

    [Header("Player Abilities and Ultimates")]

    [SerializeField] public AudioClip abilityReady;
    [SerializeField] public AudioClip coneAbility;
    [SerializeField] public AudioClip rangeAbility;
    [SerializeField] public AudioClip rangeAbilityExplosion;
    [SerializeField] public AudioClip swingAbility;
    [SerializeField] public AudioClip shieldActivate;
    [SerializeField] public AudioClip shieldAbsorb;
    [SerializeField] public AudioClip shieldBreak;

    [SerializeField] public AudioClip electricChain;
    [SerializeField] public AudioClip fireChain;
    [SerializeField] public AudioClip ghostChain;
    [SerializeField] public AudioClip laserChain;

    #endregion 

    #region Enemy Sound Effects

    [Header("Enemies")]
    [SerializeField] public AudioClip basicAttack;
    [SerializeField] public AudioClip enemyDeath;
    [SerializeField] public AudioClip chargerRoar;
    [SerializeField] public AudioClip bannerMan;

    [Header("CyberGiant")]
    [SerializeField] public AudioClip[] attacks;
    [SerializeField] public AudioClip missile;
    [SerializeField] public AudioClip shield;
    [SerializeField] public AudioClip chargeUp;
    [SerializeField] public AudioClip[] move;

    #endregion

    #region Lobby

    [Header("Lobby")]
    [SerializeField] public AudioClip evilScientist;
    [SerializeField] public AudioClip button;
    [SerializeField] public AudioClip doorOpen;

    #endregion

    #region Environment

    [Header("Environment")]

    [SerializeField] public AudioClip spawnObj;
    [SerializeField] public AudioClip trapDamage;
    [SerializeField] public AudioClip boulder;

    #endregion

    #region UI

    [Header("UI")]

    [SerializeField] public AudioClip upgradeButton;
    [SerializeField] public AudioClip mainMenuStartbutton;
    [SerializeField] public AudioClip mainMenuButton;

    [SerializeField] public AudioClip ultimateActivation;

    #endregion

    #region

    [Header("Wave")]
    [SerializeField] public AudioClip newWave;
    [SerializeField] public AudioClip itemButton;

    #endregion

    #region

    [Header("WeaponManager")]
    [SerializeField] public AudioClip pickupWeapon;
    [SerializeField] public AudioClip dropWeapon;

    #endregion

    private void Awake()
    {
        ValidateAudioClips();
        GameObject.DontDestroyOnLoad(this);
    }

    public void ValidateAudioClips()
    {
        System.Action<AudioClip, string> checkClip = (clip, name) =>
        {
            if (clip == null)
            {
                Debug.LogWarning($"AudioClip '{name}' is missing in AudioClipManager.");
            }
        };

        // Player
        checkClip(playerDeath, nameof(playerDeath));
        checkClip(playerRespawn, nameof(playerRespawn));

        // Combo-System
        ValidateAudioClipArray(tankUnarmedSounds, nameof(tankUnarmedSounds));
        ValidateAudioClipArray(warriorUnarmedSounds, nameof(warriorUnarmedSounds));
        ValidateAudioClipArray(rangedUnarmedSounds, nameof(rangedUnarmedSounds));
        ValidateAudioClipArray(supportUnarmedSounds, nameof(supportUnarmedSounds));
        ValidateAudioClipArray(twoHandedWeaponSounds, nameof(twoHandedWeaponSounds));
        ValidateAudioClipArray(oneHandedWeaponSounds, nameof(oneHandedWeaponSounds));
        ValidateAudioClipArray(reallyBigTwoHandedWeaponSounds, nameof(reallyBigTwoHandedWeaponSounds));
        ValidateAudioClipArray(polearmWeaponSounds, nameof(polearmWeaponSounds));
        ValidateAudioClipArray(daggerWeaponSounds, nameof(daggerWeaponSounds));
        ValidateAudioClipArray(bigPenWeaponSounds, nameof(bigPenWeaponSounds));

        // Abilities and Ultimates
        checkClip(coneAbility, nameof(coneAbility));
        checkClip(rangeAbility, nameof(rangeAbility));
        checkClip(rangeAbilityExplosion, nameof(rangeAbilityExplosion));
        checkClip(swingAbility, nameof(swingAbility));
        checkClip(shieldActivate, nameof(shieldActivate));
        checkClip(shieldAbsorb, nameof(shieldAbsorb));
        checkClip(shieldBreak, nameof(shieldBreak));

        // Chains
        checkClip(electricChain, nameof(electricChain));
        checkClip(fireChain, nameof(fireChain));
        checkClip(ghostChain, nameof(ghostChain));
        checkClip(laserChain, nameof(laserChain));

        // Enemies
        checkClip(basicAttack, nameof(basicAttack));
        checkClip(enemyDeath, nameof(enemyDeath));
        checkClip(chargerRoar, nameof(chargerRoar));
        checkClip(bannerMan, nameof(bannerMan));

        // CyberGiant
        ValidateAudioClipArray(attacks, nameof(attacks));
        checkClip(missile, nameof(missile));
        checkClip(shield, nameof(shield));
        checkClip(chargeUp, nameof(chargeUp));
        ValidateAudioClipArray(move, nameof(move));

        // Lobby
        checkClip(evilScientist, nameof(evilScientist));
        checkClip(button, nameof(button));
        checkClip(doorOpen, nameof(doorOpen));

        // Environment
        checkClip(spawnObj, nameof(spawnObj));
        checkClip(trapDamage, nameof(trapDamage));
        checkClip(boulder, nameof(boulder));

        // UI
        checkClip(upgradeButton, nameof(upgradeButton));
        checkClip(mainMenuStartbutton, nameof(mainMenuStartbutton));
        checkClip(mainMenuButton, nameof(mainMenuButton));
        checkClip(ultimateActivation, nameof(ultimateActivation));

        // Wave
        checkClip(newWave, nameof(newWave));
        checkClip(itemButton, nameof(itemButton));

        // WeaponManager
        checkClip(pickupWeapon, nameof(pickupWeapon));
        checkClip(dropWeapon, nameof(dropWeapon));
    }

    private void ValidateAudioClipArray(AudioClip[] clips, string arrayName)
    {
        if (clips == null || clips.Length == 0)
        {
            Debug.LogWarning($"AudioClip array '{arrayName}' is null or empty in AudioClipManager.");
            return;
        }

        for (int i = 0; i < clips.Length; i++)
        {
            if (clips[i] == null)
            {
                Debug.LogWarning($"AudioClip '{arrayName}[{i}]' is missing in AudioClipManager.");
            }
        }
    }


}