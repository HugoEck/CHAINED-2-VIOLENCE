using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform otherPlayer; // Reference to the player being swung
    public GameObject[] chainSegments; // Array of chain segments (20 segments)

    public float swingSpeed = 5f; // Speed at which the player is swung
    public float swingDuration = 5f; // Duration of the swing
    public float swingForce = 100f; // Force applied to swing the player
    public float swingDamage = 20f; // Damage dealt by the swing
    public float swingRadius = 10f; // Radius of the swing arc
    public LayerMask enemyLayer; // Layer to detect enemies
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
    private bool isSwinging = false; // Flag to track if the player is swinging another player
    private Rigidbody otherPlayerRb; // Rigidbody of the other player

    void Start()
    {
        if (otherPlayer != null)
        {
            otherPlayerRb = otherPlayer.GetComponent<Rigidbody>(); // Reference to the other player's Rigidbody
        }

        if (sharpObject != null)
        {
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

            SetObjectMaterial(bluntMaterial); // Set the initial material to blunt
        }
    }

    void Update()
    {
        HandleInput();
    }

    public void HandleInput()
    {
        // Handle attack input
        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time > lastAttackTime + attackCooldown)
        {
            Attack();
        }

        // Handle ability input (right mouse button)
        if (Input.GetKeyDown(KeyCode.Mouse1) && Time.time > lastAbilityTime + abilityCooldown)
        {
            UseAbility();
        }

        // Handle tethered swing (Space bar)
        if (Input.GetKeyDown(KeyCode.R) && !isSwinging && otherPlayer != null)
        {
            StartSwing();
        }
    }

    void Attack()
    {
        lastAttackTime = Time.time;
        Debug.Log("Attacking!");

        // Find all colliders within attack range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                BaseManager enemy = hitCollider.GetComponent<BaseManager>();
                if (enemy != null)
                {
                    enemy.SetHealth(attackDamage);
                    Debug.Log("Hit enemy: " + hitCollider.name);
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

            EnableSharpness();
            Invoke("DisableSharpness", sharpDuration);
        }
    }

    void EnableSharpness()
    {
        if (sharpCollider != null)
        {
            sharpCollider.isTrigger = true;
        }

        if (sharpHandler != null)
        {
            sharpHandler.SetSharpness(true, abilityDamage);
        }

        SetObjectMaterial(sharpMaterial);
        isSharp = true;
    }

    void DisableSharpness()
    {
        if (sharpCollider != null)
        {
            sharpCollider.isTrigger = false;
        }

        if (sharpHandler != null)
        {
            sharpHandler.SetSharpness(false, 0);
        }

        SetObjectMaterial(bluntMaterial);
        isSharp = false;
        Debug.Log("Sharpness disabled.");
    }

    void SetObjectMaterial(Material material)
    {
        Renderer objectRenderer = sharpObject.GetComponent<Renderer>();
        if (objectRenderer != null && material != null)
        {
            objectRenderer.material = material;
        }
    }

    // Start the tethered swing (swinging the other player)
    void StartSwing()
    {
        if (!isSwinging)
        {
            // Disable physics for the chain segments
            SetChainPhysicsActive(false);

            isSwinging = true;
            StartCoroutine(SwingOtherPlayer());
        }
    }

    // Disable/Enable physics on the chain segments
    void SetChainPhysicsActive(bool active)
    {
        foreach (GameObject segment in chainSegments)
        {
            Rigidbody rb = segment.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = !active; // Disable physics during swing (set to kinematic)
            }
        }
    }

    // Coroutine to swing the other player around the anchor
    IEnumerator SwingOtherPlayer()
    {
        float elapsedTime = 0f;
        Vector3 swingCenter = transform.position; // This player is the anchor

        while (elapsedTime < swingDuration)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the direction to swing the player
            Vector3 directionToAnchor = (otherPlayer.position - swingCenter).normalized;

            // Calculate the perpendicular force direction (tangential to the swing)
            Vector3 perpendicularForce = Vector3.Cross(directionToAnchor, Vector3.up).normalized;

            // Apply force to simulate swinging in a circular motion
            otherPlayerRb.AddForce(perpendicularForce * swingForce * Time.deltaTime, ForceMode.VelocityChange);

            // Manually position the chain segments between the players
            UpdateChainPositions();

            // Check for enemies hit by the swinging player
            Collider[] hitEnemies = Physics.OverlapSphere(transform.position, swingRadius ); // Adjust radius as needed
            foreach (Collider enemy in hitEnemies)
            {
                BaseManager enemyManager = enemy.GetComponent<BaseManager>();
                if (enemyManager != null)
                {
                    enemyManager.SetHealth(swingDamage); // Apply swing damage
                    Debug.Log("Hit enemy while swinging: " + enemy.name);
                }
            }

            yield return null;
        }

        isSwinging = false;

        // Re-enable physics for the chain segments
        SetChainPhysicsActive(true);

        Debug.Log("Swing ended.");
    }

    // Manually update the positions of the chain segments between the anchor and the swinging player
    void UpdateChainPositions()
    {
        Vector3 startPoint = transform.position; // Anchor player's position
        Vector3 endPoint = otherPlayer.position; // Swinging player's position
        float segmentLength = Vector3.Distance(startPoint, endPoint) / chainSegments.Length;

        for (int i = 0; i < chainSegments.Length; i++)
        {
            Vector3 targetPosition = Vector3.Lerp(startPoint, endPoint, (float)i / (chainSegments.Length - 1));
            chainSegments[i].transform.position = targetPosition;
        }
    }

    // Visualize attack range in the Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Optional: visualize the swing radius for debugging
        if (otherPlayer != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, swingRadius);
        }
    }
}
