using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 100f; // Enemy's health

    // Method to apply damage to the enemy
    public void TakeDamage(float amount)
    {
        health -= amount; // Reduce health by the amount of damage
        Debug.Log(gameObject.name + " took " + amount + " damage. Remaining health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    // Method to handle enemy death
    void Die()
    {
        Debug.Log(gameObject.name + " died.");
        Destroy(gameObject); // Destroy the enemy GameObject
    }
}