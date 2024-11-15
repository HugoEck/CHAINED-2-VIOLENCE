using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCdUI : MonoBehaviour
{
    [SerializeField] private Image player1AbilityImage;
    [SerializeField] private Image player2AbilityImage;

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
            player1CooldownRemaining = cooldown;
            player1CooldownTime = cooldown;
        }
        else if (playerId == 2)
        {
            player2CooldownRemaining = cooldown;
            player2CooldownTime = cooldown;
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
                abilityImage.fillAmount = 0;
            }
        }
    }
}
