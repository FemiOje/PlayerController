using UnityEngine;

namespace PlayerController.Input
{
    /// <summary>
    /// Data structure containing normalized input values from the input system.
    /// Used to pass input data between systems without tight coupling.
    /// </summary>
    [System.Serializable]
    public struct InputData
    {
        /// <summary>
        /// Normalized movement input vector (X: horizontal, Y: vertical).
        /// Values range from -1 to 1.
        /// </summary>
        public Vector2 moveInput;
        
        /// <summary>
        /// Mouse look input vector (X: horizontal rotation, Y: vertical rotation).
        /// Values range from -1 to 1.
        /// </summary>
        public Vector2 lookInput;
        
        /// <summary>
        /// Whether the jump button is currently pressed.
        /// </summary>
        public bool jumpPressed;
        
        /// <summary>
        /// Whether the jump button was pressed this frame.
        /// </summary>
        public bool jumpDown;
        
        /// <summary>
        /// Whether the sprint button is currently held.
        /// </summary>
        public bool sprintHeld;
        
        /// <summary>
        /// Creates a new InputData instance with default values.
        /// </summary>
        public static InputData Default => new InputData
        {
            moveInput = Vector2.zero,
            lookInput = Vector2.zero,
            jumpPressed = false,
            jumpDown = false,
            sprintHeld = false
        };
    }
}

