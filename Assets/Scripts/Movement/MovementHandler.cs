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

    /// <summary>
    /// Initializes the MovementHandler with required dependencies.
    /// </summary>
    /// <param name="rigidbody">The Rigidbody component to apply forces to.</param>
    /// <param name="transform">The player's Transform.</param>
    /// <param name="cameraTransform">The camera's Transform for camera-relative movement.</param>
    /// <param name="moveSpeed">The movement speed in units per second.</param>
    public MovementHandler(Rigidbody rigidbody, Transform transform, Transform cameraTransform, float moveSpeed)
    {
        this.rigidbody = rigidbody;
        this.transform = transform;
        this.cameraTransform = cameraTransform;
        this.moveSpeed = moveSpeed;
    }

    /// <summary>
    /// Applies movement force based on input direction relative to camera orientation.
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
        Vector3 targetVelocity = moveDirection * currentSpeed * Time.fixedDeltaTime;

        rigidbody.AddForce(targetVelocity, ForceMode.VelocityChange);
    }
}

