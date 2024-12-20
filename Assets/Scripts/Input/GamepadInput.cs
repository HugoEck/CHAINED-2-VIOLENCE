using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml;
using UnityEngine;

public class GamepadInput : MonoBehaviour, IInputInterface
{
    private string[] _numberOfActiveJoysticks;


    private string[] _joystickNames;
    private int _player1Joystick = -1;
    private int _player2Joystick = -1;

    private int _joystickIdPlayer1;
    private int _joystickIdPlayer2;

    public static KeyCode _player1BasicAttack;
    public static KeyCode _player1Ability;
    public static KeyCode _player1Ultimate;
    public static KeyCode _player1Interact;
    public static KeyCode _player1DropWeapon;

    public static KeyCode _player1AssignPlayer2;

    public static KeyCode _player2BasicAttack;
    public static KeyCode _player2Ability;
    public static KeyCode _player2Ultimate;
    public static KeyCode _player2Interact;
    public static KeyCode _player2DropWeapon;

    public static KeyCode _player2AssignPlayer2;

    //private Player _player1;
    //private Player _player2;

    private void Start()
    {
        NumberOfActiveControlls();

        #region PLAYER 1 KEYBINDS

        _player1BasicAttack = GetJoystickKeyCode(_joystickIdPlayer1, 5);
        _player1Ability = GetJoystickKeyCode(_joystickIdPlayer1, 0);
        _player1Ultimate = GetJoystickKeyCode(_joystickIdPlayer1, 1);
        _player1Interact = GetJoystickKeyCode(_joystickIdPlayer1, 2);
        _player1DropWeapon = GetJoystickKeyCode(_joystickIdPlayer1, 4);
        _player1AssignPlayer2 = GetJoystickKeyCode(_joystickIdPlayer1, 7);

        #endregion

        #region PLAYER 2 KEYBINDS

        _player2BasicAttack = GetJoystickKeyCode(_joystickIdPlayer2, 5);
        _player2Ability = GetJoystickKeyCode(_joystickIdPlayer2, 0);
        _player2Ultimate = GetJoystickKeyCode(_joystickIdPlayer2, 1);
        _player2Interact = GetJoystickKeyCode(_joystickIdPlayer2, 2);
        _player2DropWeapon = GetJoystickKeyCode(_joystickIdPlayer2, 4);
        _player2AssignPlayer2 = GetJoystickKeyCode(_joystickIdPlayer2, 7);

        #endregion     
    }
    private void Update()
    {       
        NumberOfActiveControlls();
    }
    private KeyCode GetJoystickKeyCode(int joystickNumber, int buttonNumber)
    {
        // Base KeyCode for Joystick1Button0
        int baseKeyCode = (int)KeyCode.Joystick1Button0;

        // Calculate the KeyCode dynamically
        return (KeyCode)(baseKeyCode + (joystickNumber - 1) * 20 + buttonNumber);
    }

    private void NumberOfActiveControlls()
    {
        _joystickNames = Input.GetJoystickNames();

        // Track the number of active joysticks
        int connectedJoysticks = 0;

        List<string> joysticks = new List<string>();
        
        // Loop through each joystick and check if it is connected
        for (int i = 0; i < _joystickNames.Length; i++)
        {
            if (!string.IsNullOrEmpty(_joystickNames[i]))
            {
                connectedJoysticks++;

                if(connectedJoysticks == 1)
                {
                    _joystickIdPlayer1 = i + 1;
                }
                else if(connectedJoysticks == 2)
                {
                    _joystickIdPlayer2 = i + 1;
                }
                // Optionally log the joystick name and its index (which corresponds to its joystick number)
                Debug.Log($"Joystick {i + 1} is connected: {_joystickNames[i]}");
                joysticks.Add("Joystick" + (i + 1));
            }
        }

        if (joysticks.Count == 0) return;

        if (!Chained2ViolenceGameManager.Instance.BIsPlayer2Assigned)
        {
            
            if (joysticks[0] == "Joystick2")
            {
                _player1Joystick = 1;

            }
            else
            {
                _player1Joystick = 2;
            }
           
            joysticks.Clear();
            return;
        }
        
        if(connectedJoysticks == 2)
        {            
            _player1Joystick = 1;
            _player2Joystick = 2;
        }
        else if(connectedJoysticks == 1)
        {
            _player1Joystick = -1;

            if (joysticks[0] == "Joystick2")
            {
                _player2Joystick = 1;
            }
            else
            {
                _player2Joystick = 2;
            }
        }

        joysticks.Clear();
    }

    public Vector2 GetMovementInput_P1()
    {
     
        if (_player1Joystick != -1)  // Check if Player 1 has a joystick assigned
        {
            // Use the dynamically assigned joystick number
            return new Vector2(
                Input.GetAxisRaw("Gamepad_Left_Horizontal_P" + (_player1Joystick)),  // Use the assigned joystick's horizontal axis
                Input.GetAxisRaw("Gamepad_Left_Vertical_P" + (_player1Joystick))     // Use the assigned joystick's vertical axis
            );
        }

        //// Ensure Player 1's input works regardless of activeInputs count
        ////if (activeInputs >= 1)
        ////{
        //// Always read Player 1's inputs from the assigned axes
        //return new Vector2(
        //        Input.GetAxisRaw("Gamepad_Left_Horizontal_P1"),
        //        Input.GetAxisRaw("Gamepad_Left_Vertical_P1")
        //    );
        ////}

        // If no controllers are connected, return zero
        return Vector2.zero;
    }
    public bool GetBasicAttackInput_P1()
    {
        bool isPressed = false;

        // Ensure Player 1's input works regardless of activeInputs count
        if (_player1Joystick != -1)
        {
            isPressed = Input.GetKey(_player1BasicAttack);
            //isPressed = Input.GetButtonDown("Gamepad_Basic_Attack_P" + _player1Joystick);
        }

        return isPressed;

    }
    public Vector2 GetRotationInput_P1()
    {
        //int activeInputs = NumberOfActiveControlls();

        // Ensure Player 1's input works regardless of activeInputs count
        if (_player1Joystick != -1)
        {
            return new Vector2(Input.GetAxisRaw("Gamepad_Right_Horizontal_P" + _player1Joystick), Input.GetAxis("Gamepad_Right_Vertical_P" + _player1Joystick));
        }

        return Vector2.zero;
        //if (Chained2ViolenceGameManager.Instance.BIsPlayer2Assigned) return Vector2.zero;
     
    }

    public Vector2 GetMovementInput_P2()
    {
       
        if (Chained2ViolenceGameManager.Instance.BIsPlayer2Assigned)
        {

            if (_player2Joystick != -1)  // Check if Player 1 has a joystick assigned
            {
                // Use the dynamically assigned joystick number
                return new Vector2(
                    Input.GetAxisRaw("Gamepad_Left_Horizontal_P" + (_player2Joystick)),  // Use the assigned joystick's horizontal axis
                    Input.GetAxisRaw("Gamepad_Left_Vertical_P" + (_player2Joystick))     // Use the assigned joystick's vertical axis
                );
            }

        }

        return Vector2.zero;
    }
    public Vector2 GetRotationInput_P2()
    {
        if (Chained2ViolenceGameManager.Instance.BIsPlayer2Assigned)
        {

            if (_player2Joystick != -1)
            {
                return new Vector2(Input.GetAxisRaw("Gamepad_Right_Horizontal_P" + _player2Joystick), Input.GetAxisRaw("Gamepad_Right_Vertical_P" + _player2Joystick));
            }
            
        }

        return Vector2.zero;
    }
    public bool GetBasicAttackInput_P2()
    {
        bool isPressed = false;

        if (Chained2ViolenceGameManager.Instance.BIsPlayer2Assigned)
        {

            if (_player2Joystick != -1)
            {
                if(_player1Joystick == -1)
                {
                    isPressed = Input.GetKey(_player1BasicAttack);
                }
                else
                {
                    isPressed = Input.GetKey(_player2BasicAttack);
                }
                
            }
           
        }
        Debug.Log("IS PRESSED: " + isPressed);
        return isPressed;
    }

    public bool GetAbilityAttackInput_P1()
    {
        bool isPressed = false;

        // Ensure Player 1's input works regardless of activeInputs count
        if (_player1Joystick != -1)
        {
            isPressed = Input.GetKey(_player1Ability);
        }

        return isPressed;
    }

    public bool GetAbilityAttackInput_P2()
    {
        bool isPressed = false;

        if (Chained2ViolenceGameManager.Instance.BIsPlayer2Assigned)
        {
            if (_player2Joystick != -1)
            {
                if ((_player1Joystick == -1))
                {
                    isPressed = Input.GetKey(_player1Ability);
                }
                else
                {
                    isPressed = Input.GetKey(_player2Ability);
                }
                
            }
            
        }

        return isPressed;
    }

    public bool AssignPlayer2()
    {
        if (Chained2ViolenceGameManager.Instance.BIsPlayer2Assigned) return false;
       
        bool isPressed = false;
        if(_player1Joystick != -1)
        {
            isPressed = Input.GetKey(_player1AssignPlayer2);
        }
        

        return isPressed;
    }

    public bool GetUltimateAttackInput_P1()
    {
        bool isPressed = false;

        //// Ensure Player 1's input works regardless of activeInputs count
        if (_player1Joystick != -1)
        {
            isPressed = Input.GetKey(_player1Ultimate);
        }

        return isPressed;
    }

    public bool GetUltimateAttackInput_P2()
    {
        bool isPressed = false;

        if (Chained2ViolenceGameManager.Instance.BIsPlayer2Assigned)
        {
            if (_player2Joystick != -1)
            {
                if(_player1Joystick == -1) 
                {
                    isPressed = Input.GetKey(_player1Ultimate);
                }
                else
                {
                    isPressed = Input.GetKey(_player2Ultimate);
                }
                               
            }
            
        }

        return isPressed;
    }

    public bool GetInteractInput_P1()
    {
        bool isPressed = false;

        // Ensure Player 1's input works regardless of activeInputs count
        if (_player1Joystick != -1)
        {
            isPressed = Input.GetKey(_player1Interact);
        }

        return isPressed;
    }

    public bool GetInteractInput_P2()
    {
        bool isPressed = false;

        if (Chained2ViolenceGameManager.Instance.BIsPlayer2Assigned)
        {

            if (_player2Joystick != -1)
            {
                if(_player1Joystick == -1)
                {
                    isPressed = Input.GetKey(_player1Interact);
                }
                else
                {
                    isPressed = Input.GetKey(_player2Interact);
                }
                
            }
            
        }

        return isPressed;
    }

    public bool GetDropWeaponInput_P1()
    {
        bool isPressed = false;
        //int activeInputs = NumberOfActiveControlls();

        // Ensure Player 1's input works regardless of activeInputs count
        if (_player1Joystick != -1)
        {
            isPressed |= Input.GetKey(_player1DropWeapon);
        }

        return isPressed;    
    }

    public bool GetDropWeaponInput_P2()
    {
        bool isPressed = false;
        if (_player2Joystick != -1)
        {
            if(_player1Joystick == -1)
            {
                isPressed = !Input.GetKey(_player1DropWeapon);
            }
            else
            {
                isPressed = !Input.GetKey(_player2DropWeapon);
            }
           
        }
        return isPressed;
    }
}
