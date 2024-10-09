using UnityEngine;
using UnityEngine.InputSystem;

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

    private void Start()
    {

        _mainCameraReference = Camera.main;
        _playerRigidBody = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions.FindAction("PlayerMovementAction");
        originalWalkingSpeed = _walkingSpeed;
    }
    private void Update()
    {
        if(!_mainCameraReference)
        {
            _mainCameraReference = Camera.main;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            transform.position = transform.forward * 20f;
        }

        _playerMoveDirection = _moveAction.ReadValue<Vector2>();

        float speed = _playerMoveDirection.magnitude;

    }

    #region Player Movement & Camera rotation

    #region Movement
    /// <summary>
    /// Called from the rope script 
    /// </summary>
    public void SetPlayerWalkingSpeed(float newWalkSpeed)
    {
        _walkingSpeed = newWalkSpeed;
    }
    public void MovePlayer(Vector2 movementInput)
    {

        // Calculate the desired movement direction relative to the camera's perspective (isometric movement)
        _isometricPlayerMoveDirection = ProjectToXZPlane(_mainCameraReference.transform.right) * movementInput.x +
                                ProjectToXZPlane(_mainCameraReference.transform.forward) * movementInput.y;

        // Only move if there's input
        if (_isometricPlayerMoveDirection != Vector3.zero)
        {
            // Avoid faster diagonal speed
            _isometricPlayerMoveDirection.Normalize();

            float playerSpeedDt = _walkingSpeed * Time.deltaTime;

            Vector3 newVelocity = new Vector3(_isometricPlayerMoveDirection.x * playerSpeedDt, _playerRigidBody.velocity.y, _isometricPlayerMoveDirection.z * playerSpeedDt);

            _playerRigidBody.velocity = newVelocity;
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

        // Plane where the player is walking (XZ)
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        // Calculate where the ray hits the ground plane
        if (groundPlane.Raycast(ray, out float enter))
        {
            // Get the point in world space where the ray intersects the ground plane
            Vector3 hitPoint = ray.GetPoint(enter);

            // Calculate the direction from the player to the hit point
            Vector3 directionToLook = hitPoint - transform.position;
            directionToLook.y = 0; // Ensure the player only rotates on the Y-axis

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
            float angle = Mathf.Atan2(joystickInput.x, joystickInput.y) * Mathf.Rad2Deg + 45; // The 45 value is added for the misalignment in the joystick rotation (45 degrees)
           
            Quaternion targetRotation = Quaternion.Euler(0, angle, 0);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _playerRotateSpeedJoystick);
        }
    }

    #endregion

    #endregion
}
