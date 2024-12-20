using HighlightPlus;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ProjectileBehavior : MonoBehaviour
{
    [Header("Grenade explosion")]
    [SerializeField] private float timeForGrenadeToExplode;
    [SerializeField] private ParticleSystem explosionEffect;
    [SerializeField] private HighlightEffect grenadeHighlight;

    public float explosionRadius = 5f;
    public float baseExplosionDamage = 50f;
    public float explosionDamage;
    public LayerMask enemyLayer;
    public GameObject grenadeObject;

    private HashSet<Collider> hitEnemiesOnce;
    public PlayerAttributes playerAttributes;
    private AudioClipManager audioClipManager;
    private bool hasExploded = false;

    private Rigidbody rb;
    [SerializeField] private MeshRenderer grenadeRenderer;

    private float explosionTimer;

    private int _playerId;
    private void Start()
    {
        hitEnemiesOnce = new HashSet<Collider>();
        audioClipManager = FindObjectOfType<AudioClipManager>();
        rb = GetComponent<Rigidbody>();
        explosionTimer = timeForGrenadeToExplode;
    }

    private bool addHighlight = false;
    private void Update()
    {
        explosionTimer -= Time.deltaTime;

        // Normalize the timer to a value between 0 and 1
        float progress = 1f - Mathf.Clamp01(explosionTimer / timeForGrenadeToExplode);


        grenadeHighlight.innerGlow = Mathf.Lerp(0.06f, 5, progress);
        

        if (explosionTimer <= 0 && !hasExploded)
        {
            Explode();
            explosionEffect.Play();
            hasExploded = true;


            grenadeObject.SetActive(false);
            
            
        }

        if (hasExploded && !explosionEffect.isPlaying)
        {
            Destroy(gameObject);
        }
    }
    private IEnumerator BlinkGrenade(float time)
    {
        yield return new WaitForSeconds(time);
        grenadeHighlight.HitFX();
    }

    // Method to handle the explosion and damage enemies in the radius
    void Explode()
    {
        _playerId = playerAttributes._playerId;

        explosionDamage = baseExplosionDamage + playerAttributes.attackDamage;

        if (audioClipManager.rangeAbilityExplosion != null)
        {
        SFXManager.instance.PlaySFXClip(audioClipManager.rangeAbilityExplosion, transform, 1f);
        }

        Debug.Log("Projectile exploded!");

        // Find all colliders within the explosion radius that are on the enemy layer
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            // Check if enemy has been hit
            if (hitEnemiesOnce.Contains(enemy)) continue;

            hitEnemiesOnce.Add(enemy);

            // Apply damage to each enemy
            BaseManager enemyManager = enemy.GetComponent<BaseManager>();
            if (enemyManager != null)
            {
                if(_playerId == 1)
                {
                    enemyManager.DealDamageToEnemy(explosionDamage, BaseManager.DamageType.AbilityDamage, true, false);
                }
                else if(_playerId == 2)
                {
                    enemyManager.DealDamageToEnemy(explosionDamage, BaseManager.DamageType.AbilityDamage, false, true);
                }
                
                Debug.Log("Damaged enemy: " + enemy.name + explosionDamage);


                //Enemy ragdoll
                enemyManager.chainEffects.ActivateRagdollStun(4f, this.gameObject, 500);
            }
        }
    }
}
