using UnityEngine;

public class BasicAttack : IBasicAttack
{
    private Vector3 _playerPosition;
    public float _attackRange { get; set; }
    public float _attackDamange { get; set; }

    public BasicAttack(Vector3 playerPosition)
    {
        _playerPosition = playerPosition;
    }
    public void Execute()
    {
        // Basic attack logic, e.g., detect enemies within range and deal damage
        Collider[] hitEnemies = Physics.OverlapSphere(_playerPosition, _attackRange);
        foreach (Collider enemy in hitEnemies)
        {
            BaseManager enemyManager = enemy.GetComponent<BaseManager>();
            if (enemyManager != null)
            {
                enemyManager.DealDamageToEnemy(_attackDamange);
                Debug.Log("Hit enemy: " + enemy.name);
            }
        }
    }
}
