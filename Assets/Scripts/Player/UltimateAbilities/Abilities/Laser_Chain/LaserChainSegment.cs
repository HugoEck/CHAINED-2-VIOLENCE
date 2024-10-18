using UnityEngine;

/// <summary>
/// This class handles the laser chain segments such as damage and collisions
/// </summary>
public class LaserChainSegment : MonoBehaviour
{
    [Header("Laser segment attributes")]
    [SerializeField] private float _laserChainDamage;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            BaseManager enemyManager = other.GetComponent<BaseManager>();
            enemyManager.DealDamageToEnemy(_laserChainDamage);
        }
    }
}
