using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BomberManager : BaseManager
{
    Node rootNode;

    public GameObject explosionParticle;

    Transform rootObject;
    [HideInInspector] public SphereCollider explosionCollider;

    [HideInInspector] public bool bombActivated = false;
    [HideInInspector] public bool bombExploded = false;
    //[HideInInspector] public bool bombTimerActive = false;
    [HideInInspector] public bool idleActive = false;
    [HideInInspector] public float sprintSpeed;
    [HideInInspector] public bool deathAfterActivation = false;
    [HideInInspector] public bool deathBeforeActivation = false;


    [HideInInspector] public float bombAnimationTimer = 7;
    float explosionRadius = 7f;

    private LayerMask explosionLayers;
    private float explosionForce = 75;

    void Start()
    {
        enemyID = "Bomber";
        animator.SetBool("Bomber_StartChasing", true);
        rootObject = transform.Find("Root");
        explosionCollider = rootObject.AddComponent<SphereCollider>();
        explosionCollider.radius = 5f;
        explosionCollider.isTrigger = true;
        explosionLayers = LayerMask.GetMask("Player", "Enemy");


        LoadStats();
        ConstructBT();
    }

    private void LoadStats()
    {
        maxHealth = 1 + maxHealthModifier;
        currentHealth = maxHealth;
        attack = 50 + attackModifier;
        defense = 0 + defenseModifier;
        navigation.maxSpeed = 3;
        attackRange = 10f;
        unitCost = 15;
        sprintSpeed = 7;
    }

    private void Update()
    {

        rootNode.Evaluate(this);

        if (bombActivated)
        {
            BombTimer();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            chainEffects.ActivateGhostChainEffect(3);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            chainEffects.ActivateShockChainEffect(3);
        }

    }

    public void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, explosionLayers);

        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            BaseManager baseManager = collider.GetComponent<BaseManager>();
            Player player = collider.GetComponent<Player>();
            
            if (baseManager != null)
            {
                baseManager.DealDamageToEnemy(attack, BaseManager.DamageType.ExplosionDamage);

                if (SceneManager.GetActiveScene().name != "SamTestScene")
                {
                    baseManager.chainEffects.ActivateRagdollStun(3, gameObject, 300);
                }
                else
                {
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 1, ForceMode.Impulse);
                }

            }
            if (player != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 1, ForceMode.Impulse);
                player.SetHealth(attack);

            }
       }

    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    private void BombTimer()
    {
        bombAnimationTimer -= Time.deltaTime;

        if (bombAnimationTimer + 0.2f < 5)
        {
            idleActive = false;
        }


        if ( bombAnimationTimer + 0.2f < 0 )
        {
            bombExploded = true;
        }
    }

    private void ConstructBT()
    {
        //CHASE NORMALLY BRANCH
        ChasePlayer chasePlayer = new ChasePlayer();
        BChaseConditions bChaseConditions = new BChaseConditions();
        Sequence chase = new Sequence(new List<Node>() { bChaseConditions, chasePlayer });

        //SUICIDE CHARGE BRANCH
        BChargeConditions b_ChargeConditions = new BChargeConditions();
        SuicideCharge suicideCharge = new SuicideCharge();
        Sequence bombSprint = new Sequence(new List<Node>() { b_ChargeConditions, suicideCharge  });

        //IDLE BRANCH
        ActivateBombConditions activateBombConditions = new ActivateBombConditions();
        ActivateBomb activateBomb = new ActivateBomb();
        Sequence bombBranch = new Sequence(new List<Node>() { activateBombConditions, activateBomb });

        //STUN BRANCH
        IsAgentStunned isAgentStunned = new IsAgentStunned();
        StunAgent stunAgent = new StunAgent();
        Sequence isStunned = new Sequence(new List<Node>() { isAgentStunned, stunAgent });

        //KILL BRANCH

        BKillConditions b_KillConditions = new BKillConditions();
        ExplosionConditions explosionConditions = new ExplosionConditions();
        ExplodeAgent explode = new ExplodeAgent();
        BKillNormalConditions killNormalConditions = new BKillNormalConditions();
        KillAgent killAgent = new KillAgent();
        Sequence explosion = new Sequence(new List<Node>() { explosionConditions, explode });
        Sequence normalDeath = new Sequence(new List<Node>() { killNormalConditions, killAgent });

        Selector killPath = new Selector(new List<Node>() { explosion, normalDeath });
        Sequence kill = new Sequence(new List<Node>() { b_KillConditions, killPath });


        rootNode = new Selector(new List<Node>() { kill, isStunned, bombBranch, bombSprint, chase });
    }
}
