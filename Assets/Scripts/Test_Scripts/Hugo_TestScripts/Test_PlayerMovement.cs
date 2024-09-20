using UnityEngine;

public class Test_PlayerMovement : MonoBehaviour
{
    public float speed = 5.0f; // Speed of the player movement

    private CharacterController controller;
    private Camera mainCamera;

    void Start()
    {
        controller = GetComponent<CharacterController>(); // Get the CharacterController component
        mainCamera = Camera.main; // Get the main camera in the scene
    }

    void Update()
    {
        // Get input from the player
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right keys for horizontal movement
        float moveZ = Input.GetAxis("Vertical");   // W/S or Up/Down keys for forward/backward movement

        // Get the forward and right directions relative to the camera's rotation
        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;

        // Eliminate the vertical component of the camera directions
        cameraForward.y = 0;
        cameraRight.y = 0;

        // Normalize the vectors to get the correct movement directions
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Calculate the desired movement direction based on camera-aligned input
        Vector3 move = cameraForward * moveZ + cameraRight * moveX;

        // Normalize movement direction to maintain consistent speed
        if (move.magnitude > 1)
        {
            move.Normalize();
        }

        // Move the player using the CharacterController
        controller.Move(move * speed * Time.deltaTime);
    }
}