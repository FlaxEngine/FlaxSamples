using FlaxEngine;

namespace BasicTemplate
{
    public class FreeCamera : Script
    {
        [Limit(0, 100), Tooltip("Camera movement speed factor")]
        public float MoveSpeed { get; set; }

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

            var inputH = Input.GetAxis("Horizontal");
            var inputV = Input.GetAxis("Vertical");
            var move = new Vector3(inputH, 0.0f, inputV);
            move.Normalize();
            move = camTrans.TransformDirection(move);
            
            camTrans.Orientation = Quaternion.Euler(pitch, yaw, 0);
            camTrans.Translation += move * MoveSpeed;
            
            Actor.Transform = camTrans;
        }
    }
}
