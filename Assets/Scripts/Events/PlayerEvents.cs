using System;
using UnityEngine;

namespace PlayerController.Events
{
    /// <summary>
    /// Static event definitions for player-related events.
    /// Allows systems to communicate without tight coupling.
    /// </summary>
    public static class PlayerEvents
    {
        /// <summary>
        /// Invoked when the player becomes grounded.
        /// </summary>
        public static event Action OnGrounded;
        
        /// <summary>
        /// Invoked when the player becomes airborne.
        /// </summary>
        public static event Action OnAirborne;
        
        /// <summary>
        /// Invoked when the player initiates a jump.
        /// </summary>
        public static event Action OnJump;
        
        /// <summary>
        /// Invoked when the player lands after being airborne.
        /// </summary>
        public static event Action OnLand;
        
        /// <summary>
        /// Invoked when the player starts sprinting.
        /// </summary>
        public static event Action OnSprintStart;
        
        /// <summary>
        /// Invoked when the player stops sprinting.
        /// </summary>
        public static event Action OnSprintEnd;
        
        /// <summary>
        /// Invokes the OnGrounded event.
        /// </summary>
        public static void InvokeGrounded()
        {
            OnGrounded?.Invoke();
        }
        
        /// <summary>
        /// Invokes the OnAirborne event.
        /// </summary>
        public static void InvokeAirborne()
        {
            OnAirborne?.Invoke();
        }
        
        /// <summary>
        /// Invokes the OnJump event.
        /// </summary>
        public static void InvokeJump()
        {
            OnJump?.Invoke();
        }
        
        /// <summary>
        /// Invokes the OnLand event.
        /// </summary>
        public static void InvokeLand()
        {
            OnLand?.Invoke();
        }
        
        /// <summary>
        /// Invokes the OnSprintStart event.
        /// </summary>
        public static void InvokeSprintStart()
        {
            OnSprintStart?.Invoke();
        }
        
        /// <summary>
        /// Invokes the OnSprintEnd event.
        /// </summary>
        public static void InvokeSprintEnd()
        {
            OnSprintEnd?.Invoke();
        }
    }
}

