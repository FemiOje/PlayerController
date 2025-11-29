using UnityEngine;

/// <summary>
/// Responsible for handling player jumping.
/// </summary>
public class JumpHandler
{
    private readonly Rigidbody rigidbody;
    private readonly GroundDetector groundDetector;
    private readonly InputReader inputReader;
    private readonly float jumpForce;

    private bool shouldJump;

    /// <summary>
    /// Initializes the JumpHandler with required dependencies.
    /// </summary>
    /// <param name="rigidbody">The Rigidbody component to apply jump force to.</param>
    /// <param name="groundDetector">The GroundDetector to check if player is grounded.</param>
    /// <param name="inputReader">The InputReader to get jump input.</param>
    /// <param name="jumpForce">The force applied upward when jumping.</param>
    public JumpHandler(Rigidbody rigidbody, GroundDetector groundDetector, InputReader inputReader, float jumpForce)
    {
        this.rigidbody = rigidbody;
        this.groundDetector = groundDetector;
        this.inputReader = inputReader;
        this.jumpForce = jumpForce;
    }

    /// <summary>
    /// Checks if jump should be triggered based on input and ground state.
    /// Should be called in Update().
    /// </summary>
    public void CheckJumpInput()
    {
        // Set jump flag if jump key was pressed and player is grounded
        if (inputReader.JumpDown && groundDetector.IsGrounded())
        {
            shouldJump = true;
        }
    }

    /// <summary>
    /// Applies jump force if jump was triggered.
    /// Should be called in FixedUpdate().
    /// </summary>
    public void ApplyJump()
    {
        if (shouldJump)
        {
            // Apply upward force for jump
            rigidbody.AddForce(Vector3.up * jumpForce * Time.fixedDeltaTime, ForceMode.Impulse);
            Debug.Log($"[JUMP] Jump performed! Force: {jumpForce}, Velocity after: {rigidbody.linearVelocity}");
            shouldJump = false; // Reset flag after jump
        }
    }
}

