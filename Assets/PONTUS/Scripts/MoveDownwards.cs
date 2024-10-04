using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDownwards : MonoBehaviour
{
    public float targetY = -15f;
    public float movementSpeed = 10f;

    public itemAreaSpawner spawner;

    public void Update()
    {
        if (transform.position.y > targetY)
        {
            transform.position += Vector3.down * movementSpeed * Time.deltaTime;

            if (transform.position.y < targetY)
            {
                Vector3 clampedPosition = transform.position;
                clampedPosition.y = targetY;
                transform.position = clampedPosition;
            }
        }
        if (transform.position.y <= targetY)
        {
            Destroy(gameObject);
        }
    }
}
