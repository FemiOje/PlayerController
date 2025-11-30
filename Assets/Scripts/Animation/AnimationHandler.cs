using UnityEngine;

/// <summary>
/// Responsible for updating the Animator component with player state information.
/// </summary>
public class AnimationHandler
{
    private readonly Animator animator;
    private readonly bool hasAnimator;

    // Cached animation parameter IDs for performance
    private readonly int animIDSpeed;
    private readonly int animIDGrounded;
    private readonly int animIDJump;
    private readonly int animIDFreeFall;
    private readonly int animIDMotionSpeed;

    // Animation blend value for smooth speed transitions
    private float animationBlend;
    private readonly float speedChangeRate;

    /// <summary>
    /// Initializes the AnimationHandler with required dependencies.
    /// </summary>
    /// <param name="animator">The Animator component to update. Can be null if no animator is present.</param>
    /// <param name="speedChangeRate">Rate at which animation blend transitions (for smooth speed changes).</param>
    public AnimationHandler(Animator animator, float speedChangeRate = 10.0f)
    {
        this.animator = animator;
        this.hasAnimator = animator != null;
        this.speedChangeRate = speedChangeRate;
        this.animationBlend = 0f;

        // Cache animation parameter IDs using StringToHash for performance
        // These match the parameter names in the Animator Controller
        animIDSpeed = Animator.StringToHash("Speed");
        animIDGrounded = Animator.StringToHash("Grounded");
        animIDJump = Animator.StringToHash("Jump");
        animIDFreeFall = Animator.StringToHash("FreeFall");
        animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

    /// <summary>
    /// Updates the Speed and MotionSpeed animation parameters based on current movement state.
    /// Should be called every frame in Update() or FixedUpdate().
    /// </summary>
    /// <param name="currentSpeed">Current horizontal movement speed (magnitude of horizontal velocity).</param>
    /// <param name="targetSpeed">Target movement speed (based on input and sprint state).</param>
    /// <param name="inputMagnitude">Magnitude of movement input (0 to 1). Used for MotionSpeed parameter.</param>
    public void UpdateMovementAnimation(float currentSpeed, float targetSpeed, float inputMagnitude = 1.0f)
    {
        if (!hasAnimator) return;

        // Smoothly blend animation speed for organic transitions
        // This creates curved speed changes rather than linear ones
        animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * speedChangeRate);

        // Update Speed parameter (used by blend trees for walk/run transitions)
        animator.SetFloat(animIDSpeed, animationBlend);

        // Update MotionSpeed parameter (controls animation playback speed)
        animator.SetFloat(animIDMotionSpeed, inputMagnitude);
    }

    /// <summary>
    /// Updates the Grounded animation parameter.
    /// Should be called every frame in Update().
    /// </summary>
    /// <param name="isGrounded">Whether the player is currently grounded.</param>
    public void UpdateGroundedAnimation(bool isGrounded)
    {
        if (!hasAnimator) return;

        animator.SetBool(animIDGrounded, isGrounded);
    }

    /// <summary>
    /// Triggers the Jump animation.
    /// Should be called when a jump is initiated.
    /// </summary>
    public void TriggerJumpAnimation()
    {
        if (!hasAnimator) return;

        animator.SetBool(animIDJump, true);
    }

    /// <summary>
    /// Resets the Jump animation parameter.
    /// Should be called when landing or when jump animation should stop.
    /// </summary>
    public void ResetJumpAnimation()
    {
        if (!hasAnimator) return;

        animator.SetBool(animIDJump, false);
    }

    /// <summary>
    /// Updates the FreeFall animation parameter.
    /// Should be called when the player is in the air and falling.
    /// </summary>
    /// <param name="isFreeFalling">Whether the player is currently free falling (not grounded and falling).</param>
    public void UpdateFreeFallAnimation(bool isFreeFalling)
    {
        if (!hasAnimator) return;

        animator.SetBool(animIDFreeFall, isFreeFalling);
    }
}

