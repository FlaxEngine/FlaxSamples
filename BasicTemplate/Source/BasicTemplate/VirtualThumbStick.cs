using FlaxEngine;
using FlaxEngine.GUI;

public class VirtualThumbStick : Control
{
    private bool _isPressed;
    private Vector2 _input;
    private Vector2 _inputLocation;

    [Tooltip("If checked, the control will auto-size based on the screen size")]
    public bool AutoSize = true;

    [EditorDisplay("Style"), EditorOrder(2000)]
    public IBrush CircleBrush { get; set; }

    [EditorDisplay("Style"), EditorOrder(2010)]
    public Color Color { get; set; } = new Color(1, 1, 1, 0.30555f);

    [EditorDisplay("Style"), EditorOrder(2020)]
    public Color ColorPressed { get; set; } = Color.White;

    /// <summary>
    /// Gets a value indicating whether this thumb is being pressed (by mouse or touch).
    /// </summary>
    public bool IsPressed => _isPressed;

    /// <summary>
    /// Gets the thumb input (2D direction).
    /// </summary>
    public Vector2 Input => _input;

    /// <summary>
    /// Initializes a new instance of the <see cref="VirtualThumbStick"/> class.
    /// </summary>
    public VirtualThumbStick()
    {
    }

    /// <summary>
    /// Called when thumb starts to be pressed by the used (via mouse or touch).
    /// </summary>
    /// <param name="location">The input position (in control space).</param>
    protected virtual void OnPressBegin(Vector2 location)
    {
        _isPressed = true;
        if (AutoFocus)
            Focus();
        OnPressMove(location);
    }

    /// <summary>
    /// Called when thumb input moves but continues to be pressed (via mouse or touch).
    /// </summary>
    /// <param name="location">The input position (in control space).</param>
    protected virtual void OnPressMove(Vector2 location)
    {
        var sizeHalf = Size * 0.5f;
        var dir = location - sizeHalf;
        var intensity = dir.Length / sizeHalf.Length;
        var dirNormalized = Vector2.Normalize(dir);
        _inputLocation = sizeHalf + dirNormalized * sizeHalf * intensity;
        _input = new Vector2(location.X - sizeHalf.X, sizeHalf.Y - location.Y) / sizeHalf;
    }

    /// <summary>
    /// Called when thumb ends to be pressed by the used (via mouse or touch).
    /// </summary>
    protected virtual void OnPressEnd()
    {
        _isPressed = false;
        _input = Vector2.Zero;
    }
    
    /// <inheritdoc />
    public override void PerformLayout(bool force = false)
    {
        base.PerformLayout(force);

        if (AutoSize && Parent != null)
        {
            var anchorPreset = AnchorPreset;
            Size = new Vector2(Parent.Size.MinValue * 0.4f);
            SetAnchorPreset(anchorPreset, false);
        }
    }

    /// <inheritdoc />
    public override void Draw()
    {
        var size = Size;
        var enabled = EnabledInHierarchy;
        var circleColor = _isPressed ? ColorPressed : Color;
        if (!enabled)
            circleColor *= 0.5f;
        var pressedScale = 0.3f;
        var normalSize = 0.5f;
        var circleBounds = _isPressed ? new Rectangle(Vector2.Clamp(_inputLocation, size * (pressedScale / 2), size * (1 - pressedScale / 2)) - size * (pressedScale / 2), size * pressedScale) : new Rectangle(size * (0.5f - normalSize * 0.5f), size * normalSize);
        CircleBrush?.Draw(circleBounds, circleColor);
    }

    /// <inheritdoc />
    public override void OnMouseLeave()
    {
        if (_isPressed)
        {
            OnPressEnd();
        }

        base.OnMouseLeave();
    }

    /// <inheritdoc />
    public override bool OnMouseDown(Vector2 location, MouseButton button)
    {
        if (button == MouseButton.Left && !_isPressed)
        {
            OnPressBegin(location);
            return true;
        }

        return base.OnMouseDown(location, button);
    }

    /// <inheritdoc />
    public override void OnMouseMove(Vector2 location)
    {
        if (_isPressed)
        {
            OnPressMove(location);
        }

        base.OnMouseMove(location);
    }

    /// <inheritdoc />
    public override bool OnMouseUp(Vector2 location, MouseButton button)
    {
        if (button == MouseButton.Left && _isPressed)
        {
            OnPressEnd();
            return true;
        }

        return base.OnMouseUp(location, button);
    }

    /// <inheritdoc />
    public override bool OnTouchDown(Vector2 location, int pointerId)
    {
        if (!_isPressed)
        {
            OnPressBegin(location);
            return true;
        }

        return base.OnTouchDown(location, pointerId);
    }

    /// <inheritdoc />
    public override void OnTouchMove(Vector2 location, int pointerId)
    {
        if (_isPressed)
        {
            OnPressMove(location);
        }

        base.OnTouchMove(location, pointerId);
    }

    /// <inheritdoc />
    public override bool OnTouchUp(Vector2 location, int pointerId)
    {
        if (_isPressed)
        {
            OnPressEnd();
            return true;
        }

        return base.OnTouchUp(location, pointerId);
    }

    /// <inheritdoc />
    public override void OnTouchLeave()
    {
        if (_isPressed)
        {
            OnPressEnd();
        }

        base.OnTouchLeave();
    }
}
