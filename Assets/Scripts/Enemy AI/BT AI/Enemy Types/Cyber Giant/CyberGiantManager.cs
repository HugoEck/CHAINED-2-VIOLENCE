using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class CyberGiantManager : BaseManager
{
    //[HideInInspector]
    private bool testBool = false;
    Node rootNode;

    [Header("CYBERGIANT MANAGER")]
    public GameObject bombPrefab;
    public GameObject missilePrefab;
    public Transform bombShootPoint;
    public Transform missileShootPoint;
    private CapsuleCollider damageCollider;

    public float currentTime = 0;
    [Header("CG STATS")]
    
    public float bombDamage;
    public float missileDamage;
    public float bombCooldown;
    public float longRangeCooldown;
    public float midRangeCooldown;
    public float closeRangeCooldown;

    [Header("ABILITY RANGE")]

    public float minBombDistance;
    public float minLongRangeDistance;
    public float maxMidRangeDistance;
    public float maxCloseRangeDistance;

    [Header("BOOLS")]

    [HideInInspector] public Vector3 bombVelocity;
    [HideInInspector] public Vector3 p1_LastPosition;
    [HideInInspector] public Vector3 p2_LastPosition;
    [HideInInspector] public Vector3 chain_LastPosition;
    [HideInInspector] public Vector3 p1_Velocity;
    [HideInInspector] public Vector3 p2_Velocity;
    [HideInInspector] public Vector3 chain_Velocity;

    public bool abilityInProgress = false;
    public bool missileRainActive = false;
    public bool missileReady = false;
    public bool JumpEngageActive = false;

    private float lastBombShotTime = 0;
    private float lastLongRangeTime = 0;
    private float lastMidRangeTime = 0;

    public float distance;



    void Start()
    {
        enemyID = "CyberGiant";

        animator.SetBool("CyberGiant_StartChasing", true);

        damageCollider = gameObject.AddComponent<CapsuleCollider>();
        damageCollider.isTrigger = true;
        LoadStats();
        ConstructBT();
        
    }

    private void Update()
    {
        rootNode.Evaluate(this);

        if(Input.GetKeyDown(KeyCode.B))
        {
            testBool = true;
        }
        abilityInProgress = CheckIfAbilityInProgress();
        testBool = IsLongRangeAbilityReady();
    }

    private void LoadStats()
    {
        maxHealth = 100;
        currentHealth = maxHealth;
        navigation.maxSpeed = speed;

        //minimumBombDistance = 20;
        //minimumLongRangeDistance = 25;
        //bombCooldown = 3;
        //bombDamage = 10;
        //longRangeCooldown = 15;
        //midRangeCooldown = 15;
        //missileDamage = 20;
        attackRange = 8;

        c_collider.center = new Vector3(0, 1f, 0);
        c_collider.radius = 0.75f;
        c_collider.height = 2.5f;
        damageCollider.center = new Vector3(0, 1f, 0);
        damageCollider.radius = 0.75f;
        damageCollider.height = 2.5f;

    }
    public bool CheckIfAbilityInProgress()
    {
        if (missileRainActive || JumpEngageActive)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsLongRangeAbilityReady()
    {
        if (Time.time > lastLongRangeTime + longRangeCooldown)
        {
            lastLongRangeTime = Time.time;
            missileReady = true;
            return true;
        }
        else if(testBool == true)
        {

            return false;
        }
        else
        {
            return false;
        }
    }
    public bool IsMidRangeAbilityReady()
    {
        if (Time.time > lastMidRangeTime + midRangeCooldown)
        {
            lastMidRangeTime = Time.time;

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
            //bombReady = true;
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
        LongRangeConditions longRangeConditions = new LongRangeConditions();
        PrepareMissiles prepareMissiles = new PrepareMissiles();
        CalculateMissilePosition calculateMissilePosition = new CalculateMissilePosition();
        ShootMissiles shootMissiles = new ShootMissiles();
        ChasePlayer chasePlayer = new ChasePlayer();
        MidRangeConditions midRangeConditions = new MidRangeConditions();
        IsJumpEngageChosen isJumpEngageChosen = new IsJumpEngageChosen();
        JumpEngage jumpEngage = new JumpEngage();


        //Kill Branch
        Sequence isDead = new Sequence(new List<Node> { checkIfDead, killAgent });

        //ATTACK

        //Long-Range Branch
        Sequence bomb = new Sequence(new List<Node> { calculateBombPosition, shootBomb });
        Sequence ability_missileRain = new Sequence(new List<Node> { prepareMissiles, calculateMissilePosition, shootMissiles });
        Selector LR_Ability = new Selector(new List<Node> { ability_missileRain});
        Sequence longRange = new Sequence(new List<Node> { longRangeConditions, LR_Ability });
        //Sequence longRange = new Sequence(new List<Node> { bomb, longRangeConditions, LR_Ability });

        //Mid-Range Branch

        Sequence ability_jumpEngage = new Sequence(new List<Node> { isJumpEngageChosen, jumpEngage });
        Selector MR_Ability = new Selector(new List<Node> { ability_jumpEngage });
        Sequence midRange = new Sequence (new List<Node> { midRangeConditions, MR_Ability });

        //main
        Selector attack = new Selector(new List<Node> { longRange, midRange });

        //Chase Branch

        rootNode = new Selector(new List<Node>() { isDead, attack, chasePlayer });
    }
}
////Long-Range Branch
//Sequence bomb = new Sequence(new List<Node> { calculateBombPosition, shootBomb });
//Sequence ability_missileRain = new Sequence(new List<Node> { longRangeConditions, prepareMissiles, calculateMissilePosition, shootMissiles });
//Selector LR_Ability = new Selector(new List<Node> { ability_missileRain });
//Sequence LongRange = new Sequence(new List<Node> { bomb, LR_Ability });
