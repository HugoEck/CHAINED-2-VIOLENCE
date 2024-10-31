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
    [SerializeField] private GameObject _electricSegment;
    [SerializeField] private GameObject _fireSegment;

    private ObiActor _chain;

    private const int AMOUNT_OF_OBJECTS_TO_POOL = 100;

    private List<GameObject> _pooledLaserChainSegments;
    private List<GameObject> _pooledElectricChainSegments;
    private List<GameObject> _pooledFireChainSegments;

    ObiRopeChainRenderer chainRenderer1;
    ObiRopeChainRenderer chainRenderer2;
    ObiRopeLineRenderer lineRenderer;

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

        ObiRopeChainRenderer[] chainRenderers = gameObject.GetComponents<ObiRopeChainRenderer>();

        chainRenderer1 = chainRenderers[0];
        chainRenderer2 = chainRenderers[1];
        lineRenderer = GetComponent<ObiRopeLineRenderer>();

        InstantiateLaserChainSegments();
        InstantiateElectricChainSegments();
        InstantiateFireChainSegments();
    }

    #region Fire Chain

    /// <summary>
    /// This method is called when you want to spawn all the fire chain segments
    /// </summary>
    public void SpawnFireChainSegments()
    {
        for (int i = 0; i < _chain.activeParticleCount; i++)
        {
            if (!_pooledFireChainSegments[i].activeInHierarchy)
            {
                // Update the position before setting the object active
                _pooledFireChainSegments[i].transform.position = _chain.GetParticlePosition(_chain.solverIndices[i]);

                // Now activate the object
                _pooledFireChainSegments[i].SetActive(true);
            }
        }
    }

    /// <summary>
    /// This method is called to update the position of all the fire chain segments
    /// </summary>
    public void UpdateFireChainSegments()
    {
        for (int i = 0; i < _chain.activeParticleCount; i++)
        {
            if (_pooledFireChainSegments[i].activeInHierarchy)
            {
                _pooledFireChainSegments[i].transform.position = _chain.GetParticlePosition(_chain.solverIndices[i]);
            }
        }
    }

    /// <summary>
    /// This method is called to set all the active fire chain segments to inactive
    /// </summary>
    public void DeactivateFireChainSegments()
    {
        for (int i = 0; i < _chain.activeParticleCount; i++)
        {
            if (_pooledFireChainSegments[i].activeInHierarchy)
            {
                _pooledFireChainSegments[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// This method instantiates the fire chain segment object pool
    /// </summary>
    private void InstantiateFireChainSegments()
    {
        GameObject fireChainParent = new GameObject("Fire_Chain_Segments");

        // Initialize the object pool list
        _pooledFireChainSegments = new List<GameObject>();

        GameObject tmpFireChain;

        for (int i = 0; i < AMOUNT_OF_OBJECTS_TO_POOL; i++)
        {
            tmpFireChain = Instantiate(_fireSegment);
            tmpFireChain.SetActive(false);

            tmpFireChain.transform.SetParent(fireChainParent.transform);

            _pooledFireChainSegments.Add(tmpFireChain);
        }
    }

    #endregion

    #region Electric Chain

    /// <summary>
    /// This method is called when you want to spawn all the electric chain segments
    /// </summary>
    public void SpawnElectricChainSegments()
    {
        for (int i = 0; i < _chain.activeParticleCount; i++)
        {
            if (!_pooledElectricChainSegments[i].activeInHierarchy)
            {
                // Update the position before setting the object active
                _pooledElectricChainSegments[i].transform.position = _chain.GetParticlePosition(_chain.solverIndices[i]);

                // Now activate the object
                _pooledElectricChainSegments[i].SetActive(true);
            }
        }
    }

    /// <summary>
    /// This method is called to update the position of all the electric chain segments
    /// </summary>
    public void UpdateElectricChainSegments()
    {
        for (int i = 0; i < _chain.activeParticleCount; i++)
        {
            if (_pooledElectricChainSegments[i].activeInHierarchy)
            {
                _pooledElectricChainSegments[i].transform.position = _chain.GetParticlePosition(_chain.solverIndices[i]);
            }
        }
    }

    /// <summary>
    /// This method is called to set all the active electric chain segments to inactive
    /// </summary>
    public void DeactivateElectricChainSegments()
    {
        for (int i = 0; i < _chain.activeParticleCount; i++)
        {
            if (_pooledElectricChainSegments[i].activeInHierarchy)
            {
                _pooledElectricChainSegments[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// This method instantiates the electric chain segment object pool
    /// </summary>
    private void InstantiateElectricChainSegments()
    {
        GameObject electricChainParent = new GameObject("Electric_Chain_Segments");

        // Initialize the object pool list
        _pooledElectricChainSegments = new List<GameObject>();

        GameObject tmpElectricChain;

        for (int i = 0; i < AMOUNT_OF_OBJECTS_TO_POOL; i++)
        {
            tmpElectricChain = Instantiate(_electricSegment);
            tmpElectricChain.SetActive(false);

            tmpElectricChain.transform.SetParent(electricChainParent.transform);

            _pooledElectricChainSegments.Add(tmpElectricChain);
        }
    }

    #endregion

    #region Laser chain

    /// <summary>
    /// This method is called when you want to spawn all the laser chain segments
    /// </summary>
    public void SpawnLaserChainSegments()
    {
        chainRenderer1.enabled = false;
        chainRenderer2.enabled = false;

        lineRenderer.enabled = true;

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
        lineRenderer.enabled = false;

        chainRenderer1.enabled = true;
        chainRenderer2.enabled = true;

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

