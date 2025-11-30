using UnityEngine;

/// <summary>
/// Responsible for rotating the player to face the movement direction relative to camera.
/// </summary>
public class RotationHandler
{
    private readonly Transform playerTransform;
    private readonly Transform cameraTransform;
    private readonly float rotationSmoothTime;

    private float rotationVelocity;

    /// <summary>
    /// Initializes the RotationHandler with required dependencies.
    /// </summary>
    /// <param name="playerTransform">The player's Transform to rotate.</param>
    /// <param name="cameraTransform">The camera's Transform for camera-relative rotation.</param>
    /// <param name="rotationSmoothTime">Time it takes to smoothly rotate to target rotation (in seconds).</param>
    public RotationHandler(Transform playerTransform, Transform cameraTransform, float rotationSmoothTime = 0.12f)
    {
        this.playerTransform = playerTransform;
        this.cameraTransform = cameraTransform;
        this.rotationSmoothTime = rotationSmoothTime;
        rotationVelocity = 0f;
    }

    /// <summary>
    /// Rotates the player to face the movement direction relative to camera orientation.
    /// Should be called in Update() or LateUpdate().
    /// Only rotates when there is movement input.
    /// </summary>
    /// <param name="horizontalInput">Horizontal input (-1 to 1).</param>
    /// <param name="verticalInput">Vertical input (-1 to 1).</param>
    public void Rotate(float horizontalInput, float verticalInput)
    {
        if (playerTransform == null)
        {
            Debug.LogError("Player transform is null! Rotation will not work correctly.");
            return;
        }

        if (cameraTransform == null)
        {
            Debug.LogWarning("Camera transform is null! Rotation will not work correctly.");
            return;
        }

        // Normalize input direction (flattened to horizontal plane)
        Vector3 inputDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Only rotate when there is movement input
        if (horizontalInput != 0f || verticalInput != 0f)
        {
            // Calculate target rotation angle based on input direction relative to camera
            // Atan2 gives angle in radians, convert to degrees
            // Add camera's Y rotation to make it relative to camera orientation
            float targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;

            // Smoothly rotate towards target rotation using SmoothDampAngle
            // This prevents jittery rotation and provides smooth, natural-feeling rotation
            float currentRotation = Mathf.SmoothDampAngle(
                playerTransform.eulerAngles.y,
                targetRotation,
                ref rotationVelocity,
                rotationSmoothTime
            );

            // Apply rotation only on Y-axis (horizontal rotation)
            // X and Z rotations are locked to prevent player from tilting
            playerTransform.rotation = Quaternion.Euler(0f, currentRotation, 0f);
        }
    }
}

