using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is pretty much only used for testing since it will only be player 1 that can use a keyboard and mouse 
/// </summary>
public class KeyboardAndMouse : MonoBehaviour, IInputInterface
{
    public Vector2 GetMovementInput_P1()
    {
        return new Vector2(Input.GetAxisRaw("Keyboard_Horizontal_P1"), Input.GetAxisRaw("Keyboard_Vertical_P1"));
    }

    public Vector2 GetRotationInput_P1()
    {
        return Vector2.zero; // The player doesn't have to retrieve the mouse position
    }
    
    public bool GetBasicAttackInput_P1()
    {

        if (Input.GetMouseButtonDown(0))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public Vector2 GetMovementInput_P2()
    {
        return Vector2.zero;
    }

    public Vector2 GetRotationInput_P2()
    {
        return Vector2.zero; // The player doesn't have to retrieve the mouse position
    }

    public bool GetBasicAttackInput_P2()
    {
        return false;
    }

    public bool GetAbilityAttackInput_P1()
    {
        if (Input.GetMouseButtonDown(1))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool GetAbilityAttackInput_P2()
    {
        return false;
    }

    public bool GetUltimateAttackInput_P1()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool GetUltimateAttackInput_P2()
    {
        return false;
    }
}
