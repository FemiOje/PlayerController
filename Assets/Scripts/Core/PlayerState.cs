using UnityEngine;

namespace PlayerController.Core
{
    /// <summary>
    /// Enumeration of possible player character states.
    /// Used by the state machine to manage character behavior.
    /// </summary>
    public enum PlayerState
    {
        /// <summary>
        /// Player is on the ground and can move normally.
        /// </summary>
        Grounded,
        
        /// <summary>
        /// Player is in the air, either falling or jumping.
        /// </summary>
        Airborne,
        
        /// <summary>
        /// Player is in the process of jumping (transitional state).
        /// </summary>
        Jumping
    }
}

