using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public float attackCooldown = 1f; // Cooldown time between attacks
    public float abilityCooldown = 5f; // Cooldown time between abilities
    public float attackDamage = 10f; // Damage dealt per attack
    public float abilityDamage = 50f; // Damage dealt by the ability
    public float attackRange = 2f; // The range within which the attack can hit
    public GameObject sharpObject; // The object that becomes sharp
    public float sharpDuration = 2f; // Duration for which the object remains sharp
    public Material sharpMaterial; // Material for sharp state
    public Material bluntMaterial; // Material for blunt state

    private float lastAttackTime;
    private float lastAbilityTime;
    private bool isSharp = false; // Whether the object is currently sharp
    private Collider sharpCollider; // Reference to the sharp object's collider
    private SharpObjectHandler sharpHandler; // Reference to the sharp object's handler

    void Start()
    {
        if (sharpObject != null)
        {
            // Get the collider of the sharp object
            sharpCollider = sharpObject.GetComponent<Collider>();
            sharpHandler = sharpObject.GetComponent<SharpObjectHandler>();

            if (sharpCollider != null)
            {
                sharpCollider.isTrigger = false; // Initially set to not sharp
            }

            if (sharpHandler != null)
            {
                sharpHandler.SetSharpness(false, 0); // Initially not sharp
            }

            // Set the initial material to blunt
            SetObjectMaterial(bluntMaterial);
        }
    }

    //void Update()
    //{
    //    HandleInput();
    //}

    public void HandleInput()
    {
        // Handle attack input
        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time > lastAttackTime + attackCooldown)
        {
            Attack();
        }

        // Handle ability input
        if (Input.GetKeyDown(KeyCode.Mouse1) && Time.time > lastAbilityTime + abilityCooldown)
        {
            UseAbility();
        }
    }

    void Attack()
    {
        lastAttackTime = Time.time;
        Debug.Log("Attacking!"); // Optional: Show debug message

        // Find all colliders within attack range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy")) // Check if it is an enemy
            {
                BaseManager enemy = hitCollider.GetComponent<BaseManager>();
                if (enemy != null)
                {
                    enemy.DealDamageToEnemy(attackDamage); // Call TakeDamage on the enemy
                    Debug.Log("Hit enemy: " + hitCollider.name); // Output debug message
                }
            }
        }
    }

    void UseAbility()
    {
        if (sharpObject != null && !isSharp)
        {
            lastAbilityTime = Time.time;
            Debug.Log("Used ability! Object is now sharp!");

            // Enable sharpness
            EnableSharpness();

            // Disable sharpness after a duration
            Invoke("DisableSharpness", sharpDuration);
        }
    }

    void EnableSharpness()
    {
        // Set the collider to trigger for sharp interaction
        if (sharpCollider != null)
        {
            sharpCollider.isTrigger = true; // Enable trigger to hit enemies
        }

        if (sharpHandler != null)
        {
            sharpHandler.SetSharpness(true, abilityDamage); // Enable sharpness and set damage
        }

        // Change the material to indicate sharpness
        SetObjectMaterial(sharpMaterial);

        isSharp = true; // Mark as sharp
    }

    void DisableSharpness()
    {
        // Set the collider to non-trigger for blunt interaction
        if (sharpCollider != null)
        {
            sharpCollider.isTrigger = false; // Disable trigger
        }

        if (sharpHandler != null)
        {
            sharpHandler.SetSharpness(false, 0); // Disable sharpness
        }

        // Change the material to indicate bluntness
        SetObjectMaterial(bluntMaterial);

        isSharp = false; // Mark as not sharp
        Debug.Log("Sharpness disabled.");
    }

    void SetObjectMaterial(Material material)
    {
        // Set the object's material if the Renderer and Material are available
        Renderer objectRenderer = sharpObject.GetComponent<Renderer>();
        if (objectRenderer != null && material != null)
        {
            objectRenderer.material = material;
        }
    }

    // Visualize the attack range in the Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}