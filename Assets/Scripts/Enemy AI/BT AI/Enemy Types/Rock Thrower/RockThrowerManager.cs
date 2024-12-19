using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RockThrowerManager : BaseManager
{    
    Node rootNode;

    [Header("ROCKTHROWER MANAGER")]
    public GameObject rockPrefab;
    public Transform throwPoint;

    [Header("GE EJ VÄRDE")]
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
        maxHealth = 10 + maxHealthModifier;
        currentHealth = maxHealth;
        attack = 15 + attackModifier;
        defense = 0 + defenseModifier;
        navigation.maxSpeed = 4;
        attackSpeed = 4 + attackSpeedModifier;
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
        IsAgentStunned isAgentStunned = new IsAgentStunned();
        StunAgent stunAgent = new StunAgent();

        Sequence attack = new Sequence(new List<Node> { playerInRange, calculateThrow, throwRock });
        Sequence isStunned = new Sequence(new List<Node>() { isAgentStunned, stunAgent });
        Sequence isDead = new Sequence(new List<Node> { checkIfDead, killAgent });

        rootNode = new Selector(new List<Node>() { isDead, isStunned, attack, chasePlayer });
    }

    public void SetCalculatedVelocity(Vector3 newVelocity)
    {
        calculatedVelocity = newVelocity;
    }

}
