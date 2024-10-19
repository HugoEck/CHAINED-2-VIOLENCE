using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChargerManager : BaseManager
{

    Node rootNode;

    private float evaluationInterval = 0.5f;
    private float timeSinceLastEvaluation = 0f;
    private float randomOffset;

    [Header("CHARGER MANANAGER")]

    public float chargingRange;

    [Header("GE EJ VÄRDE")]

    public bool hasAlreadyCharged = false;

    void Start()
    {
        enemyID = "Charger";
        //animator.SetBool("Plebian_StartChasing", true);
        currentHealth = maxHealth;
        navigation.maxSpeed = speed;
        ConstructBT();

        randomOffset = Random.Range(0f, evaluationInterval);
    }

    private void FixedUpdate()
    {

        FixedEvaluate();

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
