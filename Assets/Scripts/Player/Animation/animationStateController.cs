using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    private Animator _animator;
    private PlayerMovement _playerMovement;
    private Camera _mainCamera;

    private bool _bIsUsingBasicAttack = false;

    private Player _player;


    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
        _mainCamera = Camera.main;

        _player = gameObject.GetComponent<Player>();
    }

    void Update()
    {
        if(_mainCamera == null)
        {
            _mainCamera = Camera.main;
        }
 
        Vector2 movementInput = _playerMovement.GetMovementInput();

        Vector3 playerForward = transform.forward; 
        Vector3 cameraForward = _mainCamera.transform.forward;
        cameraForward.y = 0; 

       
        float forwardDot = Vector3.Dot(playerForward, cameraForward); 
        float rightDot = Vector3.Dot(transform.right, cameraForward);

        Vector2 adjustedMovementInput = AdjustMovementInputBasedOnFacing(movementInput, forwardDot, rightDot);

        _animator.SetFloat("MovementX", adjustedMovementInput.x);
        _animator.SetFloat("MovementY", adjustedMovementInput.y);

        
        bool isMoving = movementInput.magnitude > 0;
        _animator.SetBool("isMoving", isMoving);

        GetPlayerCombatInput();
    }

   
    private Vector2 AdjustMovementInputBasedOnFacing(Vector2 movementInput, float forwardDot, float rightDot)
    {
        Vector2 adjustedMovement = movementInput;

        
        if (forwardDot < 0)
        {
            adjustedMovement.y = -movementInput.y;
        }

        
        if (rightDot > 0.5f)  
        {
            adjustedMovement = new Vector2(movementInput.y, -movementInput.x);
        }
        else if (rightDot < -0.5f) 
        {
            adjustedMovement = new Vector2(-movementInput.y, movementInput.x); 
        }

        return adjustedMovement;
    }
    private void GetPlayerCombatInput()
    {
        if (_player._playerId == 1)
        {
            _bIsUsingBasicAttack = InputManager.Instance.GetBasicAttackInput_P1();

            if (_bIsUsingBasicAttack)
            {
                _animator.SetBool("isAttacking", true);
                Invoke("StopAttacking", 0.5f);  // Adjust the delay based on the attack animation duration
            }
        }
        else if (_player._playerId == 2)
        {
            _bIsUsingBasicAttack = InputManager.Instance.GetBasicAttackInput_P2();

            if (_bIsUsingBasicAttack)
            {
                _animator.SetBool("isAttacking", true);
                Invoke("StopAttacking", 0.5f);  // Adjust the delay based on the attack animation duration
            }
        }
    }

    // Method to reset the isAttacking flag after the attack animation finishes
    private void StopAttacking()
    {
        _animator.SetBool("isAttacking", false);
    }


}
