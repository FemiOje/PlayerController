using UnityEngine;
using PlayerController.Movement.Data;
using PlayerController.Physics;

namespace PlayerController.Movement
{
    /// <summary>
    /// Handles ground detection for the player character.
    /// Uses raycasting and collision detection to determine if the player is grounded.
    /// </summary>
    public class GroundDetector
    {
        private readonly Transform transform;
        private readonly MovementData movementData;
        private readonly float groundCheckOffset;
        
        private bool wasGrounded;
        private float lastGroundedTime;
        
        /// <summary>
        /// Initializes a new instance of the GroundDetector.
        /// </summary>
        /// <param name="transform">The player's transform component.</param>
        /// <param name="movementData">Movement configuration data.</param>
        /// <param name="groundCheckOffset">Vertical offset for ground check position.</param>
        public GroundDetector(Transform transform, MovementData movementData, float groundCheckOffset = 0.1f)
        {
            this.transform = transform;
            this.movementData = movementData;
            this.groundCheckOffset = groundCheckOffset;
            wasGrounded = false;
            lastGroundedTime = 0f;
        }
        
        /// <summary>
        /// Checks if the player is currently grounded.
        /// </summary>
        /// <returns>True if grounded, false otherwise.</returns>
        public bool IsGrounded()
        {
            Vector3 checkPosition = transform.position + Vector3.up * groundCheckOffset;
            bool grounded = PhysicsUtilities.IsGrounded(
                checkPosition, 
                movementData.GroundCheckDistance + groundCheckOffset, 
                movementData.GroundLayerMask
            );
            
            if (grounded)
            {
                lastGroundedTime = Time.time;
            }
            
            // Check for state change
            if (grounded && !wasGrounded)
            {
                PlayerEvents.InvokeLand();
            }
            else if (!grounded && wasGrounded)
            {
                PlayerEvents.InvokeAirborne();
            }
            
            wasGrounded = grounded;
            return grounded;
        }
        
        /// <summary>
        /// Checks if the player is within coyote time (grace period after leaving ground).
        /// </summary>
        /// <returns>True if within coyote time, false otherwise.</returns>
        public bool IsWithinCoyoteTime()
        {
            return Time.time - lastGroundedTime <= movementData.CoyoteTime;
        }
        
        /// <summary>
        /// Gets the ground normal at the player's position.
        /// </summary>
        /// <param name="normal">Output ground normal vector.</param>
        /// <returns>True if ground is detected, false otherwise.</returns>
        public bool GetGroundNormal(out Vector3 normal)
        {
            Vector3 checkPosition = transform.position + Vector3.up * groundCheckOffset;
            return PhysicsUtilities.GetGroundNormal(
                checkPosition,
                movementData.GroundCheckDistance + groundCheckOffset,
                movementData.GroundLayerMask,
                out normal
            );
        }
    }
}

