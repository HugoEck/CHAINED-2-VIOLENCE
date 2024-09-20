using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 50;

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("Enemy took " + damage + " damage!");

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject); // Destroy the enemy object
    }
}