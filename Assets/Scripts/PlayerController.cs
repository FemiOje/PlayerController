using UnityEngine;

/// <summary>
/// Orchestrates all player control systems.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(GroundDetector))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] [Tooltip("Movement speed in units per second (walk speed). Matches StarterAssets default: 2.0")]
    private float moveSpeed = 2.0f;

    [SerializeField] [Tooltip("Speed multiplier when sprinting (e.g., 2.5 = 2.5x walk speed, resulting in ~5.0 sprint speed)")]
    private float sprintMultiplier = 3.0f;

    [SerializeField] [Tooltip("Acceleration rate when moving (higher = faster acceleration). Recommended: 50")]
    private float acceleration = 50f;

    [SerializeField] [Tooltip("Deceleration rate when stopping (higher = stops faster). Recommended: 70")]
    private float deceleration = 70f;

    [SerializeField] [Tooltip("Time it takes to smoothly rotate to face movement direction (in seconds). Lower values = faster rotation.")]
    [Range(0.0f, 0.3f)]
    private float rotationSmoothTime = 0.12f;

    [SerializeField] [Tooltip("Rate at which animation blend transitions (for smooth speed changes)")]
    private float speedChangeRate = 10.0f;

    [Header("Jump")]
    [SerializeField] [Tooltip("Force applied upward when jumping")]
    private float jumpForce = 20.0f;

    [Header("Camera")]
    [SerializeField] [Tooltip("Camera transform for camera-relative movement. If not assigned, will find Main Camera.")]
    private Transform cameraTransform;

    [Header("Animation")]
    [SerializeField] [Tooltip("Animator component for character animations. Optional - animations will be disabled if not assigned.")]
    private Animator animator;

    // Component references
    private Rigidbody rb;
    private GroundDetector groundDetector;

    // System handlers (created in Awake)
    private InputReader inputReader;
    private MovementHandler movementHandler;
    private JumpHandler jumpHandler;
    private RotationHandler rotationHandler;
    private AnimationHandler animationHandler;

    // Animation state tracking
    private bool wasGrounded;
    private bool jumpTriggered;

    private void Awake()
    {
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

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        // Configure Rigidbody - freeze rotation on X and Z axes to prevent physics from rotating player
        ConfigureRigidbody();

        // Initialize system handlers
        inputReader = new InputReader();
        movementHandler = new MovementHandler(rb, transform, cameraTransform, moveSpeed, acceleration, deceleration);
        jumpHandler = new JumpHandler(rb, groundDetector, inputReader, jumpForce);
        rotationHandler = new RotationHandler(transform, cameraTransform, rotationSmoothTime);
        animationHandler = new AnimationHandler(animator, speedChangeRate);

        // Initialize animation state
        wasGrounded = groundDetector.IsGrounded();
        jumpTriggered = false;
    }

    private void Update()
    {
        // Read input every frame
        inputReader.ReadInput();

        // Check if jump should be triggered
        bool previousJumpTriggered = jumpTriggered;
        jumpHandler.CheckJumpInput();

        // Detect if jump was just triggered (for animation)
        bool isGrounded = groundDetector.IsGrounded();
        
        // Update grounded animation
        animationHandler.UpdateGroundedAnimation(isGrounded);

        // Reset jump and free fall animations when grounded
        if (isGrounded)
        {
            animationHandler.ResetJumpAnimation();
            animationHandler.UpdateFreeFallAnimation(false);
            jumpTriggered = false;
        }
        else
        {
            // Update free fall animation (not grounded and falling)
            bool isFreeFalling = rb.linearVelocity.y < 0f;
            animationHandler.UpdateFreeFallAnimation(isFreeFalling);
        }

        // Trigger jump animation when jump is initiated
        if (!jumpTriggered && inputReader.JumpDown && isGrounded)
        {
            jumpTriggered = true;
            animationHandler.TriggerJumpAnimation();
        }
    }

    private void FixedUpdate()
    {
        float currentSprintMultiplier = inputReader.SprintPressed ? sprintMultiplier : 1.0f;

        movementHandler.Move(inputReader.Horizontal, inputReader.Vertical, currentSprintMultiplier);

        // Apply jump if triggered
        jumpHandler.ApplyJump();


        // Rotate player to face movement direction (smooth rotation in Update for responsiveness)
        rotationHandler.Rotate(inputReader.Horizontal, inputReader.Vertical);

        // Update movement animation parameters
        UpdateMovementAnimation();
    }

    /// <summary>
    /// Updates movement animation parameters (Speed and MotionSpeed).
    /// </summary>
    private void UpdateMovementAnimation()
    {
        // Calculate current horizontal speed from Rigidbody velocity
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        float currentHorizontalSpeed = horizontalVelocity.magnitude;

        // Calculate target speed based on input and sprint state
        Vector2 moveInput = inputReader.GetMoveInput();
        float inputMagnitude = moveInput.magnitude;
        float targetSpeed = 0f;

        if (inputMagnitude > 0.01f)
        {
            float currentSprintMultiplier = inputReader.SprintPressed ? sprintMultiplier : 1.0f;
            targetSpeed = moveSpeed * currentSprintMultiplier;
        }

        // Update animation with current speed, target speed, and input magnitude
        animationHandler.UpdateMovementAnimation(currentHorizontalSpeed, targetSpeed, inputMagnitude);
    }

    /// <summary>
    /// Configures the Rigidbody for character movement.
    /// </summary>
    private void ConfigureRigidbody()
    {
        if (rb == null) return;

        // Freeze rotation to prevent player from tumbling
        rb.freezeRotation = true;

        // Reduces jitter when the camera renders at a different framerate than physics.
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        // Helps with fast movement and stability.
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }
}

