using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSway : MonoBehaviour
{
    [Header("Sway Settings")]
    public float swaySpeed = 1f;      // Speed of the swaying
    public float swayAmount = 3f;     // How far the tree sways (in degrees)

    private Quaternion initialRotation;

    void Start()
    {
        // Store the initial rotation of the tree
        initialRotation = transform.rotation;
    }

    void Update()
    {
        // Calculate the sway angle using a sine wave
        float swayAngle = Mathf.Sin(Time.time * swaySpeed) * swayAmount;

        // Apply the sway rotation to the tree
        transform.rotation = initialRotation * Quaternion.Euler(0f, 0f, swayAngle);
    }
}
