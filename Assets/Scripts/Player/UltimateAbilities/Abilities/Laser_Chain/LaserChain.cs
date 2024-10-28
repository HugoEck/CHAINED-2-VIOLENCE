using UnityEngine;

/// <summary>
/// This class handles the logic for the laser chain (spawning the segments, updating segments etc)
/// </summary>
public class LaserChain : MonoBehaviour, IUltimateAbility //// MUST USE IUltimateAbility
{
    [Header("Laser chain attributes")]
    [SerializeField] private float _laserActiveDuration = 10.0f;
    [SerializeField] private float _laserChainCooldown = 20.0f;

    private bool _bIsLaserActive = false;  
    private bool _bCanUseLaserChain = true;
    
    private float _laserTimer;
    private float _cooldownTimer;

    /// <summary>
    /// This method is called from the UltimateAbilityManager
    /// </summary>
    public void UseUltimate()
    {     
        if(_bCanUseLaserChain && !_bIsLaserActive)
        {
            Activate();
        }      
    } 

    /// <summary>
    /// This methods activates the laser chain (spawning the laser chain segments)
    /// </summary>
    public void Activate()
    {
        _bIsLaserActive = true;
        _laserTimer = _laserActiveDuration;
        _bCanUseLaserChain = false;
        Debug.Log("Laser Chain Activated");
        SpawnAbilityChainSegments.instance.SpawnLaserChainSegments();
    }

    /// <summary>
    /// This method deactivates the laser chain segments
    /// </summary>
    public void Deactivate()
    {
        _bIsLaserActive = false;
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
        if (_bIsLaserActive)
        {
            _laserTimer -= Time.deltaTime;
            if (_laserTimer <= 0)
            {
                Deactivate();
            }
            else
            {
                SpawnAbilityChainSegments.instance.UpdateLaserChainSegments();
            }
        }

        // Update the cooldown timer
        if (!_bCanUseLaserChain && !_bIsLaserActive)
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
