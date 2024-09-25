using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GoblinManager : BaseManager
{

    Node rootNode;
    int currentFrame = 0;
    int nrFrameSkips = 1;

    void Start()
    {
        ConstructBT();
    }

    private void Update()
    {
        if (currentFrame == nrFrameSkips)
        {
            rootNode.Evaluate(this);
        }
        else
        {
            currentFrame++;
        }

        if (Input.GetKeyDown(KeyCode.K))
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

   

}
