using UnityEngine;

/// <summary>
/// Player controller with basic movement (Iteration 2).
/// Reads input and applies physics-based movement using Rigidbody.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] [Tooltip("Movement speed in units per second")]
    private float moveSpeed = 10.0f;

    [Header("Jump")]
    [SerializeField] [Tooltip("Force applied upward when jumping")]
    private float jumpForce = 20.0f;

    [Header("Ground Detection")]
    [SerializeField] [Tooltip("Empty GameObject positioned at player's feet for ground checking")]
    private Transform groundCheck;

    [SerializeField] [Tooltip("Radius of the ground check sphere")]
    private float groundCheckRadius = 0.2f;

    [SerializeField] [Tooltip("Layer mask for ground objects")]
    private LayerMask groundLayer;

    private Rigidbody rb;

    private float horizontal;
    private float vertical;
    private float mouseX;
    private float mouseY;
    private bool jumpPressed;
    private bool jumpDown;
    private bool shouldJump;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ConfigureRigidbody();
    }

    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        jumpPressed = Input.GetKey(KeyCode.Space);
        jumpDown = Input.GetKeyDown(KeyCode.Space);

        Vector2 moveInput = new Vector2(horizontal, vertical);
        Vector2 lookInput = new Vector2(mouseX, mouseY);
        bool isGrounded = IsGrounded();

        // Set jump flag if jump key was pressed and player is grounded
        if (jumpDown && isGrounded)
        {
            shouldJump = true;
        }

        // Log input values for debugging
        Debug.Log($"[INPUT] Move: ({moveInput.x:F2}, {moveInput.y:F2}) | " +
                  $"Look: ({lookInput.x:F2}, {lookInput.y:F2}) | " +
                  $"Jump Pressed: {jumpPressed} | Jump Down: {jumpDown} | " +
                  $"Grounded: {isGrounded}");
    }

    private void FixedUpdate()
    {
        // Use input values collected in Update() to apply physics
        // Calculate move direction relative to player's transform
        Vector3 moveDirection = (transform.forward * vertical + transform.right * horizontal).normalized;

        Vector3 targetVelocity = moveDirection * moveSpeed * Time.fixedDeltaTime;

        rb.AddForce(targetVelocity, ForceMode.VelocityChange);

        // Handle jump
        if (shouldJump)
        {
            rb.AddForce(Vector3.up * jumpForce * Time.fixedDeltaTime, ForceMode.Impulse);
            Debug.Log($"[JUMP] Jump performed! Force: {jumpForce}, Velocity after: {rb.linearVelocity}");
            shouldJump = false; // Reset flag after jump
        }
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

    /// <summary>
    /// Checks if the player is currently grounded using a sphere cast.
    /// </summary>
    /// <returns>True if grounded, false otherwise.</returns>
    private bool IsGrounded()
    {
        if (groundCheck == null) {
            Debug.LogError("Ground check transform is not assigned! Please assign a Transform in the Inspector.");
            return false;
        }
        return Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
    }

    /// <summary>
    /// Visualizes the ground check sphere in the Scene view when the GameObject is selected.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}

