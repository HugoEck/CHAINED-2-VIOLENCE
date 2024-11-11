using System.Collections;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    public Animator _animator;
    private PlayerMovement _playerMovement;
    private Camera _mainCamera;

    private bool _bIsUsingBasicAttack = false;

    private Player _player;

    private bool _isAttackPlaying = false;
    private PlayerCombat _playerCombat;




    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
        _mainCamera = Camera.main;
        _playerCombat = GetComponent<PlayerCombat>();

        _player = gameObject.GetComponent<Player>();
    }

    void Update()
    {
        if(_mainCamera == null)
        {
            _mainCamera = Camera.main;
        }
 
        Vector2 movementInput = _player.GetMovementInput();

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
    //private void GetPlayerCombatInput()
    //{
    //    if (_player._playerId == 1)
    //    {
    //        _bIsUsingBasicAttack = InputManager.Instance.GetBasicAttackInput_P1();

    //        if (_bIsUsingBasicAttack)
    //        {
    //            _animator.SetBool("isAttacking", true);
    //            Invoke("StopAttacking", 0.5f); 
    //        }
    //    }
    //    else if (_player._playerId == 2)
    //    {
    //        _bIsUsingBasicAttack = InputManager.Instance.GetBasicAttackInput_P2();

    //        if (_bIsUsingBasicAttack)
    //        {
    //            _animator.SetBool("isAttacking", true);
    //            Invoke("StopAttacking", 0.5f); 
    //        }
    //    }
    //}

    public void TriggerAttackAnimation()
    {
        if (!_isAttackPlaying) 
        {
            _isAttackPlaying = true;
            _animator.SetBool("isAttacking", true);
            StartCoroutine(AttackRoutine());
        }
    }

    private IEnumerator AttackRoutine()
    {
        float attackDuration = GetAnimationClipLength("Player_Melee_Horizontal");

        yield return new WaitForSeconds(attackDuration / 8);
        _playerCombat.UseBaseAttack();
        Debug.Log("Player 1 attacked Once");

        yield return new WaitForSeconds(attackDuration / 8);

        _animator.SetBool("isAttacking", false);
        _isAttackPlaying = false;
    }

    private float GetAnimationClipLength(string clipName)
    {
        foreach (var clip in _animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
                return clip.length;
        }
        Debug.LogWarning("Animation clip not found: " + clipName);
        return 0f;
    }


}
