using UnityEngine;

public class SharpObjectHandler : MonoBehaviour
{
    public float damage = 50f; // Default damage dealt by the sharp object
    private bool isSharp = false; // Whether the object is currently sharp

    // Method to activate or deactivate the sharpness of the object
    public void SetSharpness(bool sharp, float damageValue)
    {
        isSharp = sharp;
        damage = damageValue; // Set the damage based on sharpness state
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("CHAIN COLLIDE WITH ENEMY!");
        if (isSharp && other.CompareTag("Enemy")) // Check if the object is sharp and collided with an enemy
        {
            BaseManager enemy = other.GetComponent<BaseManager>();
            if (enemy != null)
            {
                enemy.DealDamageToEnemy(damage, BaseManager.DamageType.TrapsDamage); // Deal damage to the enemy
                Debug.Log("Enemy hit by sharp object: " + other.name);
            }
        }
    }
}