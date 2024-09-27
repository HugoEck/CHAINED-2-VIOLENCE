using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationStateController : MonoBehaviour
{
    private Animator _animator;
    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _attackAction;

    int _isWalkingHash;
    int _isAttackingHash;

    void Start()
    {
        // Get references to the Animator and PlayerInput components
        _animator = GetComponentInChildren<Animator>();
        _playerInput = GetComponent<PlayerInput>();

        if (_playerInput == null)
        {
            Debug.LogError("PlayerInput component is missing on the Player GameObject!");
            return; // Early exit if the PlayerInput component is missing
        }

        // Get the movement and attack actions
        _moveAction = _playerInput.actions["PlayerMovementAction"];
        _attackAction = _playerInput.actions["AttackAction"];

        // Check if actions are initialized correctly
        if (_moveAction == null)
        {
            Debug.LogError("PlayerMovementAction is not found in the Input Actions Asset!");
        }
        else
        {
            Debug.Log("PlayerMovementAction initialized successfully.");
        }

        if (_attackAction == null)
        {
            Debug.LogError("AttackAction is not found in the Input Actions Asset!");
        }
        else
        {
            Debug.Log("AttackAction initialized successfully.");
        }

        // Cache Animator parameter hashes for performance
        _isWalkingHash = Animator.StringToHash("isWalking");
        _isAttackingHash = Animator.StringToHash("isAttacking");
    }

    void Update()
    {
        if (_moveAction == null || _attackAction == null)
        {
            Debug.LogError("Input actions are not properly initialized in the Update method.");
            return;
        }

        // Read movement input
        Vector2 moveInput = _moveAction.ReadValue<Vector2>();
        bool isWalking = moveInput.magnitude > 0;

        // Update walking animation based on input
        _animator.SetBool(_isWalkingHash, isWalking);

        // Check if the attack action is triggered
        bool isAttacking = _attackAction.triggered;
        _animator.SetBool(_isAttackingHash, isAttacking);
    }
}
