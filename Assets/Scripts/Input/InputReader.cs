using UnityEngine;

/// <summary>
/// Responsible for reading all player input.
/// </summary>
public class InputReader
{
    // Input values (read-only)
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }
    public float MouseX { get; private set; }
    public float MouseY { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool JumpDown { get; private set; }
    public bool SprintPressed { get; private set; }

    /// <summary>
    /// Reads all input from Unity's Input system.
    /// Should be called every frame in Update().
    /// </summary>
    public void ReadInput()
    {
        Horizontal = Input.GetAxis("Horizontal");
        Vertical = Input.GetAxis("Vertical");
        MouseX = Input.GetAxis("Mouse X");
        MouseY = Input.GetAxis("Mouse Y");
        JumpPressed = Input.GetKey(KeyCode.Space);
        JumpDown = Input.GetKeyDown(KeyCode.Space);
        SprintPressed = Input.GetKey(KeyCode.LeftShift);
    }

    /// <summary>
    /// Gets the movement input as a Vector2.
    /// </summary>
    public Vector2 GetMoveInput()
    {
        return new Vector2(Horizontal, Vertical);
    }

    /// <summary>
    /// Gets the look input as a Vector2.
    /// </summary>
    public Vector2 GetLookInput()
    {
        return new Vector2(MouseX, MouseY);
    }
}

