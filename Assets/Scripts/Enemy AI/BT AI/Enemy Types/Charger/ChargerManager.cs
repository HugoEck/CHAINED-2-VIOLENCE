using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChargerManager : BaseManager
{

    Node rootNode;

    [HideInInspector] public float prepareChargeTimer = 3;
    private float backupPrepareChargeTimer = 3.2f;
    [HideInInspector] public bool lockTimer = false;

    [HideInInspector] public float chargeRunTimer = 3;

    [Header("CHARGER MANANAGER")]

    public float chargingRange;
    public float chargingSpeed;
    private float chargingDamage;
    private bool sprintDamageAllowed = false;
    private bool CD_AlreadyAppliedP1 = false;
    private bool CD_AlreadyAppliedP2 = false;

    [Header("GE EJ VÄRDE")]

    [HideInInspector] public bool hasAlreadyCharged = false;
    [HideInInspector] public bool isAlreadyCharging = false;
    [HideInInspector] public bool activateChargingTimer = false;
    [HideInInspector] public bool prepareChargeComplete = false;


    [HideInInspector] public Vector3 chainPosition;
    [HideInInspector] public Vector3 lastSavedPosition;

    [HideInInspector] public bool chargeRunActive = false;
    [HideInInspector] public bool prepareChargeActive = false;
    [HideInInspector] public bool collidedWithWall = false;


    //----------------PONTUS KOD: PARTICLE & DESTRUCTION OF OBJECTS----------------\\
    [Header("DESTRUCTION OF OBJECTS & PARTICLE")]
    public itemAreaSpawner spawner;
    public GameObject destructionParticle;


    void Start()
    {
        enemyID = "Charger";

        animator.SetBool("Charger_StartChasing", true);

        LoadStats();

        ConstructBT();

        GetDestructionParticle();

    }

    private void FixedUpdate()
    {

        rootNode.Evaluate(this);

        if (prepareChargeActive)
        {
            PrepareChargeTimer();
        }

        if (chargeRunActive)
        {
            ChargingTimer();
        }

    }

    private void LoadStats()
    {

        chargingDamage = 25f;
        navigation.maxSpeed = 5;
        chargingRange = 20f;
        chargingSpeed = 25f;
        navigation.radius = 0.75f;
        c_collider.center = new Vector3(0, 0.5f, 0);
        c_collider.radius = 0.75f;
        c_collider.height = 3;
        transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        rb.mass = 50;
        maxHealth = 40 + maxHealthModifier;
        currentHealth = maxHealth;
        attack = 15 + attackModifier;
        defense = 2 + defenseModifier;
        attackSpeed = 3f + attackSpeedModifier;
        attackRange = 4f;
        unitCost = 30;

    }
    public void PrepareChargeTimer()
    {
        if (!lockTimer)
        {
            backupPrepareChargeTimer -= Time.deltaTime;
        }

        if (backupPrepareChargeTimer < 0)
        {
            lockTimer = true;
            prepareChargeTimer = 3;
            backupPrepareChargeTimer = 3.2f;
        }
    }

    public void ChargingTimer()
    {
        sprintDamageAllowed = true;
        chargeRunTimer -= Time.deltaTime;
        if (chargeRunTimer < 0)
        {
            sprintDamageAllowed = false;
            chargeRunActive = false;

        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (sprintDamageAllowed && collision.gameObject.CompareTag("Player1") && CD_AlreadyAppliedP1 == false)
        {
            //Debug.Log("P1 took Damage!");
            CD_AlreadyAppliedP1 = true;
            playerManager1.SetHealth(chargingDamage);

        }
        else if (sprintDamageAllowed && collision.gameObject.CompareTag("Player2") && CD_AlreadyAppliedP2 == false)
        {
            //Debug.Log("P2 took Damage!");
            CD_AlreadyAppliedP2 = true;
            playerManager2.SetHealth(chargingDamage);

        }
        else if (sprintDamageAllowed && collision.gameObject.CompareTag("Misc"))
        {
            //----------------PONTUS KOD: PARTICLE & DESTRUCTION OF OBJECTS----------------\\
            // Handle destruction particle effect
            if (destructionParticle != null && collision.contacts.Length > 0)
            {
                Vector3 collisionPoint = collision.contacts[0].point;
                collisionPoint.y = 0; // Adjust Y position if needed

                GameObject particleInstance = Instantiate(destructionParticle, collisionPoint, Quaternion.identity);
                Destroy(particleInstance, 2f);
            }
            else
            {
                Debug.Log("Particles prefab is not assigned or no collision contacts available.");
            }

            // Update grid graph
            if (spawner != null && collision.gameObject != null)
            {
                Collider collider = collision.gameObject.GetComponent<Collider>();
                if (collider != null)
                {
                    Bounds bounds = collider.bounds;
                    float updateRadius = (Mathf.Max(bounds.size.x, bounds.size.z) / 2f) * spawner.navMeshOffsetMultiplier + 3f;
                    spawner.gridGraphUpdater.RemoveObstacleUpdate(collision.gameObject.transform.position, updateRadius);
                }

                spawner.RemoveObjectFromCollision(collision.gameObject); // Remove from spawner's list
            }

            Destroy(collision.gameObject); // Destroy the object
        }
        else if (sprintDamageAllowed && collision.gameObject.layer == LayerMask.NameToLayer("Map"))
        {
            collidedWithWall = true;
        }
    }

    //----------------PONTUS KOD: PARTICLE & DESTRUCTION OF OBJECTS----------------\\
    private void GetDestructionParticle()
    {
        if (destructionParticle == null)
        {
            destructionParticle = Resources.Load<GameObject>("FX_GroundCrack_Blast_01");
            if (destructionParticle == null)
            {
                Debug.LogError("Destruction Particle not found in Resources! Check the file name and folder structure.");
            }
        }
        if (spawner == null)
        {
            spawner = FindObjectOfType<itemAreaSpawner>();
            if (spawner == null)
            {
                Debug.LogError("itemAreaSpawner not found! Ensure it exists in the scene.");
            }
        }
    }
    private void ConstructBT()
    {

        CheckIfDead checkIfDead = new CheckIfDead();
        KillAgent killAgent = new KillAgent();
        IsAgentStunned isAgentStunned = new IsAgentStunned();
        StunAgent stunAgent = new StunAgent();
        CChargeConditions c_ChargeConditions = new CChargeConditions();
        PrepareCharge prepareCharge = new PrepareCharge();
        ChargeToChain chargeToChain = new ChargeToChain();
        AttackConditions attackConditions = new AttackConditions();
        AttackPlayer attackPlayer = new AttackPlayer();
        ChasePlayer chasePlayer = new ChasePlayer();



        //KILL BRANCH
        Sequence isDead = new Sequence(new List<Node> { checkIfDead, killAgent });

        //STUN BRANCH

        Sequence stun = new Sequence(new List<Node> { isAgentStunned, stunAgent });

        //CHARGING BRANCH

        Sequence charging = new Sequence(new List<Node> { c_ChargeConditions, prepareCharge, chargeToChain });

        //ATTACK BRANCH

        Sequence attack = new Sequence(new List<Node> { attackConditions, attackPlayer });

        //CHASE BRANCH - 0


        rootNode = new Selector(new List<Node>() { isDead, stun, charging, attack, chasePlayer });
    }




}
