using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyberGiantManager : BaseManager
{
    //[HideInInspector]

    Node rootNode;

    [Header("CYBERGIANT MANAGER")]
    public GameObject bombPrefab;
    public float minimumBombDistance;
    public float bombFireRate;
    public Transform bombShootPoint;
    public Transform missileShootPoint;
    public float bombDamage;




    [HideInInspector] public Vector3 bombVelocity;


    private float lastBombShotTime = 3;

    void Start()
    {
        enemyID = "CyberGiant";

        animator.SetBool("CyberGiant_Idle", true);

        LoadStats();
        ConstructBT();
        
    }

    private void FixedUpdate()
    {
        rootNode.Evaluate(this);
    }

    private void ConstructBT()
    {

        CheckIfDead checkIfDead = new CheckIfDead();
        KillAgent killAgent = new KillAgent();
        IsBombInRange isBombInRange = new IsBombInRange();
        CalculateBombPosition calculateBombPosition = new CalculateBombPosition();
        ShootBomb shootBomb = new ShootBomb();

        //Kill Branch
        Sequence isDead = new Sequence(new List<Node> { checkIfDead, killAgent });

        //Range Branch
        Sequence bomb = new Sequence(new List<Node> { isBombInRange, calculateBombPosition,  shootBomb });


        rootNode = new Selector(new List<Node>() { isDead, bomb});
    }

    private void LoadStats()
    {
        //bombDamage = 10;
        maxHealth = 100;
        currentHealth = maxHealth;

        c_collider.center = new Vector3(0, 0.75f, 0);
        c_collider.radius = 0.75f;
        c_collider.height = 2.5f;

        //minimumBombDistance = 20;
    }

    public void SetCalculatedVelocity(Vector3 newVelocity)
    {
        bombVelocity = newVelocity;
    }

    public bool IsBombAttackAllowed()
    {

        if (Time.time > lastBombShotTime + bombFireRate)
        {
            lastBombShotTime = Time.time;
            return true;
        }
        else
        {
            return false;
        }
    }
}
