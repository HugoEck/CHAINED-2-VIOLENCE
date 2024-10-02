using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoblinManager : BaseManager
{
    //[HideInInspector]

    //public NavMeshAgent navMeshAgent;
    Node rootNode;
    Material material;

    //-------------------------------------------------------------------------------- DEBUG för fps

    private float evaluationInterval = 1.0f; // Time interval in seconds
    private float timeSinceLastEvaluation = 0f; // Timer to track elapsed time
    private float randomOffset; // Random offset to stagger instances

    void Start()
    {
        currentHealth = maxHealth;
        navMeshAgent.speed = speed;
        ConstructBT();

        // Randomize the initial offset for each instance to avoid synchronization
        randomOffset = Random.Range(0f, evaluationInterval);
    }

    private void FixedUpdate()
    {
        // Update the timer with the time elapsed since last frame
        timeSinceLastEvaluation += Time.fixedDeltaTime;

        // Check if the timer has passed the evaluation interval + random offset
        if (timeSinceLastEvaluation >= evaluationInterval + randomOffset)
        {
            // Reset the timer, keeping any leftover time
            timeSinceLastEvaluation -= evaluationInterval;

            // Evaluate the behavior tree
            rootNode.Evaluate(this);

            // Randomize the offset again for the next cycle to ensure continued staggering
            randomOffset = Random.Range(0f, evaluationInterval);
        }
    }

    //-------------------------------------------------------------------------------------------

    //void Start()
    //{

    //    currentHealth = maxHealth;
    //    navMeshAgent.speed = speed;
    //    ConstructBT();
    //}

    //private void FixedUpdate()
    //{
    //    rootNode.Evaluate(this);
    //}
    private void Update()
    {
        
        if (rootNode.nodeState == NodeState.FAILURE)
        {
            SetColor(Color.red);
        }



        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentHealth--;
        }
    }
  

    private void ConstructBT()
    {
        CheckIfDead checkIfDead = new CheckIfDead();
        KillAgent killAgent = new KillAgent();
        PlayerInRange playerInRange = new PlayerInRange();
        AttackPlayer attackPlayer = new AttackPlayer();
        ChasePlayer chasePlayer = new ChasePlayer();

        Sequence isDead= new Sequence(new List<Node> { checkIfDead, killAgent});
        Sequence attack = new Sequence(new List<Node> { playerInRange, attackPlayer });

        rootNode = new Selector(new List<Node>() { isDead, attack, chasePlayer });

        
    }

    public void SetColor(Color color)
    {
        material.color = color;
    }

}
