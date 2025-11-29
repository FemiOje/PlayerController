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

    [Header("Jump")]
    [SerializeField] [Tooltip("Force applied upward when jumping")]
    private float jumpForce = 20.0f;

    // Component references
    private Rigidbody rb;
    private GroundDetector groundDetector;

    // System handlers (created in Awake)
    private InputReader inputReader;
    private MovementHandler movementHandler;
    private JumpHandler jumpHandler;

    private void Awake()
    {
        // Get required components
        rb = GetComponent<Rigidbody>();
        groundDetector = GetComponent<GroundDetector>();

        // Configure Rigidbody
        ConfigureRigidbody();

        // Initialize system handlers
        inputReader = new InputReader();
        movementHandler = new MovementHandler(rb, transform, moveSpeed);
        jumpHandler = new JumpHandler(rb, groundDetector, inputReader, jumpForce);
    }

    private void Update()
    {
        // Read input every frame
        inputReader.ReadInput();

        // Check if jump should be triggered
        jumpHandler.CheckJumpInput();

        // Log input values for debugging
        Vector2 moveInput = inputReader.GetMoveInput();
        Vector2 lookInput = inputReader.GetLookInput();
        bool isGrounded = groundDetector.IsGrounded();

        Debug.Log($"[INPUT] Move: ({moveInput.x:F2}, {moveInput.y:F2}) | " +
                  $"Look: ({lookInput.x:F2}, {lookInput.y:F2}) | " +
                  $"Jump Pressed: {inputReader.JumpPressed} | Jump Down: {inputReader.JumpDown} | " +
                  $"Grounded: {isGrounded}");
    }

    private void FixedUpdate()
    {
        // Apply movement based on input
        movementHandler.Move(inputReader.Horizontal, inputReader.Vertical);

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

