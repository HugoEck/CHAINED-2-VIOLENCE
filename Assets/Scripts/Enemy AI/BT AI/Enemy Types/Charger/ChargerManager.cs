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

    [Header("GE EJ VÄRDE")]

    [HideInInspector] public bool hasAlreadyCharged = false;
    [HideInInspector] public bool isAlreadyCharging = false;
    [HideInInspector] public bool activatePrepareChargeTimer = false;
    [HideInInspector] public bool activateChargingTimer = false;
    [HideInInspector] public bool prepareChargeComplete = false;
    [HideInInspector] public Vector3 chainPosition;
    [HideInInspector] public Vector3 lastSavedPosition;

    void Start()
    {
        enemyID = "Charger";
        
        animator.SetBool("Charger_StartChasing", true);

        LoadStats();
        
        ConstructBT();


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
        currentHealth = maxHealth;
        navigation.maxSpeed = speed;
        navigation.radius = 0.75f;
        c_collider.center = new Vector3(0, 0.5f, 0);
        c_collider.radius = 0.75f;
        c_collider.height = 3;

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
        chargeTimer -= Time.deltaTime;
        if (chargeTimer < 0)
        {
            hasAlreadyCharged = true;
            activateChargingTimer = false;
            
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
