using UnityEngine;
using PlayerController.Input;
using PlayerController.Camera.Data;
using PlayerController.Events;

namespace PlayerController.Camera
{
    /// <summary>
    /// Third-person camera controller that follows and rotates around the player.
    /// Handles mouse look, camera follow, and collision detection.
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        [SerializeField] [Tooltip("Camera configuration data")]
        private CameraData cameraData;
        
        [SerializeField] [Tooltip("Target transform to follow (usually the player)")]
        private Transform target;
        
        private Camera cam;
        private float currentHorizontalAngle;
        private float currentVerticalAngle;
        private Vector3 currentOffset;
        
        private void Awake()
        {
            cam = GetComponent<Camera>();
            if (cam == null)
            {
                cam = gameObject.AddComponent<Camera>();
            }
            
            // Initialize angles
            currentHorizontalAngle = transform.eulerAngles.y;
            currentVerticalAngle = transform.eulerAngles.x;
            
            // Initialize offset
            if (cameraData != null)
            {
                currentOffset = cameraData.Offset;
            }
        }
        
        private void Start()
        {
            if (target == null)
            {
                Debug.LogWarning("CameraController: Target is not assigned!");
            }
            
            // Subscribe to player events for state-aware camera behavior
            PlayerEvents.OnGrounded += OnPlayerGrounded;
            PlayerEvents.OnAirborne += OnPlayerAirborne;
        }
        
        private void OnDestroy()
        {
            // Unsubscribe from events
            PlayerEvents.OnGrounded -= OnPlayerGrounded;
            PlayerEvents.OnAirborne -= OnPlayerAirborne;
        }
        
        private void LateUpdate()
        {
            if (target == null || cameraData == null) return;
            
            // Update camera rotation based on look input
            // (This would be called from the main controller with input data)
        }
        
        /// <summary>
        /// Updates the camera position and rotation based on input.
        /// </summary>
        /// <param name="inputData">Input data containing look input.</param>
        public void UpdateCamera(InputData inputData)
        {
            if (target == null || cameraData == null) return;
            
            // Update rotation angles
            currentHorizontalAngle += inputData.lookInput.x * cameraData.HorizontalSensitivity;
            currentVerticalAngle -= inputData.lookInput.y * cameraData.VerticalSensitivity;
            
            // Clamp vertical angle
            currentVerticalAngle = Mathf.Clamp(
                currentVerticalAngle,
                cameraData.MinVerticalAngle,
                cameraData.MaxVerticalAngle
            );
            
            // Calculate desired position
            Quaternion rotation = Quaternion.Euler(currentVerticalAngle, currentHorizontalAngle, 0f);
            Vector3 desiredPosition = target.position + rotation * cameraData.Offset;
            
            // Handle camera collision
            if (cameraData.EnableCollision)
            {
                desiredPosition = HandleCameraCollision(target.position, desiredPosition);
            }
            
            // Smoothly move camera to desired position
            transform.position = Vector3.Lerp(
                transform.position,
                desiredPosition,
                1f - cameraData.FollowSmoothness
            );
            
            // Look at target
            transform.LookAt(target);
        }
        
        /// <summary>
        /// Handles camera collision to prevent clipping through walls.
        /// </summary>
        private Vector3 HandleCameraCollision(Vector3 targetPosition, Vector3 desiredPosition)
        {
            Vector3 direction = desiredPosition - targetPosition;
            float distance = direction.magnitude;
            
            if (Physics.SphereCast(
                targetPosition,
                cameraData.CollisionRadius,
                direction.normalized,
                out RaycastHit hit,
                distance,
                cameraData.CollisionLayerMask
            ))
            {
                // Adjust position to avoid collision
                return hit.point + hit.normal * cameraData.CollisionRadius;
            }
            
            return desiredPosition;
        }
        
        /// <summary>
        /// Called when the player becomes grounded.
        /// </summary>
        private void OnPlayerGrounded()
        {
            // Camera behavior adjustments for grounded state
        }
        
        /// <summary>
        /// Called when the player becomes airborne.
        /// </summary>
        private void OnPlayerAirborne()
        {
            // Camera behavior adjustments for airborne state
        }
    }
}

