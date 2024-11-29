using UnityEngine;

/// <summary>
/// This script handles the laser chain segments such as damage and collisions
/// </summary>
public class LaserChainSegment : MonoBehaviour
{
    [Header("Layers to collide with")]
    [SerializeField] private LayerMask _layersToCollideWith;
     
    [Header("Laser segment attributes")]
    [SerializeField] private float _laserChainDamage;

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & _layersToCollideWith) != 0)
        {
            
            BaseManager enemyManager = other.GetComponent<BaseManager>();
            if (enemyManager != null)
            {
                enemyManager.DealDamageToEnemy(_laserChainDamage, BaseManager.DamageType.UltimateAbilityDamage);
            }
        }
    }
}
