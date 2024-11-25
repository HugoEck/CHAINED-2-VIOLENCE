using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyMassManager : MonoBehaviour
{
    // Dictionary to store the original mass of each Rigidbody by reference
    private Dictionary<Rigidbody, float> originalMasses = new Dictionary<Rigidbody, float>();
    public bool setToZero;

    void Start()
    {
        // Call this to store original masses at the start
        StoreOriginalMasses();
        if(setToZero )
        SetMassesToZero();
    }

    // Method to store the original masses of all Rigidbody components in the child objects
    void StoreOriginalMasses()
    {
        // Clear any existing entries in case this method is called multiple times
        originalMasses.Clear();

        // Find all Rigidbody components in the child GameObjects (excluding the main object)
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>(true);

        // Iterate over the rigidbodies
        foreach (var rb in rigidbodies)
        {
            if (rb != null && rb.gameObject != gameObject) // Exclude the main object itself
            {
                originalMasses[rb] = rb.mass; // Store the original mass of each Rigidbody
            }
        }
    }

    // Method to set all rigidbodies' masses to 0 in the child objects
    public void SetMassesToZero()
    {
        foreach (var rb in originalMasses.Keys)
        {
            if (rb != null)
            {
                rb.mass = 0.0001f; // Set the mass of the Rigidbody to 0
            }
        }
    }

    // Method to restore all rigidbodies' masses to their original values in the child objects
    public void RestoreOriginalMasses()
    {
        foreach (var kvp in originalMasses)
        {
            if (kvp.Key != null)
            {
                kvp.Key.mass = kvp.Value; // Restore the original mass of the Rigidbody
            }
        }
    }
}
