using Obi;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AI;

public class BaseManager : MonoBehaviour
{

    //Detta skript inneh�ller alla baskomponenter, basvariabler och basmetoder f�r alla andra enemy managers.

    //----------------------------------------------------------------------------------------
    // OBS! NI F�R VARKEN L�GGA TILL ELLER �NDRA KOMPONENTER/VARIABLER/METODER I DETTA SKRIPT UTAN MIN TILL�TELSE!
    //----------------------------------------------------------------------------------------

    public float maxHealth;
    public float currentHealth;
    public float attack, defense, speed, attackSpeed;
    public float attackRange;
    public float unitCost;

    [Header("GE EJ V�RDE")]

    //SKRIPTS
    [HideInInspector] public AIPath navigation;
    [HideInInspector] public AIChainEffects chainEffects;
    [HideInInspector] public AIParticleEffects particleEffects;
    [HideInInspector] public AIBehaviorMethods behaviorMethods;

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

        rb = GetComponentInChildren<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        c_collider = GetComponentInChildren<CapsuleCollider>();
        c_collider.center = new Vector3(0, 1, 0);
        c_collider.radius = 0.5f;
        c_collider.height = 2;

        ToggleRagdoll(false);

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
    }
    public virtual void DealDamageToEnemy(float damage)
    {
        if (defense - damage < 0)
        {
            particleEffects.ActivateBloodParticles();
            currentHealth = currentHealth + defense - damage;
        }
    }
    public virtual void ToggleRagdoll(bool enabled)
    {
        if (!enabled)
        {
            Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rbs in rigidbodies)
            {
                rbs.isKinematic = true; // or you can use rb.gameObject.SetActive(false) to deactivate the GameObject
                rbs.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
            }
            Collider[] capsuleColliders = GetComponentsInChildren<Collider>();
            foreach (Collider capsule1 in capsuleColliders)
            {
                capsule1.enabled = false; // or you can use rb.gameObject.SetActive(false) to deactivate the GameObject
            }
            c_collider.enabled = true;
            rb.isKinematic = false;      
        }
        else
        {
            AIPath aiPath = GetComponent<AIPath>();
            AIDestinationSetter destinationSetter = GetComponent<AIDestinationSetter>();
            ObiCollider obiCollider = GetComponent<ObiCollider>();
            ObiRigidbody obiRb = GetComponent<ObiRigidbody>();
            Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
            SimpleSmoothModifier smoothing = GetComponent<SimpleSmoothModifier>();
            foreach (Rigidbody rbs in rigidbodies)
            {
                rbs.isKinematic = false; // or you can use rb.gameObject.SetActive(false) to deactivate the GameObject
                rbs.constraints = RigidbodyConstraints.None;
                Collider[] capsuleColliders = GetComponentsInChildren<Collider>();
                foreach (Collider capsule1 in capsuleColliders)
                {
                    capsule1.enabled = true; // or you can use rb.gameObject.SetActive(false) to deactivate the GameObject
                }
            }
            //c_collider.enabled = false;
            //rb.isKinematic = true;
            obiCollider.enabled = false;
            obiRb.enabled = false;
            aiPath.enabled = false;
            smoothing.enabled = false;
            destinationSetter.enabled = false;
        }
    }
}

    

