using UnityEngine;
using PlayerController.Input;
using PlayerController.Movement;
using PlayerController.Movement.Data;
using PlayerController.Camera;
using PlayerController.Camera.Data;

namespace PlayerController.Core
{
    /// <summary>
    /// Main orchestrator for the player control system.
    /// Coordinates input, movement, camera, and state management.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] [Tooltip("Movement configuration data")]
        private MovementData movementData;
        
        [SerializeField] [Tooltip("Camera configuration data")]
        private CameraData cameraData;
        
        [Header("Input")]
        [SerializeField] [Tooltip("Input provider implementation")]
        private MonoBehaviour inputProviderComponent;
        
        [Header("Camera")]
        [SerializeField] [Tooltip("Camera controller component (optional, can be on separate GameObject)")]
        private CameraController cameraController;
        
        private Rigidbody rb;
        private IInputProvider inputProvider;
        private MovementSystem movementSystem;
        
        private void Awake()
        {
            // Cache component references
            rb = GetComponent<Rigidbody>();
            
            // Validate and get input provider
            if (inputProviderComponent == null)
            {
                Debug.LogError("PlayerController: Input Provider is not assigned!");
                return;
            }
            
            inputProvider = inputProviderComponent as IInputProvider;
            if (inputProvider == null)
            {
                Debug.LogError("PlayerController: Input Provider does not implement IInputProvider interface!");
                return;
            }
            
            // Initialize movement system
            if (movementData == null)
            {
                Debug.LogError("PlayerController: Movement Data is not assigned!");
                return;
            }
            
            movementSystem = new MovementSystem(rb, movementData);
            
            // Find camera controller if not assigned
            if (cameraController == null)
            {
                cameraController = FindObjectOfType<CameraController>();
            }
            
            // Configure rigidbody
            ConfigureRigidbody();
        }
        
        private void Start()
        {
            // Enable input provider
            inputProvider?.Enable();
        }
        
        private void OnDisable()
        {
            // Disable input provider
            inputProvider?.Disable();
        }
        
        private void Update()
        {
            // Input is typically read in Update for responsiveness
            // Movement is applied in FixedUpdate for physics consistency
        }
        
        private void FixedUpdate()
        {
            if (inputProvider == null || movementSystem == null) return;
            
            // Get input
            InputData input = inputProvider.GetInput();
            
            // Process movement (physics-based, in FixedUpdate)
            movementSystem.ProcessMovement(input, Time.fixedDeltaTime);
        }
        
        private void LateUpdate()
        {
            if (inputProvider == null || cameraController == null) return;
            
            // Get input for camera
            InputData input = inputProvider.GetInput();
            
            // Update camera (in LateUpdate to ensure player has moved)
            cameraController.UpdateCamera(input);
        }
        
        /// <summary>
        /// Configures the Rigidbody component with appropriate settings for character control.
        /// </summary>
        private void ConfigureRigidbody()
        {
            if (rb == null) return;
            
            // Freeze rotation to prevent tumbling
            rb.freezeRotation = true;
            
            // Set appropriate drag (will be adjusted by movement system)
            rb.drag = 0f;
            rb.angularDrag = 0f;
        }
        
        /// <summary>
        /// Gets the current player state.
        /// </summary>
        /// <returns>Current player state.</returns>
        public PlayerState GetState()
        {
            return movementSystem?.GetState() ?? PlayerState.Airborne;
        }
    }
}

