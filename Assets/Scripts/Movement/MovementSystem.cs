using UnityEngine;
using PlayerController.Core;
using PlayerController.Input;
using PlayerController.Movement.Data;
using PlayerController.Movement.Behaviors;
using PlayerController.Physics;
using PlayerController.Events;

namespace PlayerController.Movement
{
    /// <summary>
    /// Core movement system that handles physics-based player movement.
    /// Coordinates ground detection, jumping, and movement behaviors.
    /// </summary>
    public class MovementSystem
    {
        private readonly Rigidbody rigidbody;
        private readonly Transform transform;
        private readonly MovementData movementData;
        
        private readonly GroundDetector groundDetector;
        private readonly JumpHandler jumpHandler;
        
        private readonly WalkBehavior walkBehavior;
        private readonly SprintBehavior sprintBehavior;
        private readonly AirControlBehavior airControlBehavior;
        
        private PlayerState currentState;
        
        /// <summary>
        /// Initializes a new instance of the MovementSystem.
        /// </summary>
        /// <param name="rigidbody">The player's Rigidbody component.</param>
        /// <param name="movementData">Movement configuration data.</param>
        public MovementSystem(Rigidbody rigidbody, MovementData movementData)
        {
            this.rigidbody = rigidbody;
            this.transform = rigidbody.transform;
            this.movementData = movementData;
            
            // Initialize subsystems
            groundDetector = new GroundDetector(transform, movementData);
            jumpHandler = new JumpHandler(rigidbody, movementData, groundDetector);
            
            // Initialize movement behaviors
            walkBehavior = new WalkBehavior(movementData.WalkSpeed);
            sprintBehavior = new SprintBehavior(movementData.WalkSpeed, movementData.SprintMultiplier);
            airControlBehavior = new AirControlBehavior(movementData.WalkSpeed, movementData.AirControl);
            
            currentState = PlayerState.Airborne;
        }
        
        /// <summary>
        /// Processes movement input and applies physics-based movement.
        /// </summary>
        /// <param name="inputData">Input data from the input system.</param>
        /// <param name="deltaTime">Time delta for frame-independent movement.</param>
        public void ProcessMovement(InputData inputData, float deltaTime)
        {
            // Update state
            UpdateState();
            
            // Process jump
            jumpHandler.ProcessJump(inputData.jumpDown, inputData.jumpPressed, IsGrounded());
            
            // Apply drag based on state
            ApplyDrag();
            
            // Calculate and apply movement
            Vector3 moveDirection = CalculateMoveDirection(inputData.moveInput);
            Vector3 movementForce = CalculateMovementForce(moveDirection, inputData.sprintHeld);
            
            // Apply movement force
            rigidbody.AddForce(movementForce, ForceMode.VelocityChange);
        }
        
        /// <summary>
        /// Updates the current player state based on ground detection.
        /// </summary>
        private void UpdateState()
        {
            bool isGrounded = IsGrounded();
            
            if (isGrounded && currentState != PlayerState.Grounded)
            {
                currentState = PlayerState.Grounded;
                PlayerEvents.InvokeGrounded();
            }
            else if (!isGrounded && currentState == PlayerState.Grounded)
            {
                currentState = PlayerState.Airborne;
            }
        }
        
        /// <summary>
        /// Checks if the player is currently grounded.
        /// </summary>
        private bool IsGrounded()
        {
            return groundDetector.IsGrounded();
        }
        
        /// <summary>
        /// Calculates the movement direction in world space.
        /// </summary>
        private Vector3 CalculateMoveDirection(Vector2 input)
        {
            // Get forward and right vectors relative to the transform
            Vector3 forward = transform.forward;
            Vector3 right = transform.right;
            
            // Calculate movement direction
            Vector3 moveDirection = (forward * input.y + right * input.x).normalized;
            
            // Project onto ground plane if grounded
            if (IsGrounded() && groundDetector.GetGroundNormal(out Vector3 normal))
            {
                moveDirection = PhysicsUtilities.ProjectOnPlane(moveDirection, normal);
            }
            
            return moveDirection;
        }
        
        /// <summary>
        /// Calculates the movement force based on current state and input.
        /// </summary>
        private Vector3 CalculateMovementForce(Vector3 moveDirection, bool sprinting)
        {
            Vector3 force = Vector3.zero;
            
            if (currentState == PlayerState.Grounded)
            {
                // Use walk or sprint behavior
                force = sprinting 
                    ? sprintBehavior.CalculateForce(moveDirection, Time.fixedDeltaTime)
                    : walkBehavior.CalculateForce(moveDirection, Time.fixedDeltaTime);
                
                // Update sprint events
                if (sprinting)
                {
                    PlayerEvents.InvokeSprintStart();
                }
                else
                {
                    PlayerEvents.InvokeSprintEnd();
                }
            }
            else if (currentState == PlayerState.Airborne)
            {
                // Use air control behavior
                force = airControlBehavior.CalculateForce(moveDirection, Time.fixedDeltaTime);
            }
            
            return force;
        }
        
        /// <summary>
        /// Applies drag based on the current state.
        /// </summary>
        private void ApplyDrag()
        {
            rigidbody.drag = currentState == PlayerState.Grounded 
                ? movementData.GroundDrag 
                : movementData.AirDrag;
        }
        
        /// <summary>
        /// Gets the current player state.
        /// </summary>
        public PlayerState GetState()
        {
            return currentState;
        }
    }
}

