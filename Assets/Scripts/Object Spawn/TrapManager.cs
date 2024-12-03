using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.ParticleSystem;

public class TrapManager : MonoBehaviour
{
    #region Variables
    private float surfaceY = -0.3f;
    private float particleSurfacePosition = 0.2f;
    private float spawnY = -15f;
    public float riseSpeed = 15f;
    public float despawnSpeed = 10f;

    public float minWaitTime;
    public float maxWaitTime;
    private float timer = 0f;

    private bool isDespawning = false;
    private bool isMovingUp = false;
    private bool waitingAbove = false;
    private bool waitingBelow = false;

    public float trapDamage = 1f;

    [SerializeField] private GameObject riseParticle; // Particle prefab for the trap rising
    private GameObject currentParticle; // Reference to the active particle
    #endregion

    void Start()
    {
        // Start the trap below the surface
        transform.position = new Vector3(transform.position.x, spawnY, transform.position.z);
        timer = Random.Range(minWaitTime, maxWaitTime);
    }

    void Update()
    {
        if (!isDespawning)
        {
            Movement();
        }
        else
        {
            DespawnTrap();
        }
    }

    #region Moving up/down & Particle
    private void Movement()
    {
        if (!isDespawning)
        {
            if (waitingAbove || waitingBelow)
            {
                // Trap is waiting above or below, decrease the timer
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    waitingAbove = false;
                    waitingBelow = false;
                }
            }
            else if (isMovingUp)
            {
                // Rising towards the surface with acceleration
                if (transform.position.y < surfaceY)
                {
                    float acceleration = 5f; // Adjust this value for faster/slower acceleration
                    riseSpeed = Mathf.Min(riseSpeed + acceleration * Time.deltaTime, 15f); // Cap at max speed
                    transform.position += Vector3.up * riseSpeed * Time.deltaTime;

                    // Play the rise particle effect at the surface
                    if (currentParticle == null && riseParticle != null)
                    {
                        Vector3 particlePosition = new Vector3(transform.position.x, particleSurfacePosition, transform.position.z);
                        currentParticle = Instantiate(riseParticle, particlePosition, Quaternion.identity);
                    }
                }
                else
                {
                    // Once above the surface, reset riseSpeed and set a fixed wait time
                    riseSpeed = 0f; // Reset the rise speed
                    isMovingUp = false;
                    waitingAbove = true;
                    timer = 2f; // Fixed wait time at the surface

                    // Stop the rise particle effect
                    if (currentParticle != null)
                    {
                        Destroy(currentParticle);
                        currentParticle = null;
                    }
                }
            }
            else
            {
                // Sinking back to the spawn position with acceleration
                if (transform.position.y > spawnY)
                {
                    float acceleration = 5f; // Adjust this value for faster/slower deceleration
                    riseSpeed = Mathf.Min(riseSpeed + acceleration * Time.deltaTime, 15f); // Cap at max speed
                    transform.position += Vector3.down * riseSpeed * Time.deltaTime;
                }
                else
                {
                    // Once below the surface, reset riseSpeed and set a random wait time
                    riseSpeed = 0f; // Reset the rise speed
                    isMovingUp = true;
                    waitingBelow = true;
                    timer = Random.Range(10f, 20f); // Random wait time below the surface
                }
            }
        }
    }
    #endregion

    #region Despawn
    public void DespawnTrap()
    {
        isDespawning = true;
        isMovingUp = false; // Ensure the trap starts moving down immediately

        if (transform.position.y > spawnY)
        {
            transform.position += Vector3.down * despawnSpeed * Time.deltaTime;

            // Stop the rise particle effect if the trap is despawning
            if (currentParticle != null)
            {
                Destroy(currentParticle);
                currentParticle = null;
            }
        }
    }
    #endregion

    #region Deal Damage
    private void OnCollisionEnter(Collision collision)
    {
        // DAMAGE PLAYER
        if (collision.gameObject.CompareTag("Player1") || collision.gameObject.CompareTag("Player2"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player != null)
            {
                player.SetHealth(trapDamage);
            }
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            BaseManager enemy = collision.gameObject.GetComponent<BaseManager>();

            if (enemy != null)
            {
                enemy.DealDamageToEnemy(trapDamage, BaseManager.DamageType.TrapsDamage);
            }
        }
    }
    #endregion
}
