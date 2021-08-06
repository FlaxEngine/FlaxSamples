using FlaxEngine;

public class FreeCamera : Script
{
    [Limit(0, 100), Tooltip("Camera movement speed factor")]
    public float MoveSpeed { get; set; } = 4;

    [Tooltip("Camera rotation smoothing factor")]
    public float CameraSmoothing { get; set; } = 20.0f;

    [Tooltip("Camera mouse movement sensitivity")]
    public float Sensitivity { get; set; } = 1.0f;

    public bool UseMouse = true;

    private float _pitch;
    private float _yaw;

    /// <summary>
    /// Adds the rotation to the camera (as input).
    /// </summary>
    /// <param name="pitch">The pitch rotation input.</param>
    /// <param name="yaw">The yaw rotation input.</param>
    public void AddRotation(float pitch, float yaw)
    {
        _pitch += pitch;
        _yaw += yaw;
    }

    /// <summary>
    /// Adds the movement to the camera (as input).
    /// </summary>
    /// <param name="horizontal">The horizontal input.</param>
    /// <param name="vertical">The vertical input.</param>
    public void AddMovement(float horizontal, float vertical)
    {
        var camTrans = Actor.Transform;
        var move = new Vector3(horizontal, 0.0f, vertical);
        move.Normalize();
        move = camTrans.TransformDirection(move);
        camTrans.Translation += move * MoveSpeed;
        Actor.Position = camTrans.Translation;
    }

    /// <summary>
    /// Adds the movement and rotation to the camera (as input).
    /// </summary>
    /// <param name="horizontal">The horizontal input.</param>
    /// <param name="vertical">The vertical input.</param>
    /// <param name="pitch">The pitch rotation input.</param>
    /// <param name="yaw">The yaw rotation input.</param>
    public void AddMovementRotation(float horizontal, float vertical, float pitch, float yaw)
    {
        _pitch += pitch;
        _yaw += yaw;

        var camTrans = Actor.Transform;
        var camFactor = Mathf.Saturate(CameraSmoothing * Time.DeltaTime);
        camTrans.Orientation = Quaternion.Lerp(camTrans.Orientation, Quaternion.Euler(_pitch, _yaw, 0), camFactor);

        var move = new Vector3(horizontal, 0.0f, vertical);
        move.Normalize();
        move = camTrans.TransformDirection(move);
        camTrans.Translation += move * MoveSpeed;
        Actor.Position = camTrans.Translation;
    }

    /// <inheritdoc />
    public override void OnStart()
    {
        var initialEulerAngles = Actor.Orientation.EulerAngles;
        _pitch = initialEulerAngles.X;
        _yaw = initialEulerAngles.Y;
    }

    /// <inheritdoc />
    public override void OnUpdate()
    {
        if (UseMouse)
        {
            Screen.CursorVisible = false;
            Screen.CursorLock = CursorLockMode.Locked;

            Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            mouseDelta *= Sensitivity;
            _pitch = Mathf.Clamp(_pitch + mouseDelta.Y, -88, 88);
            _yaw += mouseDelta.X;
        }
    }

    /// <inheritdoc />
    public override void OnFixedUpdate()
    {
        var camTrans = Actor.Transform;
        var camFactor = Mathf.Saturate(CameraSmoothing * Time.DeltaTime);
        camTrans.Orientation = Quaternion.Lerp(camTrans.Orientation, Quaternion.Euler(_pitch, _yaw, 0), camFactor);

        var inputH = Input.GetAxis("Horizontal");
        var inputV = Input.GetAxis("Vertical");
        var move = new Vector3(inputH, 0.0f, inputV);
        move.Normalize();
        move = camTrans.TransformDirection(move);
        camTrans.Translation += move * MoveSpeed;
        Actor.Transform = camTrans;
    }
}
