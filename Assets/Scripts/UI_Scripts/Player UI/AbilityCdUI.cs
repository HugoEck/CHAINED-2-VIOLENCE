using System.Collections;
using System.Collections.Generic;
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

    private void OnEnable()
    {
        AbilityCdEventsUI.OnAbilityUsed += HandleAbilityUsed;
    }

    private void OnDisable()
    {
        AbilityCdEventsUI.OnAbilityUsed -= HandleAbilityUsed;
    }

    private void Update()
    {
        UpdateCooldown(player1AbilityImage, ref player1CooldownRemaining, player1CooldownTime);
        UpdateCooldown(player2AbilityImage, ref player2CooldownRemaining, player2CooldownTime);
    }

    private void HandleAbilityUsed(int playerId, PlayerCombat.PlayerClass playerClass, float cooldown)
    {
        if (playerId == 1)
        {
            SetAbilityIcon(player1AbilityImage, playerClass);

            if (player1CooldownRemaining <= 0)
            {
                player1CooldownRemaining = cooldown;
                player1CooldownTime = cooldown;
                player1AbilityImage.fillAmount = 1;
                player1AbilityImage.gameObject.SetActive(true);
            }
        }
        else if (playerId == 2)
        {
            SetAbilityIcon(player2AbilityImage, playerClass);

            if (player2CooldownRemaining <= 0)
            {
                player2CooldownRemaining = cooldown;
                player2CooldownTime = cooldown;
                player2AbilityImage.fillAmount = 1;
                player2AbilityImage.gameObject.SetActive(true);
            }
        }
    }

    private void UpdateCooldown(Image abilityImage, ref float cooldownRemaining, float cooldownTime)
    {
        if (cooldownRemaining > 0)
        {
            cooldownRemaining -= Time.deltaTime;
            abilityImage.fillAmount = cooldownRemaining / cooldownTime;

            if (cooldownRemaining <= 0)
            {
                cooldownRemaining = 0;
                abilityImage.fillAmount = 1;
                abilityImage.gameObject.SetActive(true);
            }
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
