using PsychoticLab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingDamageText : MonoBehaviour
{
    [SerializeField] private float destroyTextTimer;
    [SerializeField] private TextMesh damageText;

    public float damage { get; set; }

    private Camera mainCamera;

    private float currentScale; // Original scale
    private float randomXMovement;
    private float randomZMovement;

    private float elapsedTime;

    private bool hasStartedScalingDown = false; // Flag to trigger scaling down
    private Vector3 initialScale;              // Store the scale at the start of scaling down

    [SerializeField] private float scaleUpDuration = 0.1f; // Time to scale up quickly

    void Start()
    {
        damageText.text = damage.ToString();
        mainCamera = Camera.main;

        // Store the original scale
        currentScale = transform.localScale.x;

        // Start with scale 0
        transform.localScale = Vector3.zero;

        randomZMovement = Random.Range(0.0f, 0.05f);
        randomXMovement = Random.Range(0.0f, 0.05f);

        Destroy(gameObject, destroyTextTimer);
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        // Movement logic
        transform.position += new Vector3(randomXMovement, 0.1f, randomZMovement);

        // Scale up quickly in the first "scaleUpDuration" seconds
        if (transform.localScale.x < currentScale && !hasStartedScalingDown)
        {
            float scaleTime = elapsedTime / scaleUpDuration; // Normalize to 0 -> 1
            float scale = Mathf.Lerp(0f, currentScale, scaleTime);
            transform.localScale = new Vector3(scale, scale, scale);
        }
        else
        {
            // Start scaling down after scale-up is finished
            if (!hasStartedScalingDown)
            {
                hasStartedScalingDown = true;
                initialScale = transform.localScale; // Capture the scale at the start of scaling down
            }

            // Scale down smoothly for the remaining lifetime
            float remainingTime = (elapsedTime - scaleUpDuration) / (destroyTextTimer - scaleUpDuration);
            float scale = Mathf.Lerp(initialScale.x, 0f, remainingTime);
            transform.localScale = new Vector3(scale, scale, scale);
        }

        // Rotate to face the camera
        if (mainCamera != null)
        {
            transform.LookAt(mainCamera.transform);
            transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
        }
    }
    
}


