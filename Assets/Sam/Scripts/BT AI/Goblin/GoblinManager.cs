using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GoblinManager : BaseManager
{
    //[HideInInspector]

    //public NavMeshAgent navMeshAgent;
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
    }
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
