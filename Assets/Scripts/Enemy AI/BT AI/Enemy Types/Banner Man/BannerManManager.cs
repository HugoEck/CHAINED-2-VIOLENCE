using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BannerManManager : BaseManager
{
    Node rootNode;

    [Header("ROCKTHROWER MANAGER")]

    public float circleRadius;

    [HideInInspector] public bool hasAlreadyReachedPlayer = false;
    [HideInInspector] public bool isNewFlagReady = true;
    [HideInInspector] public bool isNewDestinationCalculated = false;
    [HideInInspector] public Vector3 newDestination;
    [HideInInspector] public Vector3 circleCenter;
    [HideInInspector] public Vector3 currentVector;


    public GameObject flagPrefab;



    void Start()
    {
        enemyID = "BannerMan";
        //animator.SetBool("Plebian_StartChasing", true);

        LoadStats();

        ConstructBT();

    }

    private void FixedUpdate()
    {

        rootNode.Evaluate(this);

    }

    private void ConstructBT()
    {
        CheckIfDead checkIfDead = new CheckIfDead();
        KillAgent killAgent = new KillAgent();
        PlayerInRange playerInRange = new PlayerInRange();
        AttackPlayer attackPlayer = new AttackPlayer();
        ChasePlayer chasePlayer = new ChasePlayer();
        IsAgentStunned isAgentStunned = new IsAgentStunned();
        StunAgent stunAgent = new StunAgent();

        Sequence isDead = new Sequence(new List<Node> { checkIfDead, killAgent });
        Sequence isStunned = new Sequence(new List<Node>() { isAgentStunned, stunAgent });
        Sequence attack = new Sequence(new List<Node> { playerInRange, attackPlayer });



        //rootNode = new Selector(new List<Node>() { isDead, isStunned, attack, chasePlayer });


    }


    private void LoadStats()
    {
        maxHealth = 50;
        currentHealth = maxHealth;
        attack = 0;
        defense = 0;
        navigation.maxSpeed = 5;
        attackSpeed = 0;
        attackRange = 40f;
        unitCost = 15;

    }
}
