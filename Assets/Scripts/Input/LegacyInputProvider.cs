using UnityEngine;

namespace PlayerController.Input
{
    /// <summary>
    /// Input provider implementation using Unity's Legacy Input Manager.
    /// Works with the default Input class (Input.GetAxis, Input.GetKey, etc.).
    /// </summary>
    public class LegacyInputProvider : MonoBehaviour, IInputProvider
    {
        [SerializeField] [Tooltip("Mouse sensitivity for camera rotation")]
        private float mouseSensitivity = 2f;
        
        [SerializeField] [Tooltip("Sprint key (default: Left Shift)")]
        private KeyCode sprintKey = KeyCode.LeftShift;
        
        /// <summary>
        /// Gets the current input data from Legacy Input Manager.
        /// </summary>
        public InputData GetInput()
        {
            var data = InputData.Default;
            
            // Movement input (WASD or Arrow Keys)
            data.moveInput = new Vector2(
                Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical")
            );
            
            // Mouse look input
            data.lookInput = new Vector2(
                Input.GetAxis("Mouse X") * mouseSensitivity,
                Input.GetAxis("Mouse Y") * mouseSensitivity
            );
            
            // Jump input
            data.jumpPressed = Input.GetButton("Jump");
            data.jumpDown = Input.GetButtonDown("Jump");
            
            // Sprint input
            data.sprintHeld = Input.GetKey(sprintKey);
            
            return data;
        }
        
        /// <summary>
        /// Enables the input provider (no-op for Legacy Input).
        /// </summary>
        public void Enable()
        {
            // Legacy Input is always enabled
        }
        
        /// <summary>
        /// Disables the input provider (no-op for Legacy Input).
        /// </summary>
        public void Disable()
        {
            // Legacy Input is always enabled
        }
    }
}

