using UnityEngine;

namespace PlayerController.Movement.Data
{
    /// <summary>
    /// Configuration data for player movement parameters.
    /// Create instances via: Create > PlayerController > Movement Data
    /// </summary>
    [CreateAssetMenu(fileName = "New Movement Data", 
                     menuName = "PlayerController/Movement Data")]
    public class MovementData : ScriptableObject
    {
        [Header("Movement")]
        [Tooltip("Base walking speed in units per second")]
        [SerializeField]
        private float walkSpeed = 5f;
        
        [Tooltip("Sprint speed multiplier")]
        [SerializeField]
        private float sprintMultiplier = 1.5f;
        
        [Tooltip("Air control multiplier (0 = no control, 1 = full control)")]
        [SerializeField]
        [Range(0f, 1f)]
        private float airControl = 0.3f;
        
        [Header("Jump")]
        [Tooltip("Jump force applied to rigidbody")]
        [SerializeField]
        private float jumpForce = 8f;
        
        [Tooltip("Coyote time in seconds (grace period after leaving ground)")]
        [SerializeField]
        private float coyoteTime = 0.2f;
        
        [Tooltip("Maximum number of consecutive jumps allowed")]
        [SerializeField]
        private int maxJumps = 1;
        
        [Header("Ground Detection")]
        [Tooltip("Distance to check for ground")]
        [SerializeField]
        private float groundCheckDistance = 0.1f;
        
        [Tooltip("Layer mask for ground detection")]
        [SerializeField]
        private LayerMask groundLayerMask = 1;
        
        [Header("Physics")]
        [Tooltip("Drag applied when grounded")]
        [SerializeField]
        private float groundDrag = 5f;
        
        [Tooltip("Drag applied when airborne")]
        [SerializeField]
        private float airDrag = 0f;
        
        // Public properties for read-only access
        public float WalkSpeed => walkSpeed;
        public float SprintMultiplier => sprintMultiplier;
        public float AirControl => airControl;
        public float JumpForce => jumpForce;
        public float CoyoteTime => coyoteTime;
        public int MaxJumps => maxJumps;
        public float GroundCheckDistance => groundCheckDistance;
        public LayerMask GroundLayerMask => groundLayerMask;
        public float GroundDrag => groundDrag;
        public float AirDrag => airDrag;
    }
}

