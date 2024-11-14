using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SwordsmanManager : BaseManager
{

    Node rootNode;

    private float evaluationInterval = 0.5f;
    private float timeSinceLastEvaluation = 0f;
    private float randomOffset;




    void Start()
    {
        enemyID = "Swordsman";
        animator.SetBool("Swordsman_StartChasing", true);

        LoadStats();

        ConstructBT();
        animator.applyRootMotion = false;
        randomOffset = Random.Range(0f, evaluationInterval);
    }

    private void FixedUpdate()
    {

        FixedEvaluate();
        //rootNode.Evaluate(this);

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

    private void LoadStats()
    {
        maxHealth = 10;
        currentHealth = maxHealth;
        attack = 15;
        defense = 0;
        navigation.maxSpeed = 6;
        attackSpeed = 1;
        attackRange = 4;
        rb.mass = 10;
        unitCost = 10;
        transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);

    }

}
