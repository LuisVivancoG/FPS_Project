using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float rotationSpeed = 3f;
    public float jumpForce = 8f;
    public float gravity = 30f;

    private Transform playerTransform;
    private Camera playerCamera;

    private Vector3 velocity;

    private void Start()
    {
        playerTransform = transform;
        playerCamera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor to the center of the screen
    }

    private void Update()
    {
        HandleMovementInput();
        HandleMouseLook();
    }

    private void HandleMovementInput()
    {
        // Get input for movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate movement direction
        Vector3 movementDirection = playerTransform.TransformDirection(new Vector3(horizontalInput, 0f, verticalInput));

        // Apply movement
        playerTransform.Translate(movementDirection * movementSpeed * Time.deltaTime, Space.World);

        // Apply gravity
        ApplyGravity();

        // Handle jumping
        if (IsGrounded())
        {
            velocity.y = -0.5f; // Reset vertical velocity if grounded

            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            }
        }

        // Apply final movement
        playerTransform.Translate(velocity * Time.deltaTime, Space.World);
    }

    private void HandleMouseLook()
    {
        // Get input for mouse look
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Rotate the player (character controller) based on mouse X input
        playerTransform.Rotate(Vector3.up * mouseX * rotationSpeed);

        // Rotate the camera (playerCamera) based on mouse Y input
        playerCamera.transform.Rotate(Vector3.left * mouseY * rotationSpeed);

        // Clamp vertical camera rotation to prevent flipping
        Vector3 currentRotation = playerCamera.transform.localEulerAngles;
        currentRotation.x = Mathf.Clamp(currentRotation.x, -90f, 90f);
        playerCamera.transform.localEulerAngles = currentRotation;
    }

    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
    }

    private bool IsGrounded()
    {
        // Perform a raycast to check if the player is grounded
        float rayLength = 0.1f;
        return Physics.Raycast(playerTransform.position, Vector3.down, rayLength);
    }
}
