using System;
using FlaxEngine;

namespace PhysicsFeaturesTour
{
    public class PlayerScript : Script
    {
        public CharacterController PlayerController;
        public Actor CameraTarget;
        public Camera Camera;

        public float CameraSmoothing = 20.0f;

        public bool CanJump = true;
        public float JumpForce = 800;

        public float Friction = 8.0f;
        public float GroundAccelerate = 5000;
        public float AirAccelerate = 10000;
        public float MaxVelocityGround = 400;
        public float MaxVelocityAir = 200;
        
        private Vector3 _velocity;
        private bool _jump;
        private float pitch;
        private float yaw;

        public bool CanShoot = false;
        public Model SphereModel;

        void Update()
        {
            Screen.CursorVisible = false;
            Screen.CursorLock = CursorLockMode.Locked;

            // Mouse
            Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            pitch = Mathf.Clamp(pitch + mouseDelta.Y, -88, 88);
            yaw += mouseDelta.X;

            // Jump
            if (CanJump && Input.GetAction("Jump"))
                _jump = true;

            // Shoot
            if (CanShoot && Input.GetAction("Fire"))
            {
                var ball = RigidBody.New();
                var ballCollider = SphereCollider.New();
                var ballModel = ModelActor.New();
                ballModel.Model = SphereModel;
                ballModel.Parent = ball;
                ballCollider.Parent = ball;
                ball.UseCCD = true;
                ball.Transform = new Transform(
                    CameraTarget.Position + Horizontal(CameraTarget.Direction) * 70.0f,
                    Quaternion.Identity,
                    new Vector3(0.1f));
                SceneManager.SpawnActor(ball);
                ball.LinearVelocity = CameraTarget.Direction * 600.0f;
                Destroy(ball, 5.0f);
            }
        }

        private Vector3 Horizontal(Vector3 v)
        {
            return new Vector3(v.X, 0, v.Z);
        }

        void FixedUpdate()
        {
            // Camera update
            var camTrans = Camera.Transform;
            var camFactor = Mathf.Clamp01(CameraSmoothing * Time.DeltaTime);
            CameraTarget.LocalOrientation = Quaternion.Lerp(CameraTarget.LocalOrientation, Quaternion.Euler(pitch, yaw, 0), camFactor);
            //CameraTarget.LocalOrientation = Quaternion.Euler(pitch, yaw, 0);
            camTrans.Translation = Vector3.Lerp(camTrans.Translation, CameraTarget.Position, camFactor);
            camTrans.Orientation = CameraTarget.Orientation;
            Camera.Transform = camTrans;

            var inputH = Input.GetAxis("Horizontal");
            var inputV = Input.GetAxis("Vertical");

            var velocity = new Vector3(inputH, 0.0f, inputV);
            velocity.Normalize();
            velocity = CameraTarget.Transform.TransformDirection(velocity);

            if (PlayerController.IsGrounded)
            {
                velocity = MoveGround(velocity.Normalized, Horizontal(_velocity));
                velocity.Y = -Mathf.Abs(Physics.Gravity.Y * 0.5f);
            }
            else
            {
                velocity = MoveAir(velocity.Normalized, Horizontal(_velocity));
                velocity.Y = _velocity.Y;
            }

            // Fix direction
            if (velocity.Length < 0.05f)
                velocity = Vector3.Zero;
            
            if (_jump && PlayerController.IsGrounded)
                velocity.Y = JumpForce;

            _jump = false;

            // Apply gravity
            velocity.Y += -Mathf.Abs(Physics.Gravity.Y * 2.5f) * Time.DeltaTime;

            // Check if player is not blocked by something above head
            if ((PlayerController.Flags & CharacterController.CollisionFlags.Above) != 0)
            {
                if (velocity.Y > 0)
                {
                    // player head hit something above,
                    // zero the gravity acceleration
                    velocity.Y = 0;
                }
            }

            // Move
            PlayerController.Move(velocity * Time.DeltaTime);
            _velocity = velocity;
        }

        // accelDir: normalized direction that the player has requested to move (taking into account the movement keys and look direction)
        // prevVelocity: The current velocity of the player, before any additional calculations
        // accelerate: The server-defined player acceleration value
        // max_velocity: The server-defined maximum player velocity (this is not strictly adhered to due to strafejumping)
        private Vector3 Accelerate(Vector3 accelDir, Vector3 prevVelocity, float accelerate, float max_velocity)
        {
            float projVel = Vector3.Dot(prevVelocity, accelDir); // Vector projection of Current velocity onto accelDir.
            float accelVel = accelerate * Time.DeltaTime; // Accelerated velocity in direction of movment

            // If necessary, truncate the accelerated velocity so the vector projection does not exceed max_velocity
            if (projVel + accelVel > max_velocity)
                accelVel = max_velocity - projVel;

            return prevVelocity + accelDir * accelVel;
        }

        private Vector3 MoveGround(Vector3 accelDir, Vector3 prevVelocity)
        {
            // Apply Friction
            float speed = prevVelocity.Length;
            if (Math.Abs(speed) > 0.01f) // To avoid divide by zero errors
            {
                float drop = speed * Friction * Time.DeltaTime;
                prevVelocity *= Mathf.Max(speed - drop, 0) / speed; // Scale the velocity based on friction.
            }

            // ground_accelerate and max_velocity_ground are server-defined movement variables
            return Accelerate(accelDir, prevVelocity, GroundAccelerate, MaxVelocityGround);
        }

        private Vector3 MoveAir(Vector3 accelDir, Vector3 prevVelocity)
        {
            // air_accelerate and max_velocity_air are server-defined movement variables
            return Accelerate(accelDir, prevVelocity, AirAccelerate, MaxVelocityAir);
        }

        private void OnDebugDraw()
        {
            var trans = Transform;
            var controller = (CharacterController)Actor;
            DebugDraw.DrawTube(trans.Translation, trans.Orientation * Quaternion.Euler(90, 0, 0), controller.Radius, controller.Height, Color.Blue);
        }
    }
}
