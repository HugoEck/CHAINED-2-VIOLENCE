using Obi;
using UnityEngine;

/// <summary>
/// This class handles the laser chain segments such as damage and collisions
/// </summary>
public class LaserChainSegment : MonoBehaviour
{

    [Header("Layers to collide with")]
    [SerializeField] private LayerMask _layersToCollideWith;
     
    [Header("Laser segment attributes")]
    [SerializeField] private float _laserChainDamage;

    public bool bIsFirstSpawnedLaserSegment = false;
    public ObiRopeLineRenderer laserChainRenderer { get; set; } // This will be set for the first segment that's spawned in "SpawnAbilityChainSegments"
    public ObiRopeChainRenderer normalChain1 { get; set; } // The first segment that is spawned disables the normal chain
    public ObiRopeChainRenderer normalChain2 { get; set; } // Same as normalChain1 since there are 2 of these components

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & _layersToCollideWith) != 0)
        {
            BaseManager enemyManager = other.GetComponent<BaseManager>();
            if (enemyManager != null)
            {
                enemyManager.DealDamageToEnemy(_laserChainDamage);
            }
        }
    }
}
