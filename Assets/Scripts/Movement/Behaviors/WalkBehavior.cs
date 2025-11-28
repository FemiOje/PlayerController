using UnityEngine;

namespace PlayerController.Movement.Behaviors
{
    /// <summary>
    /// Walking movement behavior implementation.
    /// Provides standard ground movement at base speed.
    /// </summary>
    public class WalkBehavior
    {
        private readonly float walkSpeed;
        
        /// <summary>
        /// Initializes a new instance of the WalkBehavior.
        /// </summary>
        /// <param name="walkSpeed">Base walking speed.</param>
        public WalkBehavior(float walkSpeed)
        {
            this.walkSpeed = walkSpeed;
        }
        
        /// <summary>
        /// Calculates the movement force for walking.
        /// </summary>
        /// <param name="moveDirection">Normalized movement direction.</param>
        /// <param name="deltaTime">Time delta for frame-independent movement.</param>
        /// <returns>Movement force vector.</returns>
        public Vector3 CalculateForce(Vector3 moveDirection, float deltaTime)
        {
            return moveDirection * walkSpeed * deltaTime;
        }
        
        /// <summary>
        /// Gets the current movement speed.
        /// </summary>
        public float GetSpeed()
        {
            return walkSpeed;
        }
    }
}

