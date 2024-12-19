using HighlightPlus;
using Obi;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

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

    [HideInInspector] public float maxHealthModifier = 0;
    [HideInInspector] public float attackModifier = 0;
    [HideInInspector] public float defenseModifier = 0;
    [HideInInspector] public float attackSpeedModifier = 0;

    [Header("GE EJ VÄRDE")]

    //SKRIPTS
    [HideInInspector] public AIPath navigation;
    [HideInInspector] public AIChainEffects chainEffects;
    [HideInInspector] public AIParticleEffects particleEffects;
    [HideInInspector] public AIBehaviorMethods behaviorMethods;
    [HideInInspector] public AIVisuals visuals;

    private SpawnDamageText damageText;

    //KOMPONENTER
    [HideInInspector] public GameObject player1;
    [HideInInspector] public GameObject player2;
    [HideInInspector] public Player playerManager1;
    [HideInInspector] public Player playerManager2;
    [HideInInspector] public Transform targetedPlayer;
    [HideInInspector] public Animator animator;
    [HideInInspector] public CapsuleCollider c_collider;
    [HideInInspector] public Rigidbody rb;

    private HighlightEffect highlightEffect;

    //VARIABLER
    [HideInInspector] public string enemyID;
    [HideInInspector] public bool activateDeathTimer = false;
    [HideInInspector] public bool agentIsDead = false;
    [HideInInspector] public bool PCG_componentsInstantiated = false;
    [HideInInspector] public Vector3 originalScale;

    float distance;
    [HideInInspector] public Player chosenPlayerManager;

    //PLAYER EFFECTS
    public enum DamageType
    {
        WeaponDamage,
        UnarmedDamage,
        TrapsDamage,
        AbilityDamage,
        UltimateElectricity,
        UltimateFire,
        UltimateLaser,
        ExplosionDamage,
    }

    public virtual void Awake()
    {
        chainEffects = new AIChainEffects(this);
        particleEffects = new AIParticleEffects();
        behaviorMethods = new AIBehaviorMethods(this);
        visuals= new AIVisuals(this);

        visuals.InitializeVisuals(this);

        rb = GetComponentInChildren<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        c_collider = GetComponentInChildren<CapsuleCollider>();
        c_collider.center = new Vector3(0, 1, 0);
        c_collider.radius = 0.5f;
        c_collider.height = 2;
        originalScale = transform.localScale;


        player1 = GameObject.FindGameObjectWithTag("Player1");
        player2 = GameObject.FindGameObjectWithTag("Player2");

        animator = gameObject.GetComponent<Animator>();

        navigation = GetComponent<AIPath>();


        playerManager1 = player1.GetComponent<Player>();
        playerManager2 = player2.GetComponent<Player>();

        //behaviorMethods.ToggleRagdoll(false);
        behaviorMethods.GetRagdollComponents(this);

        if(highlightEffect != null)
        {
            highlightEffect = GetComponent<HighlightEffect>();
        }
        
        
        damageText = GetComponentInChildren<SpawnDamageText>();
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            currentHealth = 0;
        }
        
        //Hej sam förlåt för jag har rört i din manager //Victor
        if (gameObject.transform.position.y < -10 && currentHealth > 0)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
        }
    }
    public virtual void DealDamageToEnemy(float damage, DamageType damageType, bool player1Attacked, bool player2Attacked)
    {
        if(currentHealth > 0)
        {
            if(player1Attacked)
            {
                playerManager1.bHasPlayerEnteredCombat = true;
            }
            if(player2Attacked)
            {
                playerManager2.bHasPlayerEnteredCombat = true;
            }

            if (damageType == DamageType.WeaponDamage || damageType == DamageType.UnarmedDamage)
            {
                particleEffects.ActivateHitParticles(transform);
            }

            if (defense - damage < 0)
            {
                if(damageText != null)
                {
                    damageText.Spawn(c_collider.transform.position + Vector3.up * (c_collider.height / 2), damage);
                }
                

                if(highlightEffect != null)
                {
                    highlightEffect.HitFX();
                }
                
                particleEffects.ActivateBloodParticles(transform);

                currentHealth = currentHealth + defense - damage;


                visuals.ActivateVisuals();
            }
        }
    }

    public virtual void DealDamageToPlayer()
    {
        targetedPlayer = behaviorMethods.CalculateClosestTarget();
        distance = Vector3.Distance(transform.position, targetedPlayer.position);

        if (distance <= attackRange)
        {
            chosenPlayerManager.SetHealth(attack);
            
        }
    }
        
}


