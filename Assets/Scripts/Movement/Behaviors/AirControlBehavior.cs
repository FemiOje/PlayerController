using UnityEngine;

namespace PlayerController.Movement.Behaviors
{
    /// <summary>
    /// Air control movement behavior implementation.
    /// Provides limited movement control while airborne.
    /// </summary>
    public class AirControlBehavior
    {
        private readonly float walkSpeed;
        private readonly float airControl;
        
        /// <summary>
        /// Initializes a new instance of the AirControlBehavior.
        /// </summary>
        /// <param name="walkSpeed">Base walking speed.</param>
        /// <param name="airControl">Air control multiplier (0-1).</param>
        public AirControlBehavior(float walkSpeed, float airControl)
        {
            this.walkSpeed = walkSpeed;
            this.airControl = airControl;
        }
        
        /// <summary>
        /// Calculates the movement force for air control.
        /// </summary>
        /// <param name="moveDirection">Normalized movement direction.</param>
        /// <param name="deltaTime">Time delta for frame-independent movement.</param>
        /// <returns>Movement force vector.</returns>
        public Vector3 CalculateForce(Vector3 moveDirection, float deltaTime)
        {
            float airSpeed = walkSpeed * airControl;
            return moveDirection * airSpeed * deltaTime;
        }
        
        /// <summary>
        /// Gets the current movement speed.
        /// </summary>
        public float GetSpeed()
        {
            return walkSpeed * airControl;
        }
    }
}

