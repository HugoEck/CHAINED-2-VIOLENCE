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

    [HideInInspector] private Material material; // Assign the material in the Inspector
    [HideInInspector] private Color flashColor = Color.white; // The color you want it to flash
    [HideInInspector] private float flashDuration = 0.1f; // Duration of the flash
    [HideInInspector] private Color originalColor;
    [HideInInspector] private bool isFlashing = false;
    Renderer renderer;

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

        behaviorMethods.ToggleRagdoll(false);

        player1 = GameObject.FindGameObjectWithTag("Player1");
        player2 = GameObject.FindGameObjectWithTag("Player2");

        animator = gameObject.GetComponent<Animator>();
        navigation = GetComponent<AIPath>();

        playerManager1 = player1.GetComponent<Player>();
        playerManager2 = player2.GetComponent<Player>();

        renderer = GetComponentInChildren<Renderer>(false);
        foreach (Transform child in transform)
        {
            // Skip if the GameObject is inactive or is the "Root" GameObject
            if (!child.gameObject.activeInHierarchy || child.name == "Root")
            {
                continue;
            }

            // Get the Renderer component from active, direct children
            renderer = child.GetComponent<Renderer>();
            if (renderer != null)
            {
                material = renderer.material;
                break;
            }
        }
        material.EnableKeyword("_EMISSION");
        originalColor = material.GetColor("_EmissionColor");
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
            particleEffects.ActivateBloodParticles(transform);
            currentHealth = currentHealth + defense - damage;


            //Trigger Flash effect here
            if(!isFlashing)
            {
                StartCoroutine(FlashCoroutine());
            }
        }
    }

    private IEnumerator FlashCoroutine()
    {
        isFlashing = true;

        float elapsedTime = 0f;

        // Enable emission if it's off
        material.EnableKeyword("_EMISSION");

        while (elapsedTime < flashDuration)
        {
            // Calculate the current color by interpolating between the original and flash colors
            Color currentColor = Color.Lerp(originalColor, flashColor, elapsedTime / flashDuration);
            material.SetColor("_EmissionColor", currentColor);

            // Increment elapsed time
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Set to the flash color at the end of the flashDuration
        material.SetColor("_EmissionColor", flashColor);

        // Lerp back to the original color
        elapsedTime = 0f;
        while (elapsedTime < flashDuration)
        {
            Color currentColor = Color.Lerp(flashColor, originalColor, elapsedTime / flashDuration);
            material.SetColor("_EmissionColor", currentColor);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset to the original color and disable emission if it was off
        material.SetColor("_EmissionColor", originalColor);
        material.DisableKeyword("_EMISSION");

        isFlashing = false;
    }

}


