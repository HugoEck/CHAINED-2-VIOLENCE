using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCdUI : MonoBehaviour
{
    [SerializeField] private Image player1AbilityImage;
    [SerializeField] private Image player2AbilityImage;

    [Header("Class Ability Icons")]
    [SerializeField] private Sprite tankAbilityIcon;
    [SerializeField] private Sprite warriorAbilityIcon;
    [SerializeField] private Sprite supportAbilityIcon;
    [SerializeField] private Sprite rangedAbilityIcon;

    private float player1CooldownRemaining;
    private float player2CooldownRemaining;

    private float player1CooldownTime;
    private float player2CooldownTime;

    [Header("Cooldown Stuff")]
    [SerializeField] private Image imageCooldownP1;
    [SerializeField] private Image imageCooldownP2;
    [SerializeField] private TMP_Text textCooldownP1;
    [SerializeField] private TMP_Text textCooldownP2;

    private void OnEnable()
    {
        PlayerCombat.OnClassSwitched += HandleClassSwitched;
        AbilityCdEventsUI.OnAbilityUsed += HandleAbilityUsed;
    }

    private void OnDisable()
    {
        PlayerCombat.OnClassSwitched -= HandleClassSwitched;
        AbilityCdEventsUI.OnAbilityUsed -= HandleAbilityUsed;
    }

    private void Update()
    {
        UpdateCooldown(ref player1CooldownRemaining, player1CooldownTime, imageCooldownP1, textCooldownP1);
        UpdateCooldown(ref player2CooldownRemaining, player2CooldownTime, imageCooldownP2, textCooldownP2);
    }

    private void HandleAbilityUsed(int playerId, PlayerCombat.PlayerClass playerClass, float cooldown)
    {
        if (playerId == 1)
        {
            if (player1CooldownRemaining <= 0)
            {
                player1CooldownRemaining = cooldown;
                player1CooldownTime = cooldown;
                imageCooldownP1.fillAmount = 1;
                textCooldownP1.text = cooldown.ToString("F1");
            }
        }
        else if (playerId == 2)
        {
            if (player2CooldownRemaining <= 0)
            {
                player2CooldownRemaining = cooldown;
                player2CooldownTime = cooldown;
                imageCooldownP2.fillAmount = 1;
                textCooldownP2.text = cooldown.ToString("F1");
            }
        }
    }

    private void UpdateCooldown(ref float cooldownRemaining, float cooldownTime, Image cooldownImage, TMP_Text cooldownText)
    {
        if (cooldownRemaining > 0)
        {
            cooldownRemaining -= Time.deltaTime;
            float fillAmount = cooldownRemaining / cooldownTime;

            cooldownImage.fillAmount = fillAmount;
            cooldownText.text = Mathf.Ceil(cooldownRemaining).ToString();

            if (cooldownRemaining <= 0)
            {
                cooldownRemaining = 0;
                cooldownImage.fillAmount = 0;
                cooldownText.text = "";
            }
        }
    }

    private void HandleClassSwitched(int playerId, PlayerCombat.PlayerClass playerClass)
    {
        if (playerId == 1)
        {
            SetAbilityIcon(player1AbilityImage, playerClass);
        }
        else if (playerId == 2)
        {
            SetAbilityIcon(player2AbilityImage, playerClass);
        }
    }

    private void SetAbilityIcon(Image abilityImage, PlayerCombat.PlayerClass playerClass)
    {
        switch (playerClass)
        {
            case PlayerCombat.PlayerClass.Tank:
                abilityImage.sprite = tankAbilityIcon;
                break;
            case PlayerCombat.PlayerClass.Warrior:
                abilityImage.sprite = warriorAbilityIcon;
                break;
            case PlayerCombat.PlayerClass.Support:
                abilityImage.sprite = supportAbilityIcon;
                break;
            case PlayerCombat.PlayerClass.Ranged:
                abilityImage.sprite = rangedAbilityIcon;
                break;
            default:
                break;
        }
    }
}
