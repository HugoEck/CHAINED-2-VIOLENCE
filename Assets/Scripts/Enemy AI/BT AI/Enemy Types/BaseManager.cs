using Obi;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AI;

public class BaseManager : MonoBehaviour
{

    //Detta skript innehåller alla baskomponenter, basvariabler och basmetoder för alla andra enemy managers.

    //----------------------------------------------------------------------------------------
    // OBS! NI FÅR VARKEN LÄGGA TILL ELLER ÄNDRA KOMPONENTER/VARIABLER/METODER I DETTA SKRIPT UTAN MIN TILLÅTELSE!
    //----------------------------------------------------------------------------------------

    public float maxHealth;
    public float currentHealth;
    public float attack, defense, speed, attackSpeed;
    public float attackRange;
    public float unitCost;

    [Header("GE EJ VÄRDE")]

    //SKRIPTS
    [HideInInspector] public AIPath navigation;
    [HideInInspector] public AIChainEffects chainEffects;
    [HideInInspector] public AIParticleEffects particleEffects;
    [HideInInspector] public AIBehaviorMethods behaviorMethods;
    [HideInInspector] public AIVisuals visuals;

    //KOMPONENTER
    [HideInInspector] public GameObject player1;
    [HideInInspector] public GameObject player2;
    [HideInInspector] public Player playerManager1;
    [HideInInspector] public Player playerManager2;
    [HideInInspector] public Transform targetedPlayer;
    [HideInInspector] public Animator animator;
    [HideInInspector] public CapsuleCollider c_collider;
    [HideInInspector] public Rigidbody rb;

    //VARIABLER
    [HideInInspector] public string enemyID;
    [HideInInspector] public bool activateDeathTimer = false;
    [HideInInspector] public bool agentIsDead = false;

    public virtual void Awake()
    {
        chainEffects = new AIChainEffects();
        particleEffects = new AIParticleEffects();
        behaviorMethods = new AIBehaviorMethods(this);
        visuals = new AIVisuals(this);

        visuals.InitializeVisuals(this);

        rb = GetComponentInChildren<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        c_collider = GetComponentInChildren<CapsuleCollider>();
        c_collider.center = new Vector3(0, 1, 0);
        c_collider.radius = 0.5f;
        c_collider.height = 2;

        behaviorMethods.ToggleRagdoll(false);

        player1 = GameObject.FindGameObjectWithTag("Player1");
        player2 = GameObject.FindGameObjectWithTag("Player2");

        animator = gameObject.GetComponent<Animator>();
        navigation = GetComponent<AIPath>();

        playerManager1 = player1.GetComponent<Player>();
        playerManager2 = player2.GetComponent<Player>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            currentHealth = 0;
        }
        visuals.FlashColor();
    }
    public virtual void DealDamageToEnemy(float damage)
    {
        if (defense - damage < 0)
        {
            particleEffects.ActivateBloodParticles(transform);
            currentHealth = currentHealth + defense - damage;


            visuals.ActivateVisuals();
        }
    }
}


