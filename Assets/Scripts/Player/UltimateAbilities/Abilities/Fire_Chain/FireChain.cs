using UnityEngine;

/// <summary>
/// This script handles the fire chain ultimate ability (activation, timers etc)
/// </summary>
public class FireChain : MonoBehaviour, IUltimateAbility
{
    [Header("Electric chain attributes")]
    [SerializeField] private float _fireChainActiveDuration = 10.0f;
    [SerializeField] private float _fireChainCooldown = 20.0f;

    public static bool _bIsFireChainActive = false;
    private bool _bCanUseFireChain = true;

    private float _fireChainTimer;
    private float _cooldownTimer;

    public void Activate()
    {
        _bIsFireChainActive = true;
        _fireChainTimer = _fireChainActiveDuration;
        _bCanUseFireChain = false;
        Debug.Log("Fire Chain Activated");
        SpawnAbilityChainSegments.instance.SpawnFireChainSegments();
    }

    public void Deactivate()
    {
        _bIsFireChainActive = false;
        _cooldownTimer = _fireChainCooldown;
        Debug.Log("Fire Chain Deactivated");
        SpawnAbilityChainSegments.instance.DeactivateFireChainSegments();
        UltimateAbilityManager.instance._bIsBothPlayersUsingUltimate = false;
    }

    public void UpdateUltimateAttack()
    {
        if (_bIsFireChainActive)
        {
            _fireChainTimer -= Time.deltaTime;
            if (_fireChainTimer <= 0)
            {
                Deactivate();
            }
            else
            {
                SpawnAbilityChainSegments.instance.UpdateFireChainSegments();
            }
        }

        // Update the cooldown timer
        if (!_bCanUseFireChain && !_bIsFireChainActive)
        {
            _cooldownTimer -= Time.deltaTime;
            if (_cooldownTimer <= 0)
            {
                _bCanUseFireChain = true;
                Debug.Log("Fire Chain Cooldown Finished");
            }
        }
    }

    public void UseUltimate()
    {
        if (_bCanUseFireChain && !_bIsFireChainActive)
        {
            Activate();
        }
    }

    
}
