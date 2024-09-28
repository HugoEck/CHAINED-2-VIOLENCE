using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChainJointController : MonoBehaviour
{
    [SerializeField] private Rigidbody _playerRigidBody;

    private Rigidbody _chainJointRigidBody;

    private void Start()
    {
        _chainJointRigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Keep the chain joint's rotation fixed (set to no rotation)
        transform.rotation = Quaternion.identity;

        //_playerRigidBody.AddForce(_chainJointRigidBody.GetAccumulatedForce());
    }
}

