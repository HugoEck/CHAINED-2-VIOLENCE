using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WobbleEffect : MonoBehaviour
{
    public float wobbleDuration = 4f;
    public float initialWobbleSpeed = 3.5f; // Starting wobble speed

    public float wobbleSpeed = 3f;
    public float wobbleAmount = 0.1f;

    public Vector3 originalScale;
    private float wobbleTimer = 0f;

    private float wobbleTimeTracker = 0f; // Custom timer to track the wobble progress


    void Start()
    {
        originalScale = transform.localScale;
        StartWobble();
    }

    void Update()
    {
        if (wobbleTimer > 0)
        {
            wobbleTimer -= Time.deltaTime;

            float currentWobbleSpeed = Mathf.Lerp(initialWobbleSpeed, 0, 1 - (wobbleTimer / wobbleDuration));

            wobbleTimeTracker += Time.deltaTime * currentWobbleSpeed;


            // Calculate wobble effect using Sine wave
            float wobbleFactorY = Mathf.Cos(wobbleTimeTracker) * wobbleAmount;

            // Apply effect to object's scale
            transform.localScale = new Vector3(originalScale.x, originalScale.y + wobbleFactorY, originalScale.z);

            // Timer reaches 0, reset to original scale
            if (wobbleTimer <= 0)
            {
                transform.localScale = originalScale;
            }
        }
    }
    public void StartWobble()
    {
        wobbleTimer = wobbleDuration;
        wobbleTimeTracker = 0f; // Reset the custom time tracker
    }
}
