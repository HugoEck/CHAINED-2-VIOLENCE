using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class TrapManager : MonoBehaviour
{
    #region Variables
    private float surfaceY = -0.3f;
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

    #endregion

    void Start()
    {
        // Start the trap below the surface
        transform.position = new Vector3(transform.position.x, spawnY, transform.position.z);
        timer = Random.Range(minWaitTime, maxWaitTime);
    }

    void Update()
    {
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
    }

    #region Moving up/down
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
                // Rising towards the surface
                if (transform.position.y < surfaceY)
                {
                    transform.position += Vector3.up * riseSpeed * Time.deltaTime;
                }
                else
                {
                    // Once above the surface, wait for a random time
                    isMovingUp = false;
                    waitingAbove = true;
                    timer = Random.Range(minWaitTime, maxWaitTime);
                }
            }
            else
            {
                // Sinking back to the spawn position
                if (transform.position.y > spawnY)
                {
                    transform.position += Vector3.down * riseSpeed * Time.deltaTime;
                }
                else
                {
                    // Once below the surface, wait for a random time
                    isMovingUp = true;
                    waitingBelow = true;
                    timer = Random.Range(minWaitTime, maxWaitTime);
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
    