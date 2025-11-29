using UnityEngine;

/// <summary>
/// Responsible for applying horizontal movement to the player.
/// </summary>
public class MovementHandler
{
    private readonly Rigidbody rigidbody;
    private readonly Transform transform;
    private readonly float moveSpeed;

    /// <summary>
    /// Initializes the MovementHandler with required dependencies.
    /// </summary>
    /// <param name="rigidbody">The Rigidbody component to apply forces to.</param>
    /// <param name="transform">The Transform to calculate movement direction from.</param>
    /// <param name="moveSpeed">The movement speed in units per second.</param>
    public MovementHandler(Rigidbody rigidbody, Transform transform, float moveSpeed)
    {
        this.rigidbody = rigidbody;
        this.transform = transform;
        this.moveSpeed = moveSpeed;
    }

    /// <summary>
    /// Applies movement force based on input direction.
    /// Should be called in FixedUpdate().
    /// </summary>
    /// <param name="horizontal">Horizontal input (-1 to 1).</param>
    /// <param name="vertical">Vertical input (-1 to 1).</param>
    public void Move(float horizontal, float vertical)
    {
        // Calculate move direction relative to player's transform
        Vector3 moveDirection = (transform.forward * vertical + transform.right * horizontal).normalized;

        Vector3 targetVelocity = moveDirection * moveSpeed * Time.fixedDeltaTime;

        rigidbody.AddForce(targetVelocity, ForceMode.VelocityChange);
    }
}

