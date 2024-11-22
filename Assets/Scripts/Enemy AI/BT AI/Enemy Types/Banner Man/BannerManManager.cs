using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BannerManManager : BaseManager
{
    Node rootNode;

    [Header("BANNERMAN MANAGER")]

    public float circleRadius;

    public bool hasAlreadyReachedPlayer = false;
    public bool isNewFlagReady = true;
    [HideInInspector] public bool isNewDestinationCalculated = false;
    public Vector3 newDestination;
    [HideInInspector] public Vector3 circleCenter;
    [HideInInspector] public Vector3 currentVector;
    [HideInInspector] public int nrFlagsLeft = 4;


    public GameObject flagPrefab;



    void Start()
    {
        enemyID = "BannerMan";
        animator.SetBool("BannerMan_StartChasing", true);

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
        IsAgentStunned isAgentStunned = new IsAgentStunned();
        StunAgent stunAgent = new StunAgent();
        IsPlayerReached isPlayerReached = new IsPlayerReached();
        WalkToPlayer walkToPlayer = new WalkToPlayer();
        IsFlagReady isFlagReady = new IsFlagReady();
        PlaceFlag placeFlag = new PlaceFlag();
        IsPositionCalculated isPositionCalculated = new IsPositionCalculated();
        CalculatePosition calculatePosition = new CalculatePosition();
        MoveToPosition moveToPosition = new MoveToPosition();

        //KILL BRANCH
        Sequence kill = new Sequence(new List<Node> { checkIfDead, killAgent });

        //STUN BRANCH
        Sequence stun = new Sequence(new List<Node> { isAgentStunned, stunAgent });

        //CHASE BRANCH
        Sequence chasePlayer = new Sequence(new List<Node> { isPlayerReached, walkToPlayer });

        //FLAG BRANCH
        Sequence flag = new Sequence(new List<Node> { isFlagReady, placeFlag });

        //RUNNING BRANCH

        Sequence conditions = new Sequence(new List<Node> { isPositionCalculated, calculatePosition });
        Selector movement = new Selector(new List<Node> { conditions, moveToPosition });

        rootNode = new Selector(new List<Node>() { kill, stun, chasePlayer, flag, movement});

    }

    private void LoadStats()
    {
        maxHealth = 50;
        currentHealth = maxHealth;
        attack = 0;
        defense = 0;
        navigation.maxSpeed = 6;
        attackSpeed = 0;
        attackRange = 30f;
        unitCost = 15;

    }
}
