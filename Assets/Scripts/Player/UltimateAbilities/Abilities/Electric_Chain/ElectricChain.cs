using System;
using UnityEngine;

/// <summary>
/// This script handles the electric chain ultimate ability 
/// </summary>
public class ElectricChain : MonoBehaviour, IUltimateAbility
{
    [Header("Electric chain attributes")]
    [SerializeField] private float _electricityChainActiveDuration = 10.0f;
    [SerializeField] private float _electricityChainCooldown = 20.0f;

    [Header("Spawner")]
    [SerializeField] private SpawnAbilityChainSegments _spawner;

    private bool _bIsElectricityChainActive = false;
    private bool _bCanUseElectricityChain = true;

    private float _electricityChainTimer;
    public float _cooldownTimer { get; set; }

    public void Activate()
    {
        _bIsElectricityChainActive = true;
        _electricityChainTimer = _electricityChainActiveDuration;
        _bCanUseElectricityChain = false;
        Debug.Log("Electricity Chain Activated");
        _spawner.SpawnElectricChainSegments();
    }

    public void Deactivate()
    {
        _bIsElectricityChainActive = false;
        _cooldownTimer = _electricityChainCooldown;
        Debug.Log("Electricity Chain Deactivated");
        _spawner.DeactivateElectricChainSegments();
        UltimateAbilityManager.instance._bIsBothPlayersUsingUltimate = false;
    }

    public void UpdateUltimateAttack()
    {
        if (_bIsElectricityChainActive)
        {
            _electricityChainTimer -= Time.deltaTime;
            if (_electricityChainTimer <= 0)
            {
                Deactivate();
            }
            else
            {
                _spawner.UpdateElectricChainSegments();
            }
        }

        // Update the cooldown timer
        if (!_bCanUseElectricityChain && !_bIsElectricityChainActive)
        {
            _cooldownTimer -= Time.deltaTime;
            if (_cooldownTimer <= 0)
            {
                _bCanUseElectricityChain = true;
                Debug.Log("Electricity Chain Cooldown Finished");
            }
        }
    }

    public void UseUltimate()
    {
        if (_bCanUseElectricityChain && !_bIsElectricityChainActive)
        {
            Activate();
        }
    }    
}
