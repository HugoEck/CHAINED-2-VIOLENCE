using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoblinManager : BaseManager
{
    //[HideInInspector]

    public NavMeshAgent navMeshAgent;
    Node rootNode;
    Material material;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        material = GetComponent<MeshRenderer>().material;
    }

    void Start()
    {

        currentHealth = maxHealth;
        navMeshAgent.speed = speed;
        ConstructBT();
    }

    private void Update()
    {
        rootNode.Evaluate(this);
        if (rootNode.nodeState == NodeState.FAILURE)
        {
            SetColor(Color.red);
        }



        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentHealth--;
        }
    }

    private void ConstructBT()
    {
        CheckIfDead checkIfDead = new CheckIfDead(this);
        KillAgent killAgent = new KillAgent(this);
        PlayerInRange playerInRange = new PlayerInRange(this);
        AttackPlayer attackPlayer = new AttackPlayer(this);
        ChasePlayer chasePlayer = new ChasePlayer(this, navMeshAgent);

        Sequence isDead= new Sequence(new List<Node> { checkIfDead, killAgent});
        Sequence attack = new Sequence(new List<Node> { playerInRange, attackPlayer });

        rootNode = new Selector(new List<Node>() { isDead, attack });

        
    }

    public void SetColor(Color color)
    {
        material.color = color;
    }
    
}
