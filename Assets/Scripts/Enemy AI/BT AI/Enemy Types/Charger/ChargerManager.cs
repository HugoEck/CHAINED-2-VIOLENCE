using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChargerManager : BaseManager
{

    Node rootNode;

    private float evaluationInterval = 0.5f;
    private float timeSinceLastEvaluation = 0f;
    private float randomOffset;

    [Header("CHARGER MANANAGER")]

    public float chargingRange;

    [Header("GE EJ VÄRDE")]

    public bool hasAlreadyCharged = false;

    void Start()
    {
        enemyID = "Charger";
        //animator.SetBool("Plebian_StartChasing", true);
        currentHealth = maxHealth;
        navigation.maxSpeed = speed;
        ConstructBT();

        randomOffset = Random.Range(0f, evaluationInterval);
    }

    private void FixedUpdate()
    {

        
        rootNode.Evaluate(this);
    }

    private void ConstructBT()
    {

        

    }

    


}
