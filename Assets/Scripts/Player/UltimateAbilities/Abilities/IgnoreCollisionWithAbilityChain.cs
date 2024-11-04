using Obi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private void IgnoreCollisionWithLaserChain()
    {
        if (LaserChain._bIsLaserChainActive && _ignoresCollisionWithLaserChain)
        {
            _obiCollider.enabled = false;
            _colliderEnabled = false;
        }
    }
    private void IgnoreCollisionWithGhostChain()
    {
        if (GhostChain._bIsGhostChainActive)
        {
            _obiCollider.enabled = false;
            _colliderEnabled = false;
        }
    }

    public void ObjectIgnoresLaserChain()
    {
        _ignoresCollisionWithLaserChain = true;
    }
}
