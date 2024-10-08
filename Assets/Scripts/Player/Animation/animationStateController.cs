using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationStateController : MonoBehaviour
{
    private Animator _animator;
    private PlayerInput _playerInput;
    private InputAction _moveAction;

    private Camera _mainCamera;

    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _playerInput = GetComponent<PlayerInput>();
        _mainCamera = Camera.main;

        if (_playerInput == null)
        {
            Debug.LogError("PlayerInput component is missing on the Player GameObject!");
            return;
        }

        _moveAction = _playerInput.actions["PlayerMovementAction"];
    }

    void Update()
    {
        if (_moveAction == null)
        {
            Debug.LogError("Input actions are not properly initialized.");
            return;
        }

        // Read movement input (WASD)
        Vector2 moveInput = _moveAction.ReadValue<Vector2>();
        bool isMoving = moveInput.magnitude > 0;

        // Update the "isMoving" parameter in the animator
        _animator.SetBool("isMoving", isMoving);

        if (!isMoving)
        {
            // Reset movement parameters if not moving
            _animator.SetFloat("MovementX", 0);
            _animator.SetFloat("MovementY", 0);
            return;
        }

        // Rotate the player towards the mouse position
        Vector3 mousePosition = MouseWorldPosition();
        if (mousePosition == Vector3.zero) return;

        RotateTowardsMouse(mousePosition);

        // Now, map WASD input to the player's local movement directions
        float moveX = moveInput.x;  // A/D for left/right strafing
        float moveY = moveInput.y;  // W/S for forward/backward

        // Update the animator parameters for the Blend Tree
        _animator.SetFloat("MovementX", moveX);   // Strafe left/right
        _animator.SetFloat("MovementY", moveY);   // Forward/backward
    }

    private void RotateTowardsMouse(Vector3 mousePosition)
    {
        Vector3 directionToMouse = (mousePosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToMouse.x, 0, directionToMouse.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f); // Adjust rotation speed here
    }

    // function to get the mouse position in world space
    private Vector3 MouseWorldPosition()
    {
        Vector3 mouseScreenPosition = Mouse.current.position.ReadValue();
        Ray ray = _mainCamera.ScreenPointToRay(mouseScreenPosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            return hitInfo.point;
        }

        return Vector3.zero;
    }
}
