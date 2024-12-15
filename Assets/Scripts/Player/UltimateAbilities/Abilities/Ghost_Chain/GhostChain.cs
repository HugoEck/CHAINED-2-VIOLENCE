using UnityEngine;

/// <summary>
/// This script handles the ghost chain ultimate ability (activation, timer etc)
/// </summary>
public class GhostChain : MonoBehaviour, IUltimateAbility
{
    [Header("Ghost chain attributes")]
    [SerializeField] private float _ghostChainActiveDuration = 10.0f;
    [SerializeField] private float _ghostChainCooldown = 20.0f;
    [SerializeField] private LayerMask _setIgnoreCollisionLayers;

    [Header("Spawner")]
    [SerializeField] private SpawnAbilityChainSegments _spawner;

    public static LayerMask ignoreCollisionLayers;

    public static bool _bIsGhostChainActive = false;
    private bool _bCanUseGhostChain = true;

    private float _ghostChainTimer;
    public float _cooldownTimer { get; set; }

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
        //Debug.Log("Ghost Chain Activated");
        _spawner.SpawnGhostChainSegments();
    }
   
    public void Deactivate()
    {
        _bIsGhostChainActive = false;
        _cooldownTimer = _ghostChainCooldown;
        //Debug.Log("Ghost Chain Deactivated");
        _spawner.DeactivateGhostChainSegments();
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
                _spawner.UpdateGhostChainSegments();
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
