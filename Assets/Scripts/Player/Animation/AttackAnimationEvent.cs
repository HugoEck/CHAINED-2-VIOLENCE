using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationEvent : MonoBehaviour
{
    private AnimationStateController _animationStateController;

    void Start()
    {
        _animationStateController = GetComponentInParent<AnimationStateController>();
    }

    public void AnimationEventAttack()
    {
        if (_animationStateController != null)
        {
            _animationStateController.AttackIsCalled();
        }
    }

    public void DealDamage()
    {
        _animationStateController._animator.SetBool("DealDamage", true);
    }

}