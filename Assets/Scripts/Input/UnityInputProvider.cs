using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerController.Input
{
    /// <summary>
    /// Input provider implementation using Unity's new Input System.
    /// Requires the Input System package to be installed.
    /// </summary>
    public class UnityInputProvider : MonoBehaviour, IInputProvider
    {
        [SerializeField] [Tooltip("Input action asset reference")]
        private InputActionAsset inputActions;
        
        [SerializeField] [Tooltip("Mouse sensitivity for camera rotation")]
        private float mouseSensitivity = 2f;
        
        private InputAction moveAction;
        private InputAction lookAction;
        private InputAction jumpAction;
        private InputAction sprintAction;
        
        private void Awake()
        {
            if (inputActions == null)
            {
                Debug.LogError("InputActionAsset is not assigned to UnityInputProvider!");
                return;
            }
            
            // Get action maps and actions
            var playerMap = inputActions.FindActionMap("Player");
            if (playerMap != null)
            {
                moveAction = playerMap.FindAction("Move");
                lookAction = playerMap.FindAction("Look");
                jumpAction = playerMap.FindAction("Jump");
                sprintAction = playerMap.FindAction("Sprint");
            }
        }
        
        private void OnEnable()
        {
            Enable();
        }
        
        private void OnDisable()
        {
            Disable();
        }
        
        /// <summary>
        /// Gets the current input data from Unity Input System.
        /// </summary>
        public InputData GetInput()
        {
            var data = InputData.Default;
            
            if (moveAction != null)
            {
                data.moveInput = moveAction.ReadValue<Vector2>();
            }
            
            if (lookAction != null)
            {
                data.lookInput = lookAction.ReadValue<Vector2>() * mouseSensitivity;
            }
            
            if (jumpAction != null)
            {
                data.jumpPressed = jumpAction.IsPressed();
                data.jumpDown = jumpAction.WasPressedThisFrame();
            }
            
            if (sprintAction != null)
            {
                data.sprintHeld = sprintAction.IsPressed();
            }
            
            return data;
        }
        
        /// <summary>
        /// Enables the input actions.
        /// </summary>
        public void Enable()
        {
            inputActions?.Enable();
        }
        
        /// <summary>
        /// Disables the input actions.
        /// </summary>
        public void Disable()
        {
            inputActions?.Disable();
        }
    }
}

