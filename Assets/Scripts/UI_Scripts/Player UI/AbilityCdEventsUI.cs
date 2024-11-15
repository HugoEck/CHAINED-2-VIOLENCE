using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to keep track of the cooldowns for the ability UI.
/// </summary>
public static class AbilityCdEventsUI
{

    //public static event Action<int, float> OnAbilityUsed;

    //public static void TriggerAbilityUsed(int playerId, float cooldown)
    //{
    //    OnAbilityUsed?.Invoke(playerId, cooldown);
    //}
    public static event Action<int, PlayerCombat.PlayerClass, float> OnAbilityUsed;

    public static void AbilityUsed(int playerId, PlayerCombat.PlayerClass playerClass, float cooldown)
    {
        OnAbilityUsed?.Invoke(playerId, playerClass, cooldown);
    }
}
