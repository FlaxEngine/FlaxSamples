using FlaxEngine;

namespace Game
{
    /// <summary>
    /// A basic character script that supports movement, camera handling, jumping and sprinting.
    /// </summary>
    public class Character : Script
    {
        private const string keyGroup = "Key Input";
        private const string movementGroup = "Movement";
        private const string cameraGroup = "Camera";

        // Keys
        [ExpandGroups]

        [Tooltip("Key for jumping"), EditorDisplay(keyGroup, "Jump"), EditorOrder(0)]
        public KeyboardKeys JumpKey = KeyboardKeys.Spacebar;

        [Tooltip("Key for sprinting"), EditorDisplay(keyGroup, "Sprint"), EditorOrder(1)]
        public KeyboardKeys SprintKey = KeyboardKeys.Shift;

        // Movement
        [ExpandGroups]

        [Tooltip("The character model"), EditorDisplay(movementGroup, "Character"), EditorOrder(2)]
        public Actor CharacterObj { get; set; } = null;

        [Range(0f, 300f), Tooltip("Movement speed factor"), EditorDisplay(movementGroup, "Speed"), EditorOrder(3)]
        public float Speed { get; set; } = 250;

        [Range(0f, 300f), Tooltip("Movement speed factor"), EditorDisplay(movementGroup, "Sprint Speed"), EditorOrder(4)]
        public float SprintSpeed { get; set; } = 300;

        [Limit(-20f, 20f), Tooltip("Gravity of this character"), EditorDisplay(movementGroup, "Gravity"), EditorOrder(5)]
        public float Gravity { get; set; } = -9.81f;

        [Range(0f, 25f), Tooltip("Jump factor"), EditorDisplay(movementGroup, "Jump Strength"), EditorOrder(6)]
        public float JumpStrength { get; set; } = 10;

        // Camera
        [ExpandGroups]

        [Tooltip("The camera view for player"), EditorDisplay(cameraGroup, "Camera View"), EditorOrder(8)]
        public Camera CameraView { get; set; } = null;

        [Range(0, 10f), Tooltip("Sensitivity of the mouse"), EditorDisplay(cameraGroup, "Mouse Sensitivity"), EditorOrder(9)]
        public float MouseSensitivity { get; set; } = 100f;

        [Range(0f, 20f), Tooltip("Lag of the camera, lower = slower"), EditorDisplay(cameraGroup, "Camera Lag"), EditorOrder(10)]
        public float CameraLag { get; set; } = 10;

        [Range(0f, 100f), Tooltip("How far to zoom in, lower = closer"), EditorDisplay(cameraGroup, "FOV Zoom"), EditorOrder(11)]
        public float FOVZoom { get; set; } = 50;

        [Tooltip("Determines the min and max pitch value for the camera"), EditorDisplay(cameraGroup, "Pitch Min Max"), EditorOrder(12)]
        public Vector2 PitchMinMax { get; set; } = new Vector2(-45, 45);


        private CharacterController _controller;
        private Vector3 _velocity;

        private float _yaw;
        private float _pitch;
        private float _origFOV;

        public override void OnStart()
        {
            // Get Controller, since its the parent we just need to cast
            _controller = (CharacterController)Parent;

            if (!CameraView || !CharacterObj)
            {
                Debug.LogError("No Character or Camera assigned!");
                return;
            }

            _origFOV = CameraView.FieldOfView;
        }

        public override void OnUpdate()
        {
            // Currently has a bug, as usual its related to the DPI!
            Screen.CursorLock = CursorLockMode.Locked;
            Screen.CursorVisible = false;
        }

        public override void OnFixedUpdate()
        {
            // Camera Rotation
            {
                // Get mouse axis values and clamp pitch.
                _yaw += Input.GetAxis("Mouse X") * MouseSensitivity * Time.DeltaTime; // H
                _pitch += Input.GetAxis("Mouse Y") * MouseSensitivity * Time.DeltaTime; // V
                _pitch = Mathf.Clamp(_pitch, PitchMinMax.X, PitchMinMax.Y);

                // The camera's parent should be another actor, like a spring arm for instance.
                CameraView.Parent.Orientation = Quaternion.Lerp(CameraView.Parent.Orientation, Quaternion.Euler(_pitch, _yaw, 0), Time.DeltaTime * CameraLag);
                CharacterObj.Orientation = Quaternion.Euler(0, _yaw, 0);

                // When right clicking, zoom in or out
                if (Input.GetMouseButton(MouseButton.Right))
                {
                    CameraView.FieldOfView = Mathf.Lerp(CameraView.FieldOfView, FOVZoom, Time.DeltaTime * 5f);
                }
                else
                {
                    CameraView.FieldOfView = Mathf.Lerp(CameraView.FieldOfView, _origFOV, Time.DeltaTime * 5f);
                }
            }

            // Character Movement
            {
                // If the character is grounded we just set it to 0, avoiding constantly adding to Y;
                if (_controller.IsGrounded)
                {
                    _velocity.Y = 0f;
                }

                // Get input axis
                var inputH = Input.GetAxis("Horizontal");
                var inputV = Input.GetAxis("Vertical");

                // Apply movement towards the camera direction
                var movement = new Vector3(inputH, 0.0f, inputV);
                var movementDirection = CameraView.Transform.TransformDirection(movement);

                // Jump if the space bar is down, jump.
                if (Input.GetKeyDown(JumpKey) && _controller.IsGrounded)
                {
                    _velocity.Y = Mathf.Sqrt(JumpStrength * -2f * Gravity);
                }

                // Apply gravity
                _velocity.Y += Gravity * Time.DeltaTime;
                movementDirection += (_velocity * 0.5f);

                // Apply controller movement, evaluate whether we are sprinting or not.
                _controller.Move(movementDirection * Time.DeltaTime * (Input.GetKey(SprintKey) ? SprintSpeed : Speed));
            }
        }
    }
}
