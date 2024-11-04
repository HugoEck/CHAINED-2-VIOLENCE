using UnityEngine;

/// <summary>
/// This class handles the logic for the laser chain (spawning the segments, updating segments etc)
/// </summary>
public class LaserChain : MonoBehaviour, IUltimateAbility //// MUST USE IUltimateAbility
{
    [Header("Laser chain attributes")]
    [SerializeField] private float _laserChainActiveDuration = 10.0f;
    [SerializeField] private float _laserChainCooldown = 20.0f;

    public static bool _bIsLaserChainActive = false;  
    private bool _bCanUseLaserChain = true;
    
    private float _laserChainTimer;
    private float _cooldownTimer;

    /// <summary>
    /// This method is called from the UltimateAbilityManager
    /// </summary>
    public void UseUltimate()
    {     
        if(_bCanUseLaserChain && !_bIsLaserChainActive)
        {
            Activate();
        }      
    } 

    /// <summary>
    /// This methods activates the laser chain (spawning the laser chain segments)
    /// </summary>
    public void Activate()
    {
        _bIsLaserChainActive = true;
        _laserChainTimer = _laserChainActiveDuration;
        _bCanUseLaserChain = false;
        Debug.Log("Laser Chain Activated");
        SpawnAbilityChainSegments.instance.SpawnLaserChainSegments();
    }

    /// <summary>
    /// This method deactivates the laser chain segments
    /// </summary>
    public void Deactivate()
    {
        _bIsLaserChainActive = false;
        _cooldownTimer = _laserChainCooldown; 
        Debug.Log("Laser Chain Deactivated");
        SpawnAbilityChainSegments.instance.DeactivateLaserChainSegments();
        UltimateAbilityManager.instance._bIsBothPlayersUsingUltimate = false;
    }

    /// <summary>
    /// This method handles the update of the laser chain segments and updates timers
    /// </summary>
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
                SpawnAbilityChainSegments.instance.UpdateLaserChainSegments();
            }
        }

        // Update the cooldown timer
        if (!_bCanUseLaserChain && !_bIsLaserChainActive)
        {
            _cooldownTimer -= Time.deltaTime;
            if (_cooldownTimer <= 0)
            {
                _bCanUseLaserChain = true;
                Debug.Log("Laser Chain Cooldown Finished");
            }
        }
    }
}
