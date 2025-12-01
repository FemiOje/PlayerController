using UnityEngine;

/// <summary>
/// Responsible for applying horizontal movement to the player.
/// </summary>
public class MovementHandler
{
    private readonly Rigidbody rigidbody;
    private readonly Transform transform;
    private readonly Transform cameraTransform;
    private readonly float moveSpeed;
    private readonly float acceleration;
    private readonly float deceleration;

    /// <summary>
    /// Initializes the MovementHandler with required dependencies.
    /// </summary>
    /// <param name="rigidbody">The Rigidbody component to apply forces to.</param>
    /// <param name="transform">The player's Transform.</param>
    /// <param name="cameraTransform">The camera's Transform for camera-relative movement.</param>
    /// <param name="moveSpeed">The movement speed in units per second.</param>
    /// <param name="acceleration">Acceleration rate when moving (higher = faster acceleration).</param>
    /// <param name="deceleration">Deceleration rate when stopping (higher = stops faster).</param>
    public MovementHandler(Rigidbody rigidbody, Transform transform, Transform cameraTransform, float moveSpeed, float acceleration, float deceleration)
    {
        this.rigidbody = rigidbody;
        this.transform = transform;
        this.cameraTransform = cameraTransform;
        this.moveSpeed = moveSpeed;
        this.acceleration = acceleration;
        this.deceleration = deceleration;
    }

    /// <summary>
    /// Applies movement force based on input direction relative to camera orientation.
    /// Uses acceleration when moving and deceleration when stopping.
    /// Should be called in FixedUpdate().
    /// </summary>
    /// <param name="horizontal">Horizontal input (-1 to 1).</param>
    /// <param name="vertical">Vertical input (-1 to 1).</param>
    /// <param name="sprintMultiplier">Speed multiplier when sprinting (1.0 = normal speed).</param>
    public void Move(float horizontal, float vertical, float sprintMultiplier = 1.0f)
    {
        if (cameraTransform == null)
        {
            Debug.LogWarning("Camera transform is null! Movement will not work correctly.");
            return;
        }

        // Get current horizontal velocity (preserve Y for gravity/jumping)
        Vector3 currentVelocity = new Vector3(rigidbody.linearVelocity.x, 0f, rigidbody.linearVelocity.z);
        Vector3 moveInput = new Vector3(horizontal, 0f, vertical);

        // Check if there's meaningful input
        if (moveInput.magnitude > 0.1f)
        {
            // Get camera's forward and right vectors, flattened to horizontal plane
            Vector3 cameraForward = cameraTransform.forward;
            Vector3 cameraRight = cameraTransform.right;

            // Flatten vectors to horizontal plane (ignore Y component)
            cameraForward.y = 0f;
            cameraRight.y = 0f;

            // Normalize to ensure consistent movement speed
            cameraForward.Normalize();
            cameraRight.Normalize();

            // Calculate move direction relative to camera orientation
            Vector3 moveDirection = (cameraForward * vertical + cameraRight * horizontal).normalized;

            // Apply sprint multiplier to movement speed
            float currentSpeed = moveSpeed * sprintMultiplier;
            Vector3 targetVelocity = moveDirection * currentSpeed;

            // Accelerate towards target velocity
            Vector3 velocityChange = (targetVelocity - currentVelocity) * acceleration * Time.fixedDeltaTime;
            rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
        }
        else
        {
            // Apply deceleration when no input
            if (currentVelocity.magnitude < 0.5f)
            {
                // Stop completely if velocity is very small
                rigidbody.linearVelocity = new Vector3(0f, rigidbody.linearVelocity.y, 0f);
            }
            else
            {
                // Apply deceleration force opposite to current velocity direction
                Vector3 decelForce = -currentVelocity.normalized * deceleration * Time.fixedDeltaTime;
                rigidbody.AddForce(decelForce, ForceMode.VelocityChange);
            }
        }
    }
}

