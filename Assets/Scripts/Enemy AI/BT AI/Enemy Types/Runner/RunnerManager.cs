using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunnerManager : BaseManager
{

    Node rootNode;
    

    private float evaluationInterval = 0.5f;
    private float timeSinceLastEvaluation = 0f;
    private float randomOffset;


    void Start()
    {
        enemyID = "Runner";
        animator.SetBool("Runner_StartChasing", true);

        LoadStats();
        ConstructBT();
    }

    private void FixedUpdate()
    {
        FixedEvaluate();

    }

    private void LoadStats()
    {
        maxHealth = 1;
        currentHealth = maxHealth;
        attack = 2;
        defense = 0;
        navigation.maxSpeed = 6;
        attackSpeed = 1;
        attackRange = 3f;
        unitCost = 2;

    }

    private void ConstructBT()
    {
        CheckIfDead checkIfDead = new CheckIfDead();
        KillAgent killAgent = new KillAgent();
        PlayerInRange playerInRange = new PlayerInRange();
        AttackPlayer attackPlayer = new AttackPlayer();
        ChasePlayer chasePlayer = new ChasePlayer();

        Sequence isDead = new Sequence(new List<Node> { checkIfDead, killAgent });
        Sequence attack = new Sequence(new List<Node> { playerInRange, attackPlayer });

        rootNode = new Selector(new List<Node>() { isDead, attack, chasePlayer });


    }

    public void FixedEvaluate()
    {
        timeSinceLastEvaluation += Time.fixedDeltaTime;

        if (timeSinceLastEvaluation >= evaluationInterval + randomOffset)
        {
            timeSinceLastEvaluation -= evaluationInterval;
            rootNode.Evaluate(this);
            randomOffset = Random.Range(0f, evaluationInterval);
        }
    }
}
