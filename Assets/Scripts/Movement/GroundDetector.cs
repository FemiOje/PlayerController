using UnityEngine;

/// <summary>
/// Responsible for detecting if the player is grounded.
/// Single Responsibility: Ground detection only.
/// </summary>
public class GroundDetector : MonoBehaviour
{
    [Header("Ground Detection")]
    [SerializeField] [Tooltip("Empty GameObject positioned at player's feet for ground checking")]
    private Transform groundCheck;

    [SerializeField] [Tooltip("Radius of the ground check sphere")]
    private float groundCheckRadius = 0.2f;

    [SerializeField] [Tooltip("Layer mask for ground objects")]
    private LayerMask groundLayer;

    /// <summary>
    /// Checks if the player is currently grounded using a sphere check.
    /// </summary>
    /// <returns>True if grounded, false otherwise.</returns>
    public bool IsGrounded()
    {
        if (groundCheck == null)
        {
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

