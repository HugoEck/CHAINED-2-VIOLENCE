using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpwards : MonoBehaviour
{
    public float targetY = 0f;
    public float moveSpeed = 10f;

    private WobbleEffect wobble;

    private void Start()
    {
        wobble = gameObject.AddComponent<WobbleEffect>();
        wobble.StartWobble();
    }

    void Update()
    {
        if (transform.position.y < targetY)
        {
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;

            if (transform.position.y > targetY)
            {
                Vector3 clampedPosition = transform.position;
                clampedPosition.y = targetY;
                transform.position = clampedPosition;
            }
        }
        if (transform.position.y >= targetY)
        {
            wobble.wobbleDuration = 1f;
            moveSpeed = 0f;
        }
    }
}
