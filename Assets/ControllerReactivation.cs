using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class ControllerReactivation : MonoBehaviour
{
    [SerializeField] Button lastSelectedButton;  // Reference to the default button
    [SerializeField] EventSystem eventSystem;    // Reference to the EventSystem
    [SerializeField] Button currentSelectedButton;
    bool hasSetButton = false;
    [SerializeField] GameObject canvas;
    private enum InputDevice
    {
        Mouse,
        Controller
    }

    private InputDevice currentInputDevice;

    // Input Modules
    private StandaloneInputModule mouseInputModule;
    private StandaloneInputModule controllerInputModule;

    void Start()
    {
        // Cache the input modules
        Cursor.lockState = CursorLockMode.None;
        mouseInputModule = eventSystem.GetComponent<StandaloneInputModule>();
        controllerInputModule = eventSystem.GetComponent<StandaloneInputModule>(); // Make sure you have this input module in the scene
    }
    [SerializeField] Button savedButton;
    void Update()
    {
        // Determine the current input device (Mouse or Controller)
        DetermineInputDevice();

        // Track selected button
        if (eventSystem.currentSelectedGameObject != null)
        {
            currentSelectedButton = eventSystem.currentSelectedGameObject.GetComponent<Button>();

            //if (currentSelectedButton != null && currentSelectedButton != lastSelectedButton)
            //{
            //    // Update lastSelectedButton to the newly selected button
            //    lastSelectedButton = currentSelectedButton;
            //}
        }

        if (currentInputDevice == InputDevice.Mouse)
        {
            //savedButton = currentSelectedButton;
            
                hasSetButton = false;
                eventSystem.SetSelectedGameObject(null);
            
            //currentSelectedButton = null;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (currentInputDevice == InputDevice.Controller)
        {
            //savedButton = lastSelectedButton;
            if (!hasSetButton)
            {
                eventSystem.SetSelectedGameObject(lastSelectedButton.gameObject);
                hasSetButton = true;
            }
            if(canvas.activeSelf)
            Cursor.lockState = CursorLockMode.Locked;
        }
        //if (currentInputDevice == InputDevice.Mouse && Input.GetMouseButtonDown(0))
        //{
        //    // Track selected button
        //    if (eventSystem.currentSelectedGameObject != null)
        //    {
        //        currentSelectedButton = eventSystem.currentSelectedGameObject.GetComponent<Button>();

        //        if (currentSelectedButton != null && currentSelectedButton != lastSelectedButton)
        //        {
        //            // Update lastSelectedButton to the newly selected button
        //            lastSelectedButton = currentSelectedButton;
        //        }
        //    }
        //    if (IsPointerOverUI())
        //    {
        //        // Reset focus to the first UI button (or any specific button)
        //        eventSystem.SetSelectedGameObject(null);  // Remove focus
        //        eventSystem.SetSelectedGameObject(lastSelectedButton.gameObject);  // Set focus again
        //    }
        //}
    }

    private void DetermineInputDevice()
    {
        // Check if mouse input is being used
        if (Input.GetMouseButton(0) || Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            currentInputDevice = InputDevice.Mouse;

        }
        // Check if controller input is being used (look for controller axes or buttons)
        else if (Input.GetAxis("Dpad Horizontal") != 0 || Input.GetAxis("Dpad Vertical") != 0)
        {

            currentInputDevice = InputDevice.Controller;
        }
    }

    private bool IsPointerOverUI()
    {
        PointerEventData pointerData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition
        };

        // List to store raycast results
        List<RaycastResult> raycastResults = new List<RaycastResult>();

        // Use EventSystem to perform a raycast all
        eventSystem.RaycastAll(pointerData, raycastResults);

        // Return true if we hit any UI elements
        return raycastResults.Count > 0;
    }
}






