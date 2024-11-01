using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CyberGiantManager : BaseManager
{
    //[HideInInspector]

    Node rootNode;

    [Header("CYBERGIANT MANAGER")]
    public GameObject bombPrefab;
    public GameObject missilePrefab;
    public Transform bombShootPoint;
    public Transform missileShootPoint;
    public bool missilePrepActivated = false;
    public bool missileSent = false;
    public float currentTime = 0;

    public bool missileReady = false;
    public bool bombReady = false;

    [HideInInspector] public float bombDamage;
    [HideInInspector] public float missileDamage;
    [HideInInspector] public float minimumBombDistance;
    [HideInInspector] public float minimumMissileDistance;
    
    [HideInInspector] public Vector3 bombVelocity;
    [HideInInspector] public Vector3 p1_LastPosition;
    [HideInInspector] public Vector3 p2_LastPosition;
    [HideInInspector] public Vector3 chain_LastPosition;
    [HideInInspector] public Vector3 p1_Velocity;
    [HideInInspector] public Vector3 p2_Velocity;
    [HideInInspector] public Vector3 chain_Velocity;


    [HideInInspector] public bool abilityInProgress = false;
    



    private float lastBombShotTime = 3;
    private float bombCooldown;
    private float lastMissileShotTime = 0;
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

        IsMissileReady();
        IsBombReady();
    }

    

    private void LoadStats()
    {
        maxHealth = 100;
        currentHealth = maxHealth;
        minimumBombDistance = 20;
        minimumMissileDistance = 25;
        bombCooldown = 3;
        bombDamage = 10;
        missileCooldown = 10;
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
            missileReady = true;
            return true;
        }
        else
        {
            
            return false;
        }
    }


    public void SetBombVelocity(Vector3 newVelocity)
    {
        bombVelocity = newVelocity;
    }

    public bool IsBombReady()
    {

        if (Time.time > lastBombShotTime + bombCooldown)
        {
            lastBombShotTime = Time.time;
            bombReady = true;
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
        CalculateBombPosition calculateBombPosition = new CalculateBombPosition();
        ShootBomb shootBomb = new ShootBomb();
        IsMissileReady isMissileReady = new IsMissileReady();
        PrepareMissiles prepareMissiles = new PrepareMissiles();
        CalculateMissilePosition calculateMissilePosition = new CalculateMissilePosition();
        ShootMissiles shootMissiles = new ShootMissiles();

        //Kill Branch
        Sequence isDead = new Sequence(new List<Node> { checkIfDead, killAgent });

        //Range Branch
        Sequence bomb = new Sequence(new List<Node> { calculateBombPosition, shootBomb });
        Sequence missiles = new Sequence(new List<Node> { isMissileReady, prepareMissiles, calculateMissilePosition, shootMissiles });
        Sequence longRange = new Sequence(new List<Node> { bomb, missiles });


        rootNode = new Selector(new List<Node>() { isDead, longRange });
    }
}
