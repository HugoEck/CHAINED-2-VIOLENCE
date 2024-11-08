using Obi;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script handles all the chain segment related abilites (like laser chain or electric chain etc..), It is designed using an object pool
/// </summary>
public class SpawnAbilityChainSegments : MonoBehaviour
{
    public static SpawnAbilityChainSegments instance { get; private set; }

    [Header("Laser chain segment prefab")]
    [SerializeField] private GameObject _laserSegment;

    private ObiActor _chain;

    private const int AMOUNT_OF_OBJECTS_TO_POOL = 100;

    private List<GameObject> _pooledLaserChainSegments;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        _chain = GetComponent<ObiActor>();

        InstantiateLaserChainSegments();
    }

    #region Laser chain

    /// <summary>
    /// This method is called when you want to spawn all the laser chain segments
    /// </summary>
    public void SpawnLaserChainSegments()
    {
        for (int i = 0; i < _chain.activeParticleCount; i++)
        {
            if (!_pooledLaserChainSegments[i].activeInHierarchy)
            {
                // Update the position before setting the object active
                _pooledLaserChainSegments[i].transform.position = _chain.GetParticlePosition(_chain.solverIndices[i]);

                // Now activate the object
                _pooledLaserChainSegments[i].SetActive(true);
            }
        }
    }

    /// <summary>
    /// This method is called to update the position of all the laser chain segments
    /// </summary>
    public void UpdateLaserChainSegments()
    {
        for (int i = 0; i < _chain.activeParticleCount; i++)
        {
            if (_pooledLaserChainSegments[i].activeInHierarchy)
            {
                _pooledLaserChainSegments[i].transform.position = _chain.GetParticlePosition(_chain.solverIndices[i]);
            }
        }
    }

    /// <summary>
    /// This method is called to set all the active laser chain segments to inactive
    /// </summary>
    public void DeactivateLaserChainSegments()
    {
        for (int i = 0; i < _chain.activeParticleCount; i++)
        {
            if (_pooledLaserChainSegments[i].activeInHierarchy)
            {
                _pooledLaserChainSegments[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// This method instantiates the laser chain segment object pool
    /// </summary>
    private void InstantiateLaserChainSegments()
    {
        GameObject laserChainParent = new GameObject("Laser_Chain_Segments");

        // Initialize the object pool list
        _pooledLaserChainSegments = new List<GameObject>();

        GameObject tmpLaserChain;

        for (int i = 0; i < AMOUNT_OF_OBJECTS_TO_POOL; i++)
        {
            tmpLaserChain = Instantiate(_laserSegment);
            tmpLaserChain.SetActive(false);

            tmpLaserChain.transform.SetParent(laserChainParent.transform);

            _pooledLaserChainSegments.Add(tmpLaserChain);
        }
    }

    #endregion
}

