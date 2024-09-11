using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestRunning : MonoBehaviour
{

    [SerializeField] private float _moveSpeed;

    private PlayerInput playerInput;

    private InputAction moveAction;

    private Rigidbody _rb;

    private Vector3 _movementDirection; 

    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();

        if(gameObject.tag == "player1")
        {
            moveAction = playerInput.actions.FindAction("movementPlayer1");
        }
        if (gameObject.tag == "player2")
        {
            moveAction = playerInput.actions.FindAction("movementPlayer2");
        }
            

    }
  
    private void FixedUpdate()
    {
        MovePlayer();
    }
    private void MovePlayer()
    {
        Vector2 direction = moveAction.ReadValue<Vector2>();
        Debug.Log(moveAction.ReadValue<Vector2>());

        _rb.velocity = new Vector3(direction.x, 0, direction.y) * _moveSpeed;
    }
}
