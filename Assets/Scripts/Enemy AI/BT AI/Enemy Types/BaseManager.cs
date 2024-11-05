using Obi;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AI;

public class BaseManager : MonoBehaviour
{

    //Detta skript innehåller alla basvariabler samt basmetoder för alla andra managers.

    //----------------------------------------------------------------------------------------
    // OBS! NI FÅR VARKEN LÄGGA TILL ELLER ÄNDRA VARIABLER/METODER I DETTA SKRIPT UTAN MIN TILLÅTELSE!
    //----------------------------------------------------------------------------------------

    public float maxHealth;
    public float currentHealth;
    public float attack, defense, speed, attackSpeed;
    public float attackRange;
    public float unitCost;



    private float lastAttackedTime;
    private float timer = 5f;

    [Header("GE EJ VÄRDE")]

    [HideInInspector] public AIPath navigation;
    [HideInInspector] public AIChainEffects chainEffects;


    [HideInInspector] public GameObject player1;
    [HideInInspector] public GameObject player2;
    [HideInInspector] public Player playerManager1;
    [HideInInspector] public Player playerManager2;
    [HideInInspector] public Transform targetedPlayer;
    //[HideInInspector] public Vector3 chainPosition;
    [HideInInspector] public Animator animator;
    [HideInInspector] public string enemyID;
    [HideInInspector] public bool activateDeathTimer = false;
    [HideInInspector] public bool agentIsDead = false;


    [HideInInspector] public CapsuleCollider c_collider;
    [HideInInspector] public Rigidbody rb;

    public float trackTime;




    public virtual void Awake()
    {
        chainEffects = new AIChainEffects();

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
        if (Input.GetKeyDown(KeyCode.I))
        {
            chainEffects.ActivateGhostChainEffect(3f);
        }
        trackTime = chainEffects.stunDurationTime;

        if (Input.GetKeyDown(KeyCode.K))
        {
            currentHealth = 0;
        }
        if (activateDeathTimer)
        {
            DeathTimer();
        }
    }

    public virtual void DealDamageToEnemy(float damage)
    {
        if (defense - damage < 0)
        {
            currentHealth = currentHealth + defense - damage;
        }

    }

    public virtual Transform CalculateClosestTarget()
    {
        if (Vector3.Distance(this.transform.position, player1.transform.position) < Vector3.Distance(this.transform.position, player2.transform.position))
        {
            return player1.transform;
        }
        else if((Vector3.Distance(this.transform.position, player2.transform.position) < Vector3.Distance(this.transform.position, player1.transform.position)))
        {
            return player2.transform;
        }
        else
        {
            return null;
        }
    }

    public virtual bool IsAttackAllowed()
    {

        if (Time.time > lastAttackedTime + attackSpeed)
        {
            lastAttackedTime = Time.time;
            return true;
        }
        else
        {
            return false;
        }
    }

    public Player GetCorrectPlayerManager(Transform player)
    {
        if (player == player1.transform)
        {
            return playerManager1;
        }
        else
        {
            return playerManager2;
        }
    }

    public Vector3 CalculateChainPosition()
    {

        Vector3 p1Position = player1.transform.position;
        Vector3 p2Position = player2.transform.position;
        Vector3 midPoint = (p1Position + p2Position) / 2;
        midPoint.y = 0;
        return midPoint;
    }

    public virtual void DeathTimer()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            agentIsDead = true;
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

    

