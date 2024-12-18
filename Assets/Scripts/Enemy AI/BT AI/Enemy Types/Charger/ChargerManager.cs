using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChargerManager : BaseManager
{

    Node rootNode;

    private float chargeTimer = 3;
    
    [Header("CHARGER MANANAGER")]

    public float chargingRange;
    public float chargingSpeed;
    private float chargingDamage;
    private bool SprintDamageAllowed = false;
    private bool CD_AlreadyAppliedP1 = false;
    private bool CD_AlreadyAppliedP2 = false;

    [Header("GE EJ VÄRDE")]

    [HideInInspector] public bool hasAlreadyCharged = false;
    [HideInInspector] public bool isAlreadyCharging = false;
    [HideInInspector] public bool activatePrepareChargeTimer = false;
    [HideInInspector] public bool activateChargingTimer = false;
    [HideInInspector] public bool prepareChargeComplete = false;

  
    [HideInInspector] public Vector3 chainPosition;
    [HideInInspector] public Vector3 lastSavedPosition;

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

        //----------------PONTUS KOD: PARTICLE & DESTRUCTION OF OBJECTS----------------\\
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

    private void FixedUpdate()
    {
        
        rootNode.Evaluate(this);

        if (activatePrepareChargeTimer)
        {
            PrepareChargeTimer();
        }

        if (activateChargingTimer)
        {
            ChargingTimer();
        }

    }

    private void LoadStats()
    {
        
        chargingDamage = 25f;
        navigation.maxSpeed = 4;
        chargingRange = 15f;
        chargingSpeed = 20f;
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
        chargeTimer -= Time.deltaTime;
        if (chargeTimer < 0)
        {
            prepareChargeComplete = true;
            activatePrepareChargeTimer = false;
            chargeTimer = 3;
        }
    }

    public void ChargingTimer()
    {
        SprintDamageAllowed = true;
        chargeTimer -= Time.deltaTime;
        if (chargeTimer < 0)
        {
            SprintDamageAllowed = false;
            hasAlreadyCharged = true;
            activateChargingTimer = false;
            
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (SprintDamageAllowed && collision.gameObject.CompareTag("Player1") && CD_AlreadyAppliedP1 == false )
        {
            //Debug.Log("P1 took Damage!");
            CD_AlreadyAppliedP1 = true;
            playerManager1.SetHealth(chargingDamage);

        }
        else if (SprintDamageAllowed && collision.gameObject.CompareTag("Player2") && CD_AlreadyAppliedP2 == false )
        {
            //Debug.Log("P2 took Damage!");
            CD_AlreadyAppliedP2 = true;
            playerManager2.SetHealth(chargingDamage);

        }
        else if (SprintDamageAllowed && collision.gameObject.CompareTag("Misc") || collision.gameObject.CompareTag("Obstacles"))
        {
            //----------------PONTUS KOD: PARTICLE & DESTRUCTION OF OBJECTS----------------\\
            if (collision.contacts.Length > 0 && destructionParticle != null)
            {
                Vector3 collisionPoint = collision.contacts[0].point;
                collisionPoint.y = 0;

                GameObject particleInstance = Instantiate(destructionParticle, collisionPoint, Quaternion.identity);
                Destroy(particleInstance, 2f);
            }
            else
            {
                Debug.Log("Particles prefab is not assigned or no collision contacts available.");
            }

            if (spawner != null)
            {
                spawner.RemoveObjectFromCollision(collision.gameObject);
            }

            Destroy(collision.gameObject);
        }
    }
    private void ConstructBT()
    {
        CheckIfDead checkIfDead = new CheckIfDead();
        KillAgent killAgent = new KillAgent();
        NotAlreadyCharged notAlreadyCharged = new NotAlreadyCharged();
        MoveToChargeRange moveToChargeRange= new MoveToChargeRange();
        ChargeComplete chargeComplete = new ChargeComplete();
        IsNotInRange isNotInRange = new IsNotInRange();
        ChasePlayer chasePlayer = new ChasePlayer();
        PrepareCharge prepareCharge = new PrepareCharge();
        ChargeToChain chargeToChain = new ChargeToChain();
        PlayerInRange playerInRange = new PlayerInRange();
        AttackPlayer attackPlayer = new AttackPlayer();

        //Branch 1
        Sequence isDead = new Sequence(new List<Node> { checkIfDead, killAgent });

        //Branch 2
        Sequence walkingToCharge = new Sequence(new List<Node> { notAlreadyCharged, moveToChargeRange });
        Sequence walkingToPlayer = new Sequence(new List<Node> { chargeComplete, isNotInRange, chasePlayer });
        Selector walking = new Selector(new List<Node> { walkingToCharge, walkingToPlayer });

        //Branch 3
        Sequence charging = new Sequence(new List<Node> { notAlreadyCharged, prepareCharge, chargeToChain });

        //Branch 4
        Sequence attack = new Sequence(new List<Node> { playerInRange, attackPlayer });

        rootNode = new Selector(new List<Node>() { isDead, walking, charging, attack });
    }

    


}
