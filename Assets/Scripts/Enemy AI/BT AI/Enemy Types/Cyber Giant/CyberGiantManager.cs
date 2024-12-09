using System.Collections.Generic;
using UnityEngine;

public class CyberGiantManager : BaseManager
{

    Node rootNode;

    [Header("CYBERGIANT MANAGER")]
    public GameObject bombPrefab;
    public GameObject missilePrefab;
    public GameObject weaponPrefab;
    public Transform bombShootPoint;
    public Transform missileShootPoint;
    public GameObject energyShield;
    [HideInInspector] public CapsuleCollider damageCollider;
    private CapsuleCollider weaponCollider;
    public GameObject effectPrefab;

    [Header("CG STATS")]

    public float energyShieldDefense;
    [HideInInspector] public float baseDefense;
    public float bombDamage;
    public float missileDamage;
    public float overheadSmashDamage;
    public float jumpEngageDamage;
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

    [HideInInspector] public bool abilityInProgress = false;
    [HideInInspector] public bool missileRainActive = false;
    [HideInInspector] public bool missileReady = false;
    [HideInInspector] public bool jumpEngageActive = false;
    [HideInInspector] public bool overheadSmashActive = false;
    [HideInInspector] public bool shieldWalkActive = false;
    [HideInInspector] public bool staggerActive = false;
    [HideInInspector] public bool deathActive = false;

    [HideInInspector] public bool P1_damageApplied = false;
    [HideInInspector] public bool P2_damageApplied = false;
    [HideInInspector] public bool weaponDamageAllowed = false;
    [HideInInspector] public string weaponDamageType;
    [HideInInspector] private float currentWeaponDamage;

    private float lastBombShotTime = 0;
    private float lastLongRangeTime = -5;
    private float lastMidRangeTime = -10;
    private float lastCloseRangeTime = -5;

    public float debugTimer;

    void Start()
    {
        enemyID = "CyberGiant";
        animator.SetBool("CyberGiant_StartChasing", true);
        damageCollider = gameObject.AddComponent<CapsuleCollider>();
        damageCollider.isTrigger = true;
        LoadStats();
        ConstructBT();
        rb.isKinematic = true;

    }

    private void Update()
    {
        rootNode.Evaluate(this);

        ToggleEnergyShield();

        
    }

    private void LoadStats()
    {
        //maxHealth = 50;
        currentHealth = maxHealth;
        navigation.maxSpeed = speed;
        baseDefense = defense;

        c_collider.center = new Vector3(0, 0.7f, 0);
        c_collider.radius = 0.75f;
        c_collider.height = 2.25f;
        damageCollider.center = new Vector3(0, 0.7f, 0);
        damageCollider.radius = 0.75f;
        damageCollider.height = 2.25f;

    }

    public void DealDamageWithWeapon(string player)
    {

        if (CheckIfDamageAllowed(player))
        {
            GetCurrentWeaponDamage();

            if(player == "p1")
            {
                playerManager1.SetHealth(currentWeaponDamage);
                P1_damageApplied = true;
            }
            else
            {
                playerManager2.SetHealth(currentWeaponDamage);
                P2_damageApplied = true;
            }
        }
    }

    

    public bool CheckIfAbilityInProgress()
    {
        if (missileRainActive || jumpEngageActive || overheadSmashActive || staggerActive || deathActive)
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
            return true;
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
    public bool IsCloseRangeAbilityReady()
    {
        if (Time.time > lastCloseRangeTime + closeRangeCooldown)
        {
            lastCloseRangeTime = Time.time;

            return true;
        }
        else
        {
            return false;
        }
    }

    private void ToggleEnergyShield()
    {
        if (!staggerActive)
        {
            Transform closestPlayer = behaviorMethods.CalculateClosestTarget();
            float distance = Vector3.Distance(transform.position, closestPlayer.position);

            if (distance > maxMidRangeDistance && !CheckIfAbilityInProgress())
            {
                energyShield.SetActive(true);
                damageCollider.radius = 1.4f;
                c_collider.radius = 1.4f;
                defense = energyShieldDefense;
            }
            else
            {
                energyShield.SetActive(false);
                damageCollider.radius = 0.75f;
                c_collider.radius = 0.75f;
                defense = baseDefense;
            }
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
            return true;
        }
        else
        {

            return false;
        }
    }

    private bool CheckIfDamageAllowed(string player)
    {
        if (player == "p1")
        {
            if (weaponDamageAllowed && !P1_damageApplied)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (player == "p2")
        {
            if (weaponDamageAllowed && !P2_damageApplied)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else { return false; }
    }

    private float GetCurrentWeaponDamage()
    {
        if (weaponDamageType == "JumpEngage")
        {
            currentWeaponDamage = jumpEngageDamage;
        }
        else if (weaponDamageType == "OverheadSmash")
        {
            currentWeaponDamage = overheadSmashDamage;
        }
        return currentWeaponDamage;
    }

    private void ConstructBT()
    {


        CalculateBombPosition calculateBombPosition = new CalculateBombPosition();
        ShootBomb shootBomb = new ShootBomb();
        LongRangeConditions longRangeConditions = new LongRangeConditions();
        IsMissileRainChosen isMissileRainChosen = new IsMissileRainChosen();
        MissileRain missileRain = new MissileRain();
        MidRangeConditions midRangeConditions = new MidRangeConditions();
        IsJumpEngageChosen isJumpEngageChosen = new IsJumpEngageChosen();
        JumpEngage jumpEngage = new JumpEngage();
        CloseRangeConditions closeRangeConditions = new CloseRangeConditions();
        IsOverheadSmashChosen isOverheadSmashChosen = new IsOverheadSmashChosen();
        OverheadSmash overheadSmash = new OverheadSmash();
        ChaseConditions chaseConditions = new ChaseConditions();
        CGChasePlayer cg_ChasePlayer = new CGChasePlayer();
        Idle idle = new Idle();
        StaggerConditions staggerConditions = new StaggerConditions();
        Stagger stagger = new Stagger();
        DeathConditions deathConditions = new DeathConditions();
        CGKill cgKill = new CGKill();

        //-----------------------------------------------------------------------------------------------------

        //DEATH
        Sequence killBoss = new Sequence(new List<Node> { deathConditions, cgKill });

        //-----------------------------------------------------------------------------------------------------

        //STAGGER

        Sequence staggerBehavior = new Sequence(new List<Node> { staggerConditions, stagger });


        //-----------------------------------------------------------------------------------------------------
        //ATTACK

        //Long-Range Branch
        Sequence bomb = new Sequence(new List<Node> { calculateBombPosition, shootBomb });
        Sequence ability_missileRain = new Sequence(new List<Node> { isMissileRainChosen, missileRain });
        Selector LR_Ability = new Selector(new List<Node> { ability_missileRain });
        Sequence longRange = new Sequence(new List<Node> { bomb, longRangeConditions, LR_Ability });

        //Mid-Range Branch
        Sequence ability_jumpEngage = new Sequence(new List<Node> { isJumpEngageChosen, jumpEngage });
        Selector MR_Ability = new Selector(new List<Node> { ability_jumpEngage });
        Sequence midRange = new Sequence(new List<Node> { midRangeConditions, MR_Ability });

        //Close-Range Branch
        Sequence ability_overheadSmash = new Sequence(new List<Node> { isOverheadSmashChosen, overheadSmash });
        Selector CR_Ability = new Selector(new List<Node> { ability_overheadSmash });
        Sequence closeRange = new Sequence(new List<Node> { closeRangeConditions, CR_Ability });

        //Attack
        Selector attack = new Selector(new List<Node> { longRange, midRange, closeRange });

        //-----------------------------------------------------------------------------------------------------

        //Chase Branch
        Sequence chase = new Sequence(new List<Node> { chaseConditions, cg_ChasePlayer });

        //-------------------------------------------------------------------------------------------------------
        //rootNode = new Selector(new List<Node>() { killBoss, staggerBehavior, attack, chase, idle });
        rootNode = new Selector(new List<Node>() { chase, idle });
    }
}