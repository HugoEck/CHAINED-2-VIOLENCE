using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private float _walkingSpeed = 200.0f;
    [SerializeField] private float _playerRotateSpeed = 5.0f;

    private Camera _mainCameraReference;
    private Rigidbody _playerRigidBody;
    private InputAction _moveAction;
    private PlayerInput _playerInput;
    private Vector2 _playerMoveDirection = Vector2.zero;

    private void Start()
    {
        _mainCameraReference = Camera.main;
        _playerRigidBody = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions.FindAction("PlayerMovementAction");
    }

    private void Update()
    {
        _playerMoveDirection = _moveAction.ReadValue<Vector2>();
    }

    #region Player Movement & Camera rotation

    public void MovePlayer()
    {

        RotatePlayerToCursor();

        // get the current forward and right direction relative to where the player is facing
        Vector3 playerForward = transform.forward;
        Vector3 playerRight = transform.right;

        // calculate movement based on WASD input and direction relative to cursor
        Vector3 moveDirection = playerForward * _playerMoveDirection.y + playerRight * _playerMoveDirection.x;

        // normalize to avoid faster diagonal movement
        if (moveDirection.magnitude > 1)
        {
            moveDirection.Normalize();
        }

        float playerSpeedDt = _walkingSpeed * Time.deltaTime;

        MovePlayerServerRpc(moveDirection * playerSpeedDt);
    }

    [ServerRpc]
    public void MovePlayerServerRpc(Vector3 moveDirection)
    {
        Vector3 newVelocity = new Vector3(moveDirection.x, _playerRigidBody.velocity.y, moveDirection.z);
        _playerRigidBody.velocity = newVelocity;
    }

    public void RotatePlayerToCursor()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Ray ray = _mainCameraReference.ScreenPointToRay(mouseScreenPosition);

        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            Vector3 directionToLook = hitPoint - transform.position;
            directionToLook.y = 0;

            if (directionToLook != Vector3.zero)
            {
                float playerRotationSpeedDt = _playerRotateSpeed * Time.deltaTime;
                RotatePlayerServerRpc(directionToLook, playerRotationSpeedDt);
            }
        }
    }

    [ServerRpc]
    public void RotatePlayerServerRpc(Vector3 directionToLook, float playerRotateSpeed)
    {
        Quaternion targetRotation = Quaternion.LookRotation(directionToLook);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, playerRotateSpeed);
        RotatePlayerClientRpc(transform.rotation);
    }

    [ClientRpc]
    private void RotatePlayerClientRpc(Quaternion targetRotation)
    {
        transform.rotation = targetRotation;
    }

    #endregion
}
