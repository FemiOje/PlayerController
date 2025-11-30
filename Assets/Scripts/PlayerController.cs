using UnityEngine;

/// <summary>
/// Orchestrates all player control systems.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(GroundDetector))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] [Tooltip("Movement speed in units per second")]
    private float moveSpeed = 10.0f;

    [SerializeField] [Tooltip("Speed multiplier when sprinting (e.g., 1.5 = 50% faster)")]
    private float sprintMultiplier = 1.5f;

    [SerializeField] [Tooltip("Time it takes to smoothly rotate to face movement direction (in seconds). Lower values = faster rotation.")]
    [Range(0.0f, 0.3f)]
    private float rotationSmoothTime = 0.12f;

    [Header("Jump")]
    [SerializeField] [Tooltip("Force applied upward when jumping")]
    private float jumpForce = 20.0f;

    [Header("Camera")]
    [SerializeField] [Tooltip("Camera transform for camera-relative movement. If not assigned, will find Main Camera.")]
    private Transform cameraTransform;

    // Component references
    private Rigidbody rb;
    private GroundDetector groundDetector;

    // System handlers (created in Awake)
    private InputReader inputReader;
    private MovementHandler movementHandler;
    private JumpHandler jumpHandler;
    private RotationHandler rotationHandler;

    private void Awake()
    {
        // Get required components

        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found on this GameObject! Player movement will not work correctly.");
            return;
        }
        
        groundDetector = GetComponent<GroundDetector>();
        if (groundDetector == null)
        {
            Debug.LogError("GroundDetector component not found on this GameObject! Player movement will not work correctly.");
            return;
        }

        // Find camera if not assigned
        if (cameraTransform == null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                cameraTransform = mainCamera.transform;
            }
            else
            {
                Debug.LogWarning("No camera assigned and Camera.main not found! Movement will not work correctly. Please assign a camera in the Inspector.");
            }
        }

        // Configure Rigidbody - freeze rotation on X and Z axes to prevent physics from rotating player
        ConfigureRigidbody();

        // Initialize system handlers
        inputReader = new InputReader();
        movementHandler = new MovementHandler(rb, transform, cameraTransform, moveSpeed);
        jumpHandler = new JumpHandler(rb, groundDetector, inputReader, jumpForce);
        rotationHandler = new RotationHandler(transform, cameraTransform, rotationSmoothTime);
    }

    private void Update()
    {
        // Read input every frame
        inputReader.ReadInput();

        // Check if jump should be triggered
        jumpHandler.CheckJumpInput();

        // Rotate player to face movement direction (smooth rotation in Update for responsiveness)
        rotationHandler.Rotate(inputReader.Horizontal, inputReader.Vertical);

        // Log input values for debugging
        Vector2 moveInput = inputReader.GetMoveInput();
        Vector2 lookInput = inputReader.GetLookInput();
        bool isGrounded = groundDetector.IsGrounded();

        // Debug.Log($"[INPUT] Move: ({moveInput.x:F2}, {moveInput.y:F2}) | " +
        //           $"Look: ({lookInput.x:F2}, {lookInput.y:F2}) | " +
        //           $"Jump Pressed: {inputReader.JumpPressed} | Jump Down: {inputReader.JumpDown} | " +
        //           $"Sprint: {inputReader.SprintPressed} | Grounded: {isGrounded}");
    }

    private void FixedUpdate()
    {
        // Calculate sprint multiplier based on input
        float currentSprintMultiplier = inputReader.SprintPressed ? sprintMultiplier : 1.0f;

        // Apply movement based on input with sprint multiplier
        movementHandler.Move(inputReader.Horizontal, inputReader.Vertical, currentSprintMultiplier);

        // Apply jump if triggered
        jumpHandler.ApplyJump();
    }

    /// <summary>
    /// Configures the Rigidbody for character movement.
    /// </summary>
    private void ConfigureRigidbody()
    {
        if (rb == null) return;

        // Freeze rotation to prevent player from tumbling
        rb.freezeRotation = true;
    }
}

