using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script handles spawning ground fire wherever the fire chain is
/// </summary>
public class SpawnGroundFire : MonoBehaviour
{
    [SerializeField] private GameObject _groundFireParticle;
    [SerializeField] private float _maxRayDistance = 100f;  
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _fireLayer;

    private const int AMOUNT_OF_OBJECTS_TO_POOL = 100;

    private List<GameObject> _pooledGroundFire;
    private List<GroundFire> _groundFireTimers;

    private void Start()
    {
        _groundFireTimers = new List<GroundFire>();
        InstantiateGroundFire();        
    }

    private void Update()
    {
        UpdateGroundFireEmitter(); 
    }

    public void UpdateGroundFireEmitter()
    {
        // Update each ground fire (contdown for if its active or not)
        foreach (GroundFire groundFire in _groundFireTimers)
        {
            if (!groundFire.bIsGroundFireActive)
            {
                DeactivateGroundFireParticle(groundFire.gameObject);
                continue;
            }
            groundFire.UpdateGroundFire();
        }

        if (!FireChain._bIsFireChainActive) return;

        
        int combinedLayerMask = _groundLayer | _fireLayer;

        // Use raycast to find ground, if there is already a fire spawned at that location, don't spawn ground fire
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, _maxRayDistance, combinedLayerMask))
        {
            // Check if the first hit is on the ground layer
            if ((1 << hit.collider.gameObject.layer & _groundLayer) != 0)
            {
                // Perform an additional check within a small radius to see if fire already exists at this location
                Collider[] nearbyColliders = Physics.OverlapSphere(hit.point, 0.5f, _fireLayer);
                bool fireAlreadyExists = nearbyColliders.Length > 0;

                if (!fireAlreadyExists)
                {
                    // Spawn ground fire if no fire exists at this location
                    SpawnGroundFireParticle(hit.point);
                }
            }
        }

        
    }

    private void InstantiateGroundFire()
    {
        GameObject fireChainParent = new GameObject("Ground_Fire_Particles");
        

        // Initialize the object pool list
        _pooledGroundFire = new List<GameObject>();

        GameObject tmpGroundFire;

        for (int i = 0; i < AMOUNT_OF_OBJECTS_TO_POOL; i++)
        {
            tmpGroundFire = Instantiate(_groundFireParticle);
            tmpGroundFire.SetActive(false);

            tmpGroundFire.transform.SetParent(fireChainParent.transform);

            _pooledGroundFire.Add(tmpGroundFire);
            _groundFireTimers.Add(tmpGroundFire.gameObject.GetComponent<GroundFire>());
        }
    }

    public void DeactivateGroundFireParticle(GameObject groundFireToDeactivate)
    {
        for (int i = 0; i < _pooledGroundFire.Count; i++)
        {
            if (_pooledGroundFire[i] == groundFireToDeactivate)
            {
                _pooledGroundFire[i].SetActive(false);
                return;
            }
        }
    }

    public void SpawnGroundFireParticle(Vector3 groundPosition)
    {
        for (int i = 0; i < _pooledGroundFire.Count; i++)
        {
            if (!_pooledGroundFire[i].activeInHierarchy)
            {
                // Update the position before setting the object active
                _pooledGroundFire[i].transform.position = groundPosition;
                // Now activate the object
                _pooledGroundFire[i].SetActive(true);
                return;
            }
        }
    }
}

