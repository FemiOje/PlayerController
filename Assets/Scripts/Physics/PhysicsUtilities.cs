using UnityEngine;

namespace PlayerController.Physics
{
    /// <summary>
    /// Utility methods for physics calculations and operations.
    /// Provides reusable physics functions for the player controller.
    /// </summary>
    public static class PhysicsUtilities
    {
        /// <summary>
        /// Checks if a position is on the ground using a raycast.
        /// </summary>
        /// <param name="position">Position to check from.</param>
        /// <param name="distance">Distance to check downward.</param>
        /// <param name="layerMask">Layer mask for ground detection.</param>
        /// <returns>True if ground is detected, false otherwise.</returns>
        public static bool IsGrounded(Vector3 position, float distance, LayerMask layerMask)
        {
            return Physics.Raycast(position, Vector3.down, distance, layerMask);
        }
        
        /// <summary>
        /// Gets the ground normal at a position using a raycast.
        /// </summary>
        /// <param name="position">Position to check from.</param>
        /// <param name="distance">Distance to check downward.</param>
        /// <param name="layerMask">Layer mask for ground detection.</param>
        /// <param name="normal">Output ground normal vector.</param>
        /// <returns>True if ground is detected, false otherwise.</returns>
        public static bool GetGroundNormal(Vector3 position, float distance, LayerMask layerMask, out Vector3 normal)
        {
            if (Physics.Raycast(position, Vector3.down, out RaycastHit hit, distance, layerMask))
            {
                normal = hit.normal;
                return true;
            }
            
            normal = Vector3.up;
            return false;
        }
        
        /// <summary>
        /// Calculates the angle of a slope in degrees.
        /// </summary>
        /// <param name="normal">Surface normal vector.</param>
        /// <returns>Slope angle in degrees (0 = flat, 90 = vertical).</returns>
        public static float GetSlopeAngle(Vector3 normal)
        {
            return Vector3.Angle(normal, Vector3.up);
        }
        
        /// <summary>
        /// Projects a direction onto a plane defined by a normal.
        /// </summary>
        /// <param name="direction">Direction to project.</param>
        /// <param name="planeNormal">Normal of the plane.</param>
        /// <returns>Projected direction vector.</returns>
        public static Vector3 ProjectOnPlane(Vector3 direction, Vector3 planeNormal)
        {
            return Vector3.ProjectOnPlane(direction, planeNormal).normalized;
        }
        
        /// <summary>
        /// Clamps a velocity vector to a maximum magnitude.
        /// </summary>
        /// <param name="velocity">Velocity to clamp.</param>
        /// <param name="maxSpeed">Maximum speed magnitude.</param>
        /// <returns>Clamped velocity vector.</returns>
        public static Vector3 ClampVelocity(Vector3 velocity, float maxSpeed)
        {
            if (velocity.magnitude > maxSpeed)
            {
                return velocity.normalized * maxSpeed;
            }
            return velocity;
        }
    }
}

