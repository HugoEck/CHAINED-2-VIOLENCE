using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RockThrowerManager : BaseManager
{    
    Node rootNode;

    [Header("ROCKTHROWER MANAGER")]
    [HideInInspector] public GameObject rockPrefab;
    [HideInInspector] public Transform throwPoint;

    [Header("GE EJ V�RDE")]
    [HideInInspector]  public Vector3 calculatedVelocity;




    void Start()
    {
        enemyID = "RockThrower";
        animator.SetBool("RockThrower_StartChasing", true);

        LoadStats();

        ConstructBT();
    }

    private void FixedUpdate()
    {
        rootNode.Evaluate(this);

    }

    private void LoadStats()
    {
        maxHealth = 10;
        currentHealth = maxHealth;
        attack = 4;
        defense = 0;
        navigation.maxSpeed = 3;
        attackSpeed = 4;
        attackRange = 20;
        unitCost = 5;

    }
    private void ConstructBT()
    {

        CheckIfDead checkIfDead = new CheckIfDead();
        KillAgent killAgent = new KillAgent();
        PlayerInRange playerInRange = new PlayerInRange();
        CalculateThrow calculateThrow = new CalculateThrow();
        ThrowRock throwRock = new ThrowRock();
        ChasePlayer chasePlayer = new ChasePlayer();

        Sequence attack = new Sequence(new List<Node> { playerInRange, calculateThrow, throwRock });
        Sequence isDead = new Sequence(new List<Node> { checkIfDead, killAgent });

        rootNode = new Selector(new List<Node>() { isDead, attack, chasePlayer });
    }

    public void SetCalculatedVelocity(Vector3 newVelocity)
    {
        calculatedVelocity = newVelocity;
    }

}
