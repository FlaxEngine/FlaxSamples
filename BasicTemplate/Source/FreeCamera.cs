using FlaxEngine;

namespace BasicTemplate
{
    public class FreeCamera : Script
    {
        [Limit(0, 100), Tooltip("Camera movement speed factor")]
        public float MoveSpeed { get; set; }

        [Tooltip("Camera rotation smoothing factor")]
        public float CameraSmoothing { get; set; } = 20.0f;

        private float pitch;
        private float yaw;

        FreeCamera()
        {
            MoveSpeed = 1;
        }

        void Update()
        {
            Screen.CursorVisible = false;
            Screen.CursorLock = CursorLockMode.Locked;

            Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            pitch = Mathf.Clamp(pitch + mouseDelta.Y, -88, 88);
            yaw += mouseDelta.X;
        }

        void FixedUpdate()
        {
            var camTrans = Actor.Transform;
            var camFactor = Mathf.Clamp01(CameraSmoothing * Time.DeltaTime);

            camTrans.Orientation = Quaternion.Lerp(camTrans.Orientation, Quaternion.Euler(pitch, yaw, 0), camFactor);

            var inputH = Input.GetAxis("Horizontal");
            var inputV = Input.GetAxis("Vertical");
            var move = new Vector3(inputH, 0.0f, inputV);
            move.Normalize();
            move = camTrans.TransformDirection(move);

            camTrans.Translation += move * MoveSpeed;

            Actor.Transform = camTrans;
        }
    }
}