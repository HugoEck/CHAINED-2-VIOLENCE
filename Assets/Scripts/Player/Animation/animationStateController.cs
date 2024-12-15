using System.Collections;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    public Animator _animator;
    private AnimationStateController _parentController;
    private PlayerMovement _playerMovement;
    private Camera _mainCamera;
    private PlayerCombat _playerCombat;

    private bool _bIsUsingBasicAttack = false;
    private Player _player;
    private float _lastAttackedTime = 0f;
    private float _attackSpeed = 1f;
    private bool _isAttackPlaying = false;

    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
        _mainCamera = Camera.main;
        _playerCombat = GetComponent<PlayerCombat>();
        _player = gameObject.GetComponent<Player>();
        _parentController = GetComponentInParent<AnimationStateController>();
    }

    void Update()
    {
        if (_mainCamera == null)
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

        _animator.SetFloat("MovementX", adjustedMovementInput.x, 0.2f, Time.deltaTime);
        _animator.SetFloat("MovementY", adjustedMovementInput.y, 0.2f, Time.deltaTime);


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

    public void StartAttackAnimation()
    {
        //Debug.Log("Attack initiated");
        _isAttackPlaying = true;
        _animator.SetBool("isAttacking", true);
        Invoke("StopAttackAnimation", 0.5f);
    }

    private void StopAttackAnimation()
    {
        _isAttackPlaying = false;
        _animator.SetBool("isAttacking", false);
    }

    private float GetAnimationClipLength(string clipName)
    {
        AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;

        foreach (AnimationClip clip in clips)
        {
            if (clip.name == clipName)
            {
                return clip.length;
            }
        }
        Debug.LogWarning("Animation clip not found: " + clipName);
        return 0f;
    }

    public bool IsAttackAllowed()
    {
        if (Time.time > _lastAttackedTime + _attackSpeed)
        {
            _lastAttackedTime = Time.time;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AttackIsCalled()
    {
        //Debug.Log("Base Attack is used, animation event called");
        _playerCombat.UseBaseAttack();
    }
}
