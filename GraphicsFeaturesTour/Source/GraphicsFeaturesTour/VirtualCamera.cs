using System;
using FlaxEngine;

/// <summary>
/// Script for camera movement and rotation control via virtual thumb sticks on a screen (for mobile platforms).
/// </summary>
public class VirtualCamera : Script
{
    public PlayerScript Camera;
    public UIControl VirtualThumbStick_Left;
    public UIControl VirtualThumbStick_Right;
    public float Strength_Left = 1.0f;
    public float Strength_Right = 1.0f;
    public bool InvertLookUp = false;

    /// <inheritdoc />
    public override void OnEnable()
    {
        if (VirtualThumbStick_Left != null && !VirtualThumbStick_Left.Is<VirtualThumbStick>())
            throw new ArgumentException("Invalid VirtualThumbStick_Left");
        if (VirtualThumbStick_Right != null && !VirtualThumbStick_Right.Is<VirtualThumbStick>())
            throw new ArgumentException("Invalid VirtualThumbStick_Right");
    }

    /// <inheritdoc />
    public override void OnUpdate()
    {
        var movement = Vector2.Zero;
        var rotation = Vector2.Zero;

        if (VirtualThumbStick_Left != null)
        {
            movement = VirtualThumbStick_Left.Get<VirtualThumbStick>().Input * Strength_Left;
        }

        if (VirtualThumbStick_Right != null)
        {
            rotation = VirtualThumbStick_Right.Get<VirtualThumbStick>().Input * Strength_Right;
        }

        if (!InvertLookUp)
            rotation.Y *= -1;
        Camera?.AddMovementRotation(movement.X, movement.Y, rotation.Y, rotation.X);
    }
}
