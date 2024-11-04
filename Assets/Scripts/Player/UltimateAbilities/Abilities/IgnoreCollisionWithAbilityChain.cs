using Obi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// /// <summary>
/// /// This script makes the object that is holding it ignore collisions with the chain when a specific ultimate ability is activated
/// /// </summary>
public class IgnoreCollisionWithAbilityChain : MonoBehaviour
{
    private bool _ignoresCollisionWithLaserChain = false;

    private ObiCollider _obiCollider;

    private bool _colliderEnabled = true;

    private void Start()
    {
        _obiCollider = GetComponent<ObiCollider>();
    }
    private void Update()
    {
        if(!LaserChain._bIsLaserChainActive && !GhostChain._bIsGhostChainActive)
        {
            if (!_colliderEnabled)
            {
                _obiCollider.enabled = true;
                _colliderEnabled = true;
            }
        }
        else
        {
            IgnoreCollisionWithLaserChain();
            IgnoreCollisionWithGhostChain();
        }
    }

    /// <summary>
    /// For laser chain
    /// </summary>
    private void IgnoreCollisionWithLaserChain()
    {
        if (LaserChain._bIsLaserChainActive && _ignoresCollisionWithLaserChain)
        {
            _obiCollider.enabled = false;
            _colliderEnabled = false;
        }
    }

    /// <summary>
    /// For ghost chain
    /// </summary>
    private void IgnoreCollisionWithGhostChain()
    {
        if (GhostChain._bIsGhostChainActive)
        {
            _obiCollider.enabled = false;
            _colliderEnabled = false;
        }
    }

    /// <summary>
    /// An object doesn't always need to ignore collision with laser chain, but it does for ghost chain (call this to ignore with laser chain aswell)
    /// </summary>
    public void ObjectIgnoresLaserChain()
    {
        _ignoresCollisionWithLaserChain = true;
    }
}
