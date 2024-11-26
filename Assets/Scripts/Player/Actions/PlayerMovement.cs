using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

/// <summary>
/// Script for handling player movement & rotation of camera
/// </summary>
public class PlayerMovement : MonoBehaviour ///// NOT PRODUCTION READY
{
    [Header("Player Movement")]
    [SerializeField] private float _walkingSpeed = 200.0f;
    [SerializeField] private float _playerRotateSpeedMouse = 5.0f;
    [SerializeField] private float _playerRotateSpeedJoystick = 10.0f;

    private Camera _mainCameraReference;

    private Rigidbody _playerRigidBody;

    private InputAction _moveAction;
    private PlayerInput _playerInput;

    private Vector2 _playerMoveDirection = Vector2.zero; // Direction on 2d plane (movement input)
    private Vector3 _isometricPlayerMoveDirection = Vector3.zero; // Adjust the player direction based on camera angle
    public float originalWalkingSpeed { get; private set; }

    private Vector3 _externalForce = Vector3.zero;

    private bool _bIsGrounded = false;
    private float _groundCheckDistance = 2;
    private LayerMask _groundLayer;
    
    private void Start()
    {
        _mainCameraReference = Camera.main;
        _playerRigidBody = GetComponent<Rigidbody>();

        if (gameObject.CompareTag("Player1"))
        {
            _walkingSpeed = StatsTransfer.Player1WalkingSpeed > 0 ? StatsTransfer.Player1WalkingSpeed : _walkingSpeed;
        }
        else if (gameObject.CompareTag("Player2"))
        {
            _walkingSpeed = StatsTransfer.Player2WalkingSpeed > 0 ? StatsTransfer.Player2WalkingSpeed : _walkingSpeed;
        }

        originalWalkingSpeed = _walkingSpeed;

        _groundLayer = LayerMask.GetMask("Ground");
    }
    private void Update()
    {
        RaycastGroundCheck();
        if(!_mainCameraReference)
        {
            _mainCameraReference = Camera.main;
        }     
    }

    #region Player Movement & Camera rotation

    #region Movement
   
    public Vector2 GetMovementInput()
    {
        // Return the movement input for other scripts (such as the animation controller)
        return _playerMoveDirection;
    }
    public void MovePlayer(Vector2 movementInput)
    {
        if (!_bIsGrounded) return;

        // Calculate the desired movement direction relative to the camera's perspective (isometric movement)
        _isometricPlayerMoveDirection = ProjectToXZPlane(_mainCameraReference.transform.right) * movementInput.x +
                                         ProjectToXZPlane(_mainCameraReference.transform.forward) * movementInput.y;

        // Only move if there's input
        if (_isometricPlayerMoveDirection != Vector3.zero)
        {
            // Avoid faster diagonal speed
            _isometricPlayerMoveDirection.Normalize();

            float playerSpeedDt = _walkingSpeed * Time.deltaTime;

            Vector3 newVelocity = new Vector3(
                _isometricPlayerMoveDirection.x * playerSpeedDt,
                _playerRigidBody.velocity.y, // Keep the current vertical velocity
                _isometricPlayerMoveDirection.z * playerSpeedDt
            );

            // Lerp between the last frame's velocity and the new calculated total movement velocity
            float lerpFactor = 0.1f; // Adjust this value for how quickly you want to blend
            _playerRigidBody.velocity = Vector3.Lerp(_playerRigidBody.velocity, newVelocity, lerpFactor);
        }
    }
    private void RaycastGroundCheck()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.down * 0.1f;  // Start ray just below player pivot to avoid false positives

        // Perform raycast with debug visualization
        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, _groundCheckDistance, _groundLayer))
        {
            _bIsGrounded = true;
            Debug.DrawRay(rayOrigin, Vector3.down * _groundCheckDistance, Color.green);
            //Debug.Log("Ground detected at distance: " + hit.distance);
        }
        else
        {
            _bIsGrounded = false;
            Debug.DrawRay(rayOrigin, Vector3.down * _groundCheckDistance, Color.red);
            Debug.Log("No ground detected within range.");
        }
    }



    /// <summary>
    /// Get the XZ plane of the camera for handling isometric camera angle
    /// </summary>
    /// <param name="cameraVector"></param>
    /// <returns></returns>
    private Vector3 ProjectToXZPlane(Vector3 cameraVector)
    {
        // Only get the XZ values
        cameraVector.y = 0;

        // Normalize the vector to ensure consistent movement speed
        return cameraVector.normalized;
    }


    // Used for upgrading the player movement speed thru the upgrade system.
    public void SetWalkingSpeed(float newSpeed)
    {
        _walkingSpeed = newSpeed;
    }

    public float GetWalkingSpeed()
    {
        return _walkingSpeed;
    }

    #endregion

    ////////// ROTATION MIGHT NEED WORK IN FUTURE (USE INPUT SYSTEM INSTEAD AND MAYBE CALCULATE WITHOUT RAYCAST)
    #region Rotation
    /// <summary>
    /// Have the player rotate toward cursor
    /// </summary>
    public void RotatePlayerToCursor()
    {
        // Get the current mouse position in screen space
        Vector3 mouseScreenPosition = Input.mousePosition;

        // Convert mouse position to a ray that originates from the camera
        Ray ray = _mainCameraReference.ScreenPointToRay(mouseScreenPosition);

        // Plane where the player is walking (XZ), dynamically positioned at the player's level
        Plane groundPlane = new Plane(Vector3.up, transform.position);

        // Calculate where the ray hits the ground plane
        if (groundPlane.Raycast(ray, out float enter))
        {
            // Get the point in world space where the ray intersects the ground plane
            Vector3 hitPoint = ray.GetPoint(enter);

            // Calculate the direction from the player to the hit point
            Vector3 rayOrigin = transform.position; // Player's position
            Vector3 directionToLook = hitPoint - rayOrigin; // Direction to mouse position
            directionToLook.y = 0; // Ensure the player only rotates on the XZ plane

            // Only rotate if there is a direction
            if (directionToLook != Vector3.zero)
            {
                float playerRotationSpeedDt = _playerRotateSpeedMouse * Time.deltaTime;

                // Calculate the target rotation based on the direction the player should face
                Quaternion targetRotation = Quaternion.LookRotation(directionToLook);

                // Smoothly interpolate the rotation based on _playerLookSpeed
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, playerRotationSpeedDt);
            }
        }
    }


    public void RotatePlayerWithJoystick(Vector2 joystickInput)
    {
        if (joystickInput != Vector2.zero)
        {
            // Get the camera's forward direction on the XZ plane
            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0;  // Flatten to XZ plane
            cameraForward.Normalize();

            // Calculate right direction relative to the camera's forward
            Vector3 cameraRight = Camera.main.transform.right;
            cameraRight.y = 0;
            cameraRight.Normalize();

            // Map joystick input to camera's forward and right directions
            Vector3 targetDirection = (cameraRight * joystickInput.x + cameraForward * joystickInput.y).normalized;

            // Calculate the target rotation based on the target direction
            float angle = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, angle, 0);

            // Smoothly rotate the player towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _playerRotateSpeedJoystick);
        }
    }


    #endregion

    #endregion
}
