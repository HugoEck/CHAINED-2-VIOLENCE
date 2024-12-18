using UnityEditor;
using UnityEngine;

public class Projectile : MonoBehaviour, IAbility
{
    [Header("Range Ability Sound: ")]
    [SerializeField] private AudioClip rangeAbilitySound;

    [Header("Range Ability Sound: ")]
    [SerializeField] private AudioClip abilityReadySound;
    public PlayerAttributes playerAttributes;

    public GameObject projectilePrefab;
    public Transform firePoint;
    public float cooldown;
    private float lastShootTime = -Mathf.Infinity;
    [SerializeField] private float projectileSpeed = 3f;
    private float projectileLifeTime = 5f;

    public static bool player1ThrowGrenade = false;
    public static bool player2ThrowGrenade = false;

    private bool bHasUsedAbility = false;
    private int _playerId;

    private bool abilityReadySoundPlayed = false;

    private void Update()
    {
        if(bHasUsedAbility)
        {
            if(_playerId == 1)
            {
                if(player1ThrowGrenade)
                {
                    player1ThrowGrenade = false;
                    Shoot();
                    lastShootTime = Time.time;

                }
            }
            else if(_playerId == 2)
            {
                if (player2ThrowGrenade)
                {
                    player2ThrowGrenade = false;
                    Shoot();
                    lastShootTime = Time.time;
                }
            }
        }

        if(cooldown <= 0 && !abilityReadySoundPlayed)
        {
            SFXManager.instance.PlaySFXClip(abilityReadySound, transform, 1f);
            abilityReadySoundPlayed |= true;
        }
    }
    
    public void UseAbility(int playerId)
    {
        if (Time.time >= lastShootTime + cooldown)
        {
            _playerId = playerId;

            if (playerId == 1)
            {
                Player1ComboManager.instance.currentAnimator.SetBool("UseAbility", true);
            }
            else if (playerId == 2)
            {
                Player2ComboManager.instance.currentAnimator.SetBool("UseAbility", true);
            }

            bHasUsedAbility = true;
            abilityReadySoundPlayed = false;
            
        }
        else
        {
            Debug.Log("Projectile ability is on cooldown.");
        }
    }

    void Shoot()
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("Projectile prefab is not assigned.");
            return;
        }

        if (firePoint == null)
        {
            Debug.LogError("Fire point is not assigned.");
            return;
        }

        SFXManager.instance.PlaySFXClip(rangeAbilitySound, transform, 1f);

        // Retrieve speed from the particle system.
        //ParticleSystem particleSystem = projectilePrefab.GetComponent<ParticleSystem>();
        //if (particleSystem != null)
        //{
        //    ParticleSystem.MainModule mainModule = particleSystem.main;
        //    //projectileSpeed = mainModule.startSpeed.constant;
        //}
        //else
        //{
        //    Debug.LogWarning("Projectile prefab is missing a ParticleSystem. Using default projectile speed.");
        //}

        Vector3 direction = transform.forward.normalized;
        //Debug.Log("Spawning projectile with direction: " + direction);

        // Instantiate the projectile
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position + direction * 3f, Quaternion.LookRotation(direction));
        ProjectileBehavior projectileBehavior = projectile.GetComponent<ProjectileBehavior>();
        projectileBehavior.playerAttributes = playerAttributes; // Assuming "this" is the PlayerCombat instance
                                                                
        // Trigger the AbilityCdEventsUI event for cooldown start
        lastShootTime = Time.time;
        AbilityCdEventsUI.AbilityUsed(_playerId, PlayerCombat.PlayerClass.Ranged, cooldown);

        if (projectile != null)
        {
            Debug.Log("Projectile spawned: " + projectile.name);
        }
        else
        {
            Debug.LogError("Failed to spawn projectile.");
            return;
        }

        projectile.layer = LayerMask.NameToLayer("Projectile");

        // Ignore collision between the player and the projectile
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Projectile"));

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * projectileSpeed;
        }
        else
        {
            Debug.LogError("Projectile is missing Rigidbody component.");
        }

        //Destroy(projectile, projectileLifeTime);
    }
}
