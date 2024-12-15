using UnityEngine;

/// <summary>
/// This class handles the logic for the laser chain (spawning the segments, updating segments etc)
/// </summary>
public class LaserChain : MonoBehaviour, IUltimateAbility //// MUST USE IUltimateAbility
{
    [Header("Laser chain attributes")]
    [SerializeField] private float _laserChainActiveDuration = 10.0f;
    [SerializeField] private float _laserChainCooldown = 20.0f;

    [Header("Spawner")]
    [SerializeField] private SpawnAbilityChainSegments _spawner;

    public static bool _bIsLaserChainActive = false;  
    private bool _bCanUseLaserChain = true;
    
    private float _laserChainTimer;
    public float _cooldownTimer { get; set; }

    public void UseUltimate()
    {     
        if(_bCanUseLaserChain && !_bIsLaserChainActive)
        {
            Activate();
        }      
    } 

    public void Activate()
    {
        _bIsLaserChainActive = true;
        _laserChainTimer = _laserChainActiveDuration;
        _bCanUseLaserChain = false;
        //Debug.Log("Laser Chain Activated");
        _spawner.SpawnLaserChainSegments();
    }

    public void Deactivate()
    {
        _bIsLaserChainActive = false;
        _cooldownTimer = _laserChainCooldown; 
        //Debug.Log("Laser Chain Deactivated");
        _spawner.DeactivateLaserChainSegments();
        UltimateAbilityManager.instance._bIsBothPlayersUsingUltimate = false;
    }

    public void UpdateUltimateAttack()
    {
        if (_bIsLaserChainActive)
        {
            _laserChainTimer -= Time.deltaTime;
            if (_laserChainTimer <= 0)
            {
                Deactivate();
            }
            else
            {
                _spawner.UpdateLaserChainSegments();
            }
        }

        // Update the cooldown timer
        if (!_bCanUseLaserChain && !_bIsLaserChainActive)
        {
            _cooldownTimer -= Time.deltaTime;
            if (_cooldownTimer <= 0)
            {
                _bCanUseLaserChain = true;
                //Debug.Log("Laser Chain Cooldown Finished");
            }
        }
    }
}
