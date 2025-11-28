namespace PlayerController.Input
{
    /// <summary>
    /// Interface for input providers.
    /// Allows the movement system to work with different input implementations.
    /// </summary>
    public interface IInputProvider
    {
        /// <summary>
        /// Gets the current input data from the input system.
        /// </summary>
        /// <returns>InputData structure containing normalized input values.</returns>
        InputData GetInput();
        
        /// <summary>
        /// Enables the input provider.
        /// </summary>
        void Enable();
        
        /// <summary>
        /// Disables the input provider.
        /// </summary>
        void Disable();
    }
}

