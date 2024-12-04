using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDownwards : MonoBehaviour
{
    #region VARIABLES
    // PUBLIC
    public itemAreaSpawner spawner;
    
    public float targetY = -15f;
    public float movementSpeed = 10f;
    
    // PRIVATE
    private float delayBeforeMove = 2f;
    private float delayTimer;

    private WobbleEffect wobble;
    #endregion

    #region START
    private void Start()
    {
        wobble = gameObject.AddComponent<WobbleEffect>();
        wobble.StartWobble();
        delayTimer = delayBeforeMove;
    }
    #endregion

    #region UPDATE
    public void Update()
    {
        if (delayTimer > 0)
        {
            delayTimer -= Time.deltaTime;
            return;
        }
        
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
    #endregion
}
