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
    float runSpeed = 7;
    float shieldAttackSpeed = 2.5f;
    float swordAttackSpeed = 1;
    float shieldAttackRange = 6;
    float swordAttackRange = 6;

    [HideInInspector] public bool shieldBroken = false;
    private bool runShieldMethodOnce = false;

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

        if (Input.GetKeyDown(KeyCode.L))
        {
            BreakShield();
            runShieldMethodOnce = true;
        }

        if (!runShieldMethodOnce && currentHealth <= maxHealth * 0.5f)
        {
            BreakShield();
            runShieldMethodOnce = true;
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

    private void BreakShield()
    {
        Destroy(shield);
        shieldBroken = true;
        navigation.maxSpeed = runSpeed;
        defense = baseDefense;
        attackSpeed = swordAttackSpeed;
        attackRange = swordAttackRange;
    }

    private void ConstructBT()
    {
        //CHASE BRANCH

        BKChasePlayer bk_ChasePlayer = new BKChasePlayer();

        //STUN BRANCH

        IsAgentStunned isAgentStunned = new IsAgentStunned();
        StunAgent stunAgent = new StunAgent();
        Sequence isStunned = new Sequence(new List<Node>() { isAgentStunned, stunAgent });

        //ATTACK BRANCH

        BKAttackConditions bk_AttackConditions = new BKAttackConditions();
        IfShieldAttackChosen ifShieldAttackChosen = new IfShieldAttackChosen();
        ShieldAttack shieldAttack = new ShieldAttack();
        //SwordAttack

        Sequence ability_shieldAttack = new Sequence(new List<Node>() { ifShieldAttackChosen, shieldAttack });

        Selector chooseAttackType = new Selector(new List<Node>() { ability_shieldAttack });

        Sequence attack = new Sequence(new List<Node>() { bk_AttackConditions, chooseAttackType });


        rootNode = new Selector(new List<Node>() { attack, bk_ChasePlayer });
    }
}
