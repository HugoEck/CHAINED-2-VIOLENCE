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
        //rootNode.Evaluate(this);
    }

    private void LoadStats()
    {
        maxHealth = 1;
        currentHealth = maxHealth;
        attack = 2;
        defense = 0;
        navigation.maxSpeed = 9;
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
        IsAgentStunned isAgentStunned = new IsAgentStunned();
        StunAgent stunAgent = new StunAgent();

        Sequence isDead = new Sequence(new List<Node> { checkIfDead, killAgent });
        Sequence isStunned = new Sequence(new List<Node>() { isAgentStunned, stunAgent });
        Sequence attack = new Sequence(new List<Node> { playerInRange, attackPlayer });

        rootNode = new Selector(new List<Node>() { isDead, isStunned, attack, chasePlayer });


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
