using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyberGiantManager : BaseManager
{
    //[HideInInspector]

    Node rootNode;

    [Header("CYBERGIANT MANAGER")]
    public GameObject bombPrefab;
    public Transform bombShootPoint;
    public Transform missileShootPoint;


    [HideInInspector] public float bombDamage;
    [HideInInspector] public float missileDamage;
    [HideInInspector] public float minimumBombDistance;
    [HideInInspector] public float minimumMissileDistance;
    
    [HideInInspector] public Vector3 bombVelocity;
    [HideInInspector] public Vector3 p1LastPosition;
    [HideInInspector] public Vector3 p2LastPosition;

    [HideInInspector] public bool abilityInProgress = false;
    [HideInInspector] public bool missileReady = false;



    private float lastBombShotTime = 3;
    private float bombCooldown;
    private float lastMissileShotTime = 5;
    private float missileCooldown;

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

    

    private void LoadStats()
    {
        maxHealth = 100;
        currentHealth = maxHealth;
        minimumBombDistance = 20;
        minimumMissileDistance = 25;
        bombCooldown = 3;
        bombDamage = 10;
        missileCooldown = 3;
        missileDamage = 20;

        c_collider.center = new Vector3(0, 0.75f, 0);
        c_collider.radius = 0.75f;
        c_collider.height = 2.5f;

    }

    public bool IsMissileReady()
    {
        if (Time.time > lastMissileShotTime + missileCooldown)
        {
            lastMissileShotTime = Time.time;

            return true;
        }
        else
        {
            return false;
        }
    }


    public void SetCalculatedVelocity(Vector3 newVelocity)
    {
        bombVelocity = newVelocity;
    }

    public bool IsBombReady()
    {

        if (Time.time > lastBombShotTime + bombCooldown)
        {
            lastBombShotTime = Time.time;
            return true;
        }
        else
        {
            return false;
        }
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
        Sequence bomb = new Sequence(new List<Node> { isBombInRange, calculateBombPosition, shootBomb });


        rootNode = new Selector(new List<Node>() { isDead, bomb });
    }
}
