using UnityEngine;

/// <summary>
/// This class handles all the logic for each individual chain segment in the chain 
/// </summary>
public class ChainSegment : MonoBehaviour
{
    private Rigidbody _chainSegmentRigidBody;

    private void Awake()
    {
        _chainSegmentRigidBody = GetComponent<Rigidbody>();

        _chainSegmentRigidBody.solverIterations = 50;

        DontDestroyOnLoad(gameObject);
    }

}


