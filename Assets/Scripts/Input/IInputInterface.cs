using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputInterface
{
    public Vector2 GetMovementInput_P1();

    public Vector2 GetRotationInput_P1();

    public bool GetBasicAttackInput_P1();

    public Vector2 GetMovementInput_P2();

    public Vector2 GetRotationInput_P2();

    public bool GetBasicAttackInput_P2();

    public bool GetAbilityAttackInput_P1();

    public bool GetAbilityAttackInput_P2();

    public bool GetUltimateAttackInput_P1();

    public bool GetUltimateAttackInput_P2();

}
