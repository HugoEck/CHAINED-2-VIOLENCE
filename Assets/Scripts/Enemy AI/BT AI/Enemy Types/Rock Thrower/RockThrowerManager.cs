using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RockThrowerManager : BaseManager
{    
    Node rootNode;

    [Header("ROCKTHROWER MANAGER")]
    public GameObject rockPrefab;
    public Transform throwPoint;

    [Header("GE EJ V�RDE")]
    [HideInInspector]  public Vector3 calculatedVelocity;

    private float evaluationInterval = 0.5f;
    private float timeSinceLastEvaluation = 0f;
    private float randomOffset;

    private CapsuleCollider c_collider;
    private Rigidbody rb;

    void Start()
    {
        enemyID = "RockThrower";
        animator.SetBool("RockThrower_StartChasing", true);
        currentHealth = maxHealth;
        navigation.maxSpeed = speed;

        c_collider = GetComponent<CapsuleCollider>();
        c_collider.center = new Vector3(0, 1, 0);
        c_collider.radius = 0.5f;
        c_collider.height = 2;

        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

        ConstructBT();
    }

    private void FixedUpdate()
    {
        timeSinceLastEvaluation += Time.fixedDeltaTime;

        if (timeSinceLastEvaluation >= evaluationInterval + randomOffset)
        {
            timeSinceLastEvaluation -= evaluationInterval;
            rootNode.Evaluate(this);
            randomOffset = Random.Range(0f, evaluationInterval);
        }

    }
    private void ConstructBT()
    {

        CheckIfDead checkIfDead = new CheckIfDead();
        KillAgent killAgent = new KillAgent();
        PlayerInRange playerInRange = new PlayerInRange();
        CalculateThrow calculateThrow = new CalculateThrow();
        ThrowRock throwRock = new ThrowRock();
        ChasePlayer chasePlayer = new ChasePlayer();

        Sequence attack = new Sequence(new List<Node> { playerInRange, calculateThrow, throwRock });
        Sequence isDead = new Sequence(new List<Node> { checkIfDead, killAgent });

        rootNode = new Selector(new List<Node>() { isDead, attack, chasePlayer });
    }

    public void SetCalculatedVelocity(Vector3 newVelocity)
    {
        calculatedVelocity = newVelocity;
    }

}
