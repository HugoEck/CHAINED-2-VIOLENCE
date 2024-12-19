using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ConeAbility : MonoBehaviour, IAbility
{
    //[Header("Cone Ability Sound: ")]
    //[SerializeField] private AudioClip coneAbilitySound;

    //[Header("Ability Ready Sound: ")]
    //[SerializeField] private AudioClip abilityReadySound;

    private AudioClipManager audioClipManager;

    [Header("Ability")]
    [SerializeField] private float _damageCooldown = 0.2f;
    [SerializeField] private float _coneEffectCooldown = 1.5f;

    public float coneRange = 7f;
    public float coneAngle = 160f;
    public float baseConeDamage = 50f;
    private float coneDamage;
    public GameObject coneEffectPrefab;
    public Transform coneAnchor;           // Anchor object for the effect to follow
    public PlayerAttributes playerAttributes;

    public float cooldown = 5f;            // Cooldown duration in seconds
    private float lastUseTime = -Mathf.Infinity;  // Time when the ability was last used

    private HashSet<Collider> hitEnemiesOnce;

    private AnimationStateController animationStateController;

    private GameObject _coneEffect;

    private float _coneEffectTimer;

    private float _damageTimer;

    private bool _bHasConeAbilityBeenUsed = false;

    private bool abilityReadySoundPlayed = false;

    private int _playerId;

    private void Start()
    {
        hitEnemiesOnce = new HashSet<Collider>();
        animationStateController = GetComponent<AnimationStateController>();
        audioClipManager = FindObjectOfType<AudioClipManager>();
    }

    public void UseAbility(int playerId)
    {
        _playerId = playerId;
        // Check if the cooldown has elapsed
        if (Time.time >= lastUseTime + cooldown)
        {
            if (_playerId == 1)
            {
                Player1ComboManager.instance.currentAnimator.SetBool("UseAbility", true);
            }
            else if (_playerId == 2)
            {
                Player2ComboManager.instance.currentAnimator.SetBool("UseAbility", true);
            }

            _bHasConeAbilityBeenUsed = true;
            abilityReadySoundPlayed = false; // Reset the sound flag

            //ActivateConeAbility();
            lastUseTime = Time.time; // Update the last use time
        }
        else
        {
            Debug.Log("Cone ability is on cooldown.");
        }
    }
    private void Update()
    {
        if (_bHasConeAbilityBeenUsed)
        {
            _coneEffectTimer -= Time.deltaTime;
            _damageTimer -= Time.deltaTime;
            ActivateConeAbility();
            if (_coneEffectTimer < 0)
            {
                _bHasConeAbilityBeenUsed = false;
                animationStateController._animator.SetBool("WarriorAbilityOver", true);
                _bHasEffectBeenSpawned = false;
                Destroy(_coneEffect);

            }
        }

        if (_damageTimer <= 0 && !abilityReadySoundPlayed)
        {
            SFXManager.instance.PlaySFXClip(audioClipManager.abilityReady, transform, 1f);
            abilityReadySoundPlayed = true;
        }

    }

    private bool _bHasEffectBeenSpawned = false;

    void ActivateConeAbility()
    {
        coneDamage = baseConeDamage + playerAttributes.attackDamage;

        // Instantiate the visual effect at the anchor's position
        if (coneEffectPrefab != null && coneAnchor != null && !_bHasEffectBeenSpawned)
        {
            _coneEffectTimer = _coneEffectCooldown;
            _damageTimer = _damageCooldown;
            _bHasEffectBeenSpawned = true;

            _coneEffect = Instantiate(coneEffectPrefab, coneAnchor.position, Quaternion.identity);

            animationStateController._animator.SetBool("WarriorAbilityOver", false);

            // Make the effect follow the cone anchor
            _coneEffect.transform.SetParent(coneAnchor);

            // Align and scale the visual effect to match the cone
            _coneEffect.transform.localRotation = Quaternion.identity; // Keep aligned with the anchor's forward direction
            _coneEffect.transform.localScale = new Vector3(coneRange, coneRange, coneRange);

            //Destroy(coneEffect, 2.0f); // Destroy the effect after some time if it's temporary
        }

        if (_damageTimer <= 0)
        {
            // Find all enemies within the range
            Collider[] hitEnemies = Physics.OverlapSphere(transform.position, coneRange);

            foreach (Collider enemy in hitEnemies)
            {
                // Check if enemy has been hit
                if (hitEnemiesOnce.Contains(enemy)) continue;

                hitEnemiesOnce.Add(enemy);

                Vector3 directionToEnemy = (enemy.transform.position - transform.position).normalized;
                float angleToEnemy = Vector3.Angle(transform.forward, directionToEnemy);

                // Check if the enemy is within the cone's angle
                if (angleToEnemy <= coneAngle / 2)
                {
                    BaseManager enemyManager = enemy.GetComponent<BaseManager>();
                    if (enemyManager != null)
                    {
                        if(_playerId == 1)
                        {
                            enemyManager.DealDamageToEnemy(coneDamage, BaseManager.DamageType.AbilityDamage, true, false);
                            SFXManager.instance.PlaySFXClip(audioClipManager.coneAbility, transform, 1f);
                        }
                        else if(_playerId == 2)
                        {
                            enemyManager.DealDamageToEnemy(coneDamage, BaseManager.DamageType.AbilityDamage, false, true);
                            SFXManager.instance.PlaySFXClip(audioClipManager.coneAbility, transform, 1f);
                        }
                        
                        Debug.Log("Cone hit enemy: " + enemy.name);
                        // enemyManager.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 150, ForceMode.Force);
                    }
                }
            }
            _damageTimer = _damageCooldown;
        }

        Debug.Log("Cone Ability triggered.");
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, coneRange);

        Vector3 forwardDirection = transform.forward * coneRange;

        Quaternion leftRotation = Quaternion.AngleAxis(-coneAngle / 2, Vector3.up);
        Vector3 leftEdge = leftRotation * forwardDirection;

        Quaternion rightRotation = Quaternion.AngleAxis(coneAngle / 2, Vector3.up);
        Vector3 rightEdge = rightRotation * forwardDirection;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + leftEdge);
        Gizmos.DrawLine(transform.position, transform.position + rightEdge);

        Gizmos.color = Color.yellow;
        for (float i = -coneAngle / 2; i <= coneAngle / 2; i += coneAngle / 10)
        {
            Quaternion stepRotation = Quaternion.AngleAxis(i, Vector3.up);
            Vector3 edge = stepRotation * forwardDirection;
            Gizmos.DrawLine(transform.position, transform.position + edge);
        }
    }
}
