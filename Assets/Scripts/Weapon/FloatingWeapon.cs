using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingWeapon : MonoBehaviour
{
    public float rotationSpeed = 50f;
    public float floatAmplitude = 0.5f;
    public float floatSpeed = 1f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {

        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        // Floating effect
        transform.position = startPos + new Vector3(0, Mathf.Sin(Time.time * floatSpeed) * floatAmplitude, 0);
    }
}
