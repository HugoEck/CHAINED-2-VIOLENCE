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
        StoreOriginalMasses();
        if(setToZero )
        SetMassesToZero();
    }

    private void Update()
    {
        if(originalMasses == null)
        {
            StoreOriginalMasses();
        }
    }

    void StoreOriginalMasses()
    {
       
        originalMasses.Clear();


        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>(true);


        foreach (var rb in rigidbodies)
        {
            if (rb != null && rb.gameObject != gameObject) 
            {
                originalMasses[rb] = rb.mass; 
            }
        }
    }

    public void SetMassesToZero()
    {
        foreach (var rb in originalMasses.Keys)
        {
            if (rb != null)
            {
                rb.mass = 0.0001f; 
            }
        }
    }


    public void RestoreOriginalMasses()
    {
        foreach (var kvp in originalMasses)
        {
            if (kvp.Key != null)
            {
                kvp.Key.mass = kvp.Value;
            }
        }
    }
}
