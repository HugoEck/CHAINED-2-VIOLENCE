using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RockThrowerManager : BaseManager
{    
    Node rootNode;
    Material material;

    [Header("ROCKTHROWER MANAGER")]
    public GameObject rockPrefab;
    public Transform throwPoint;

    [Header("GE EJ VÄRDE")]
    public Vector3 calculatedVelocity;

    void Start()
    {
        currentHealth = maxHealth;
        navMeshAgent.speed = speed;
        ConstructBT();
    }

    private void FixedUpdate()
    {
        rootNode.Evaluate(this);

        if (rootNode.nodeState == NodeState.FAILURE)
        {
            SetColor(Color.red);
        }

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

    public void SetColor(Color color)
    {
        material.color = color;
    }
}
