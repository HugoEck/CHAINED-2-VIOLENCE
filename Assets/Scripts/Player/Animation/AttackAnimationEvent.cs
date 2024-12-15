using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationEvent : MonoBehaviour
{
    private AnimationStateController _animationStateController;

    private int _playerId;

    void Start()
    {
        _animationStateController = GetComponentInParent<AnimationStateController>();
        
        Player player = GetComponentInParent<Player>();

        _playerId = player._playerId;
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
        _animationStateController._animator.SetBool("WeaponSlash", true);
    }
    public void UseRangedAbility()
    {
        if(_playerId == 1)
        {
            Projectile.player1ThrowGrenade = true;
        }
        else if(_playerId == 2)
        {
            Projectile.player2ThrowGrenade = true;
        }
    }
    
}