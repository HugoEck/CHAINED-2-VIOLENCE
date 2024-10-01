using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class RunnerManager : BaseManager
{

    Node rootNode;
    Material material;

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
        AttackPlayer attackPlayer = new AttackPlayer();
        ChasePlayer chasePlayer = new ChasePlayer();

        Sequence isDead = new Sequence(new List<Node> { checkIfDead, killAgent });
        Sequence attack = new Sequence(new List<Node> { playerInRange, attackPlayer });

        rootNode = new Selector(new List<Node>() { isDead, attack, chasePlayer });


    }

    public void SetColor(Color color)
    {
        material.color = color;
    }

}
