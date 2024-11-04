using UnityEngine;

/// <summary>
/// This script handles collisions for each individual ghost chain segment
/// </summary>
public class GhostChainSegment : MonoBehaviour
{
    [Header("Layers to collide with")]
    [SerializeField] private LayerMask _layersToCollideWith;

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & _layersToCollideWith) != 0)
        {

            BaseManager enemyManager = other.GetComponent<BaseManager>();
            if (enemyManager != null)
            {
                /// CALL MakeEnemyScared METHOD FROM HERE
            }
        }
    }
}
