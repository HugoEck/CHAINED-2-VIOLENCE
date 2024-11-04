using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostChain : MonoBehaviour, IUltimateAbility
{
    [Header("Ghost chain attributes")]
    [SerializeField] private float _ghostChainActiveDuration = 10.0f;
    [SerializeField] private float _ghostChainCooldown = 20.0f;
    [SerializeField] private LayerMask _setIgnoreCollisionLayers;

    public static LayerMask ignoreCollisionLayers;

    public static bool _bIsGhostChainActive = false;
    private bool _bCanUseGhostChain = true;

    private float _ghostChainTimer;
    private float _cooldownTimer;

    private void Start()
    {
        ignoreCollisionLayers = _setIgnoreCollisionLayers.value;
    }

    public void UseUltimate()
    {
        if (_bCanUseGhostChain && !_bIsGhostChainActive)
        {
            Activate();
        }
    }

    public void Activate()
    {
        _bIsGhostChainActive = true;
        _ghostChainTimer = _ghostChainActiveDuration;
        _bCanUseGhostChain = false;
        Debug.Log("Ghost Chain Activated");
        SpawnAbilityChainSegments.instance.SpawnGhostChainSegments();
    }

    public void Deactivate()
    {
        _bIsGhostChainActive = false;
        _cooldownTimer = _ghostChainCooldown;
        Debug.Log("Ghost Chain Deactivated");
        SpawnAbilityChainSegments.instance.DeactivateGhostChainSegments();
        UltimateAbilityManager.instance._bIsBothPlayersUsingUltimate = false;
    }

    public void UpdateUltimateAttack()
    {
        if (_bIsGhostChainActive)
        {
            _ghostChainTimer -= Time.deltaTime;
            if (_ghostChainTimer <= 0)
            {
                Deactivate();
            }
            else
            {
                SpawnAbilityChainSegments.instance.UpdateGhostChainSegments();
            }
        }

        // Update the cooldown timer
        if (!_bCanUseGhostChain && !_bIsGhostChainActive)
        {
            _cooldownTimer -= Time.deltaTime;
            if (_cooldownTimer <= 0)
            {
                _bCanUseGhostChain = true;
                Debug.Log("Ghost Chain Cooldown Finished");
            }
        }
    }
}
