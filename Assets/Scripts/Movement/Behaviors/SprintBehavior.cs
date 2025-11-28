using UnityEngine;

namespace PlayerController.Movement.Behaviors
{
    /// <summary>
    /// Sprinting movement behavior implementation.
    /// Provides increased movement speed when sprinting.
    /// </summary>
    public class SprintBehavior
    {
        private readonly float walkSpeed;
        private readonly float sprintMultiplier;
        
        /// <summary>
        /// Initializes a new instance of the SprintBehavior.
        /// </summary>
        /// <param name="walkSpeed">Base walking speed.</param>
        /// <param name="sprintMultiplier">Sprint speed multiplier.</param>
        public SprintBehavior(float walkSpeed, float sprintMultiplier)
        {
            this.walkSpeed = walkSpeed;
            this.sprintMultiplier = sprintMultiplier;
        }
        
        /// <summary>
        /// Calculates the movement force for sprinting.
        /// </summary>
        /// <param name="moveDirection">Normalized movement direction.</param>
        /// <param name="deltaTime">Time delta for frame-independent movement.</param>
        /// <returns>Movement force vector.</returns>
        public Vector3 CalculateForce(Vector3 moveDirection, float deltaTime)
        {
            float sprintSpeed = walkSpeed * sprintMultiplier;
            return moveDirection * sprintSpeed * deltaTime;
        }
        
        /// <summary>
        /// Gets the current movement speed.
        /// </summary>
        public float GetSpeed()
        {
            return walkSpeed * sprintMultiplier;
        }
    }
}

