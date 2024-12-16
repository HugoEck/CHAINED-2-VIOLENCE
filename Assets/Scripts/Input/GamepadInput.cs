using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadInput : MonoBehaviour, IInputInterface
{
    public Vector2 GetMovementInput_P1()
    {
        if (Chained2ViolenceGameManager.Instance.BIsPlayer2Assigned) return Vector2.zero;

        return new Vector2(Input.GetAxisRaw("Gamepad_Left_Horizontal_P1"), Input.GetAxis("Gamepad_Left_Vertical_P1"));

    }
    public Vector2 GetRotationInput_P1()
    {
        if (Chained2ViolenceGameManager.Instance.BIsPlayer2Assigned) return Vector2.zero;

        return new Vector2(Input.GetAxisRaw("Gamepad_Right_Horizontal_P1"), Input.GetAxis("Gamepad_Right_Vertical_P1"));
     
    }
    public bool GetBasicAttackInput_P1()
    {
        bool isPressed = false;

        if (Chained2ViolenceGameManager.Instance.BIsPlayer2Assigned) return false;

        isPressed = Input.GetButtonDown("Gamepad_Basic_Attack_P1");

        return isPressed;
    }

    public Vector2 GetMovementInput_P2()
    {
        if (Chained2ViolenceGameManager.Instance.BIsPlayer2Assigned)
        {
            return new Vector2(Input.GetAxisRaw("Gamepad_Left_Horizontal_P1"), Input.GetAxisRaw("Gamepad_Left_Vertical_P1"));
        }

        return Vector2.zero;
    }
    public Vector2 GetRotationInput_P2()
    {
        if (Chained2ViolenceGameManager.Instance.BIsPlayer2Assigned)
        {
            return new Vector2(Input.GetAxisRaw("Gamepad_Right_Horizontal_P1"), Input.GetAxisRaw("Gamepad_Right_Vertical_P1"));
        }

        return Vector2.zero;
    }
    public bool GetBasicAttackInput_P2()
    {
        bool isPressed = false;

        if (Chained2ViolenceGameManager.Instance.BIsPlayer2Assigned)
        {
            isPressed = Input.GetButtonDown("Gamepad_Basic_Attack_P1");
        }

        return isPressed;
    }

    public bool GetAbilityAttackInput_P1()
    {
        bool isPressed = false;

        if (Chained2ViolenceGameManager.Instance.BIsPlayer2Assigned) return false;

        isPressed = Input.GetButtonDown("Gamepad_Ability_Attack_P1");

        return isPressed;
    }

    public bool GetAbilityAttackInput_P2()
    {
        bool isPressed = false;

        if (Chained2ViolenceGameManager.Instance.BIsPlayer2Assigned)
        {
            isPressed = Input.GetButtonDown("Gamepad_Ability_Attack_P1");
        }

        return isPressed;
    }

    public bool AssignPlayer2()
    {
        bool isPressed = Input.GetButtonDown("Assign_P2");

        return isPressed;
    }

    public bool GetUltimateAttackInput_P1()
    {
        bool isPressed = false;

        if (Chained2ViolenceGameManager.Instance.BIsPlayer2Assigned) return false;

        isPressed = Input.GetButtonDown("Gamepad_Ultimate_Attack_P1");

        return isPressed;
    }

    public bool GetUltimateAttackInput_P2()
    {
        bool isPressed = false;

        if (Chained2ViolenceGameManager.Instance.BIsPlayer2Assigned)
        {
            isPressed = Input.GetButtonDown("Gamepad_Ultimate_Attack_P2");
        }

        return isPressed;
    }

    public bool GetInteractInput_P1()
    {
        bool isPressed = false;

        if (Chained2ViolenceGameManager.Instance.BIsPlayer2Assigned) return false;

        isPressed = Input.GetButtonDown("Gamepad_Interact_P1");

        return isPressed;
    }

    public bool GetInteractInput_P2()
    {
        bool isPressed = false;

        if (Chained2ViolenceGameManager.Instance.BIsPlayer2Assigned)
        {
            isPressed = Input.GetButtonDown("Gamepad_Interact_P2");
        }

        return isPressed;
    }

    public bool GetDropWeaponInput_P1()
    {
        if (Chained2ViolenceGameManager.Instance.BIsPlayer2Assigned) return false;

        return Input.GetButtonDown("Gamepad_Drop_Weapon_P1");
    }

    public bool GetDropWeaponInput_P2()
    {
        if (!Chained2ViolenceGameManager.Instance.BIsPlayer2Assigned) return false;

        return Input.GetButtonDown("Gamepad_Drop_Weapon_P2");
    }
}
