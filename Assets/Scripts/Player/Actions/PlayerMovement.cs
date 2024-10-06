using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Script for handling player movement & rotation of camera
/// </summary>
public class PlayerMovement : NetworkBehaviour ///// KIND OF PRODUCTION READY
{
    [Header("Player Movement")]
    [SerializeField] private float _walkingSpeed = 200.0f;
    [SerializeField] private float _playerRotateSpeed = 5.0f;

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
    public void MovePlayer()
    {

        // Calculate the desired movement direction relative to the camera's perspective (isometric movement)
        _isometricPlayerMoveDirection = ProjectToXZPlane(_mainCameraReference.transform.right) * _playerMoveDirection.x +
                                ProjectToXZPlane(_mainCameraReference.transform.forward) * _playerMoveDirection.y;

        // Only move if there's input
        if (_isometricPlayerMoveDirection != Vector3.zero)
        {
            // Avoid faster diagonal speed
            _isometricPlayerMoveDirection.Normalize();

            float playerSpeedDt = _walkingSpeed * Time.deltaTime;

            Vector3 newVelocity = new Vector3(_isometricPlayerMoveDirection.x * playerSpeedDt, _playerRigidBody.velocity.y, _isometricPlayerMoveDirection.z * playerSpeedDt);

            _playerRigidBody.velocity = newVelocity;

            // CURRENTLY COMMENTED OUT BECAUSE THE CHAIN WAS CHANGED SO THE CLIENT DOESN'T HAVE TO ASK THE SERVER ANYMORE TO MOVE WITH NETWORK TRANSFORM, REMOVE LATER
            //MovePlayerServerRpc(_isometricPlayerMoveDirection * playerSpeedDt);
        }

        RotatePlayerToCursor();
    }

    /// <summary>
    /// Update player movement on server
    /// </summary>
    /// <param name="moveDirection"></param>
    [ServerRpc]
    public void MovePlayerServerRpc(Vector3 moveDirection)
    {
        Vector3 newVelocity = new Vector3(moveDirection.x, _playerRigidBody.velocity.y, moveDirection.z);

        _playerRigidBody.velocity = newVelocity;
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
                float playerRotationSpeedDt = _playerRotateSpeed * Time.deltaTime;
                // Call the Server RPC to rotate the player
                RotatePlayerServerRpc(directionToLook, playerRotationSpeedDt);
            }
        }
    }

    /// <summary>
    /// Perform the player rotations on the server
    /// </summary>
    /// <param name="directionToLook"></param>
    [ServerRpc]
    public void RotatePlayerServerRpc(Vector3 directionToLook, float playerRotateSpeed)
    {
        // Calculate the target rotation based on the direction the player should face
        Quaternion targetRotation = Quaternion.LookRotation(directionToLook);

        // Smoothly interpolate the rotation based on _playerLookSpeed
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, playerRotateSpeed);

        // Optionally, call a Client RPC to inform all clients of the new rotation
        RotatePlayerClientRpc(transform.rotation);
    }

    /// <summary>
    /// Update the rotation for all clients
    /// </summary>
    /// <param name="targetRotation"></param>
    [ClientRpc]
    private void RotatePlayerClientRpc(Quaternion targetRotation)
    {
        // Update rotation for other clients
        transform.rotation = targetRotation;
    }
    #endregion

    #endregion
}
