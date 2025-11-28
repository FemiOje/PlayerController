using UnityEngine;

namespace PlayerController.Camera.Data
{
    /// <summary>
    /// Configuration data for camera parameters.
    /// Create instances via: Create > PlayerController > Camera Data
    /// </summary>
    [CreateAssetMenu(fileName = "New Camera Data", 
                     menuName = "PlayerController/Camera Data")]
    public class CameraData : ScriptableObject
    {
        [Header("Position")]
        [Tooltip("Camera offset from player position")]
        [SerializeField]
        private Vector3 offset = new Vector3(0f, 2f, -5f);
        
        [Tooltip("Camera follow smoothness (lower = smoother)")]
        [SerializeField]
        [Range(0.1f, 1f)]
        private float followSmoothness = 0.1f;
        
        [Header("Rotation")]
        [Tooltip("Mouse sensitivity for horizontal rotation")]
        [SerializeField]
        private float horizontalSensitivity = 2f;
        
        [Tooltip("Mouse sensitivity for vertical rotation")]
        [SerializeField]
        private float verticalSensitivity = 2f;
        
        [Tooltip("Minimum vertical rotation angle (degrees)")]
        [SerializeField]
        private float minVerticalAngle = -30f;
        
        [Tooltip("Maximum vertical rotation angle (degrees)")]
        [SerializeField]
        private float maxVerticalAngle = 60f;
        
        [Header("Collision")]
        [Tooltip("Enable camera collision detection")]
        [SerializeField]
        private bool enableCollision = true;
        
        [Tooltip("Camera collision radius")]
        [SerializeField]
        private float collisionRadius = 0.3f;
        
        [Tooltip("Layer mask for camera collision")]
        [SerializeField]
        private LayerMask collisionLayerMask = 1;
        
        // Public properties for read-only access
        public Vector3 Offset => offset;
        public float FollowSmoothness => followSmoothness;
        public float HorizontalSensitivity => horizontalSensitivity;
        public float VerticalSensitivity => verticalSensitivity;
        public float MinVerticalAngle => minVerticalAngle;
        public float MaxVerticalAngle => maxVerticalAngle;
        public bool EnableCollision => enableCollision;
        public float CollisionRadius => collisionRadius;
        public LayerMask CollisionLayerMask => collisionLayerMask;
    }
}

