using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public enum InputType
    {
        Gamepad,
        KeyboardAndMouse
    };

    public static InputManager Instance { get; private set; }
    public InputType currentInputType { get; private set; } = InputType.Gamepad;

    #region Available Inputs

    private GamepadInput _gamepadInput;
    private KeyboardAndMouse _keyboardAndMouse;

    #endregion

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            #region Add Inputs To InputManager GameObject

            _gamepadInput = gameObject.AddComponent<GamepadInput>();
            _keyboardAndMouse = gameObject.AddComponent<KeyboardAndMouse>();

            #endregion
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #region Check inputs
    public Vector2 GetMovementInput_P1()
    {
        CheckActiveInput();

        if(currentInputType == InputType.Gamepad)
        {
           return _gamepadInput.GetMovementInput_P1();
        }
        else if(currentInputType == InputType.KeyboardAndMouse)
        {
            return _keyboardAndMouse.GetMovementInput_P1();
        }
       
        return Vector2.zero;
    }
    public Vector2 GetRotationInput_P1()
    {
        CheckActiveInput();

        if (currentInputType == InputType.Gamepad)
        {
            return _gamepadInput.GetRotationInput_P1();
        }
        else if(currentInputType== InputType.KeyboardAndMouse)
        {
            return _keyboardAndMouse.GetRotationInput_P1();
        }

        return Vector2.zero;
    }
    public bool GetBasicAttackInput_P1()
    {
        CheckActiveInput();

        if( currentInputType == InputType.Gamepad)
        {
            return _gamepadInput.GetBasicAttackInput_P1();
        }
        else if(currentInputType == InputType.KeyboardAndMouse)
        {
            return _keyboardAndMouse.GetBasicAttackInput_P1();
        }

        return false;
    }

    public Vector2 GetMovementInput_P2()
    {     
        return _gamepadInput.GetMovementInput_P2();
    }
    public Vector2 GetRotationInput_P2()
    {
        
        return _gamepadInput.GetRotationInput_P2();

    }
    public bool GetBasicAttackInput_P2()
    {
        
        return _gamepadInput.GetBasicAttackInput_P2();        
    }

    public bool GetAbilityAttackInput_P1()
    {

        CheckActiveInput();

        if (currentInputType == InputType.Gamepad)
        {
            return _gamepadInput.GetAbilityAttackInput_P1();
        }
        else if (currentInputType == InputType.KeyboardAndMouse)
        {
            return _keyboardAndMouse.GetAbilityAttackInput_P1();
        }

        return false;
    }

    public bool GetAbilityAttackInput_P2()
    {

        return _gamepadInput.GetAbilityAttackInput_P2();
    }


    #endregion
    #region Check Active Input

    public void CheckActiveInput()
    {      
        
        if (IsGamepadActive())
        {
            if (currentInputType != InputType.Gamepad)
            {
                currentInputType = InputType.Gamepad;

                if(!Chained2ViolenceGameManager.Instance.BIsPlayer2Assigned)
                {
                    Debug.Log("Switched to Gamepad input");
                }            
            }
        }
        else if (IsKeyboardAndMouseActive())
        {
            if (currentInputType != InputType.KeyboardAndMouse)
            {
                currentInputType = InputType.KeyboardAndMouse;

                if(!Chained2ViolenceGameManager.Instance.BIsPlayer2Assigned)
                {
                    Debug.Log("Switched to Keyboard and Mouse input");
                }           
            }
        }
    }

    private bool IsGamepadActive()
    {

        // Check for gamepad movement (e.g., joystick movement) or button presses
        return Mathf.Abs(Input.GetAxisRaw("Gamepad_Left_Horizontal_P1")) > 0.1f ||
               Mathf.Abs(Input.GetAxisRaw("Gamepad_Left_Vertical_P1")) > 0.1f ||
               Mathf.Abs(Input.GetAxisRaw("Gamepad_Right_Horizontal_P1")) > 0.1f ||
                Mathf.Abs(Input.GetAxisRaw("Gamepad_Right_Vertical_P1")) > 0.1f ||
                Input.GetButtonDown("Gamepad_Basic_Attack_P1");
                

    }

    private bool IsKeyboardAndMouseActive()
    {       

        // Check for keyboard or mouse input (e.g., movement keys or mouse clicks)
        return Input.GetKey(KeyCode.W) ||
               Input.GetKey(KeyCode.A) ||
               Input.GetKey(KeyCode.S) ||
               Input.GetKey(KeyCode.D) ||
               Input.GetMouseButtonDown(0) || 
               Input.GetMouseButtonDown(1);  
    }

    #endregion

    #region Player 2 assignment

    public bool AssignPlayer2()
    {
        return _gamepadInput.AssignPlayer2();
    }

    #endregion
}
