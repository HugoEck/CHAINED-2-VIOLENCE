using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulwarkKnightManager : BaseManager
{

    Node rootNode;

    [Header("BULWARK KNIGHT MANAGER")]
    public GameObject shield;
    Transform[] children;


    float shieldDefense = 20;
    float baseDefense = 1;
    float shieldWalkSpeed = 3;
    float runSpeed = 9;
    float shieldAttackSpeed = 2.5f;
    float swordAttackSpeed = 1.25f;
    float shieldAttackRange = 6;
    float swordAttackRange = 6;

    [HideInInspector] public float rageAnimationTimer = 4;

    [HideInInspector] public bool shieldBroken = false;
    [HideInInspector] public bool rageActive = false;

    void Start()
    {
        enemyID = "BulwarkKnight";
        animator.SetBool("BulwarkKnight_StartChasing", true);

        children = GetComponentsInChildren<Transform>();
        shield = FindShieldObject()?.gameObject;
        LoadStats();
        ConstructBT();
    }

    private void Update()
    {
        rootNode.Evaluate(this);

        if (rageActive)
        {
            RageTimer();
        }
    }

    private void LoadStats()
    {
        maxHealth = 100 + maxHealthModifier;
        attack = 10 + attackModifier;
        currentHealth = maxHealth;
        navigation.maxSpeed = shieldWalkSpeed;
        defense = shieldDefense + defenseModifier;
        attackSpeed = shieldAttackSpeed +attackSpeedModifier;
        attackRange = 5;
        attackRange = shieldAttackRange;

    }


    Transform FindShieldObject()
    {

        foreach (Transform child in children)
        {
            if (child.name == "BulwarkKnight_Shield")

                return child;
            
        }
        return null;
    }

    public void BreakShield()
    {
        Destroy(shield);
        shieldBroken = true;
        navigation.maxSpeed = runSpeed;
        defense = baseDefense + defenseModifier;
        attack = 15 + attackModifier;
        attackSpeed = swordAttackSpeed + attackSpeedModifier;
        attackRange = swordAttackRange;
    }

    public void RageTimer()
    {
        rageAnimationTimer -= Time.deltaTime;

        if (rageAnimationTimer + 0.2f < 0)
        {
            rageActive = false;
        }
    }

    private void ConstructBT()
    {
        //KILL BRANCH

        CheckIfDead checkIfDead = new CheckIfDead();
        KillAgent killAgent = new KillAgent();
        Sequence isDead = new Sequence(new List<Node> { checkIfDead, killAgent });

        //CHASE BRANCH

        BKChasePlayer bk_ChasePlayer = new BKChasePlayer();

        //STUN BRANCH

        IsAgentStunned isAgentStunned = new IsAgentStunned();
        StunAgent stunAgent = new StunAgent();
        Sequence isStunned = new Sequence(new List<Node>() { isAgentStunned, stunAgent });

        //RAGE BRANCH

        RageConditions rageConditions = new RageConditions();
        Rage rage = new Rage();
        Sequence isRaging = new Sequence(new List<Node>() { rageConditions, rage });

        //ATTACK BRANCH

        AttackConditions attackConditions = new AttackConditions();
        BKAttackPlayer bk_AttackPlayer = new BKAttackPlayer();
        Sequence attack = new Sequence(new List<Node>() { attackConditions, bk_AttackPlayer });


        rootNode = new Selector(new List<Node>() { isDead, isStunned, isRaging, attack, bk_ChasePlayer });
    }
}
