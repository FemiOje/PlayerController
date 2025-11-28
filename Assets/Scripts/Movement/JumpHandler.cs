using UnityEngine;
using PlayerController.Movement.Data;
using PlayerController.Events;

namespace PlayerController.Movement
{
    /// <summary>
    /// Handles jump mechanics for the player character.
    /// Manages jump input, force application, and jump state.
    /// </summary>
    public class JumpHandler
    {
        private readonly Rigidbody rigidbody;
        private readonly MovementData movementData;
        private readonly GroundDetector groundDetector;
        
        private int currentJumps;
        private bool jumpInputHeld;
        
        /// <summary>
        /// Initializes a new instance of the JumpHandler.
        /// </summary>
        /// <param name="rigidbody">The player's Rigidbody component.</param>
        /// <param name="movementData">Movement configuration data.</param>
        /// <param name="groundDetector">Ground detection system.</param>
        public JumpHandler(Rigidbody rigidbody, MovementData movementData, GroundDetector groundDetector)
        {
            this.rigidbody = rigidbody;
            this.movementData = movementData;
            this.groundDetector = groundDetector;
            currentJumps = 0;
            jumpInputHeld = false;
        }
        
        /// <summary>
        /// Processes jump input and applies jump force if conditions are met.
        /// </summary>
        /// <param name="jumpDown">Whether jump was pressed this frame.</param>
        /// <param name="jumpPressed">Whether jump is currently held.</param>
        /// <param name="isGrounded">Whether the player is currently grounded.</param>
        public void ProcessJump(bool jumpDown, bool jumpPressed, bool isGrounded)
        {
            jumpInputHeld = jumpPressed;
            
            // Reset jump count when grounded
            if (isGrounded)
            {
                currentJumps = 0;
            }
            
            // Check if jump should be triggered
            if (jumpDown && CanJump(isGrounded))
            {
                PerformJump();
            }
        }
        
        /// <summary>
        /// Checks if the player can perform a jump.
        /// </summary>
        /// <param name="isGrounded">Whether the player is currently grounded.</param>
        /// <returns>True if jump can be performed, false otherwise.</returns>
        private bool CanJump(bool isGrounded)
        {
            // Can jump if grounded or within coyote time
            if (isGrounded || groundDetector.IsWithinCoyoteTime())
            {
                return currentJumps < movementData.MaxJumps;
            }
            
            // Can perform additional jumps if multi-jump is enabled
            return currentJumps < movementData.MaxJumps;
        }
        
        /// <summary>
        /// Performs a jump by applying force to the rigidbody.
        /// </summary>
        private void PerformJump()
        {
            // Reset vertical velocity for consistent jump height
            Vector3 velocity = rigidbody.velocity;
            velocity.y = 0f;
            rigidbody.velocity = velocity;
            
            // Apply jump force
            rigidbody.AddForce(Vector3.up * movementData.JumpForce, ForceMode.Impulse);
            
            currentJumps++;
            PlayerEvents.InvokeJump();
        }
    }
}

