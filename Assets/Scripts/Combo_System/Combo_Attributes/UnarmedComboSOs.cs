using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Combo attack/ Unarmed Combos")]
public class UnarmedComboSOs : ScriptableObject
{

    public ComboAttackSO[] unarmedDefaultCombos;
    public ComboAttackSO[] unarmedTankCombos;
    public ComboAttackSO[] unarmedWarriorCombos;
    public ComboAttackSO[] unarmedRangedCombos;
    public ComboAttackSO[] unarmedSupportCombos;

}
