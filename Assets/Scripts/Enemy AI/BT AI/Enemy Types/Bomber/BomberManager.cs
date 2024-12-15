using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BomberManager : BaseManager
{
    Node rootNode;

    Transform rootObject;
    [HideInInspector] public SphereCollider explosionRadius;

    [HideInInspector] public bool bombActivated = false;
    [HideInInspector] public bool bombExploded = false;
    //[HideInInspector] public bool bombTimerActive = false;
    [HideInInspector] public bool idleActive = false;
    [HideInInspector] public float sprintSpeed;

    public float bombAnimationTimer = 7;
    


    void Start()
    {
        enemyID = "Bomber";
        animator.SetBool("Bomber_StartChasing", true);
        rootObject = transform.Find("Root");
        explosionRadius = rootObject.AddComponent<SphereCollider>();
        explosionRadius.radius = 5f;
        explosionRadius.isTrigger = true;
        LoadStats();
        ConstructBT();
    }

    private void LoadStats()
    {
        maxHealth = 10 + maxHealthModifier;
        currentHealth = maxHealth;
        attack = 50 + attackModifier;
        defense = 0 + defenseModifier;
        navigation.maxSpeed = 4;
        attackRange = 20f;
        unitCost = 10;
        sprintSpeed = 10;
    }

    private void Update()
    {

        rootNode.Evaluate(this);

        if (bombActivated)
        {
            BombTimer();
        }

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


        rootNode = new Selector(new List<Node>() { bombBranch, bombSprint, chase });
    }
}
