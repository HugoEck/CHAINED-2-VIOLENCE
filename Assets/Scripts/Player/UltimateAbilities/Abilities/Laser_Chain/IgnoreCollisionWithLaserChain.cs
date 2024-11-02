using Obi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollisionWithLaserChain : MonoBehaviour
{
    private ObiCollider _obiCollider;

    private bool _colliderEnabled = true;

    private void Start()
    {
        _obiCollider = GetComponent<ObiCollider>();
    }
    private void Update()
    {
        if(LaserChain._bIsLaserActive)
        {
            _obiCollider.enabled = false;
            _colliderEnabled = false;
        }
        else
        {
            if(!_colliderEnabled)
            {
                _obiCollider.enabled = true;
                _colliderEnabled = true;
            }            
        }
    }
}
