using FlaxEngine;

namespace GraphicsFeaturesTour
{
    public class SkyRotate : Script
    {
        private Quaternion _originalRotation;

        [Tooltip("Directional light to animate when player enters trigger area")]
        public DirectionalLight Light;

        [Limit(-20, 20, 0.01f), Tooltip("Sun rotation speed")]
        public float RotateSpeed = 4.0f;

        private bool _isPlayerInside;

        public override void OnEnable()
        {
            if (Light)
                _originalRotation = Light.Orientation;

            Actor.As<Collider>().TriggerEnter += OnTriggerEnter;
            Actor.As<Collider>().TriggerExit += OnTriggerExit;
        }

        public override void OnDisable()
        {
            if (Light)
                Light.Orientation = _originalRotation;

            Actor.As<Collider>().TriggerEnter -= OnTriggerEnter;
            Actor.As<Collider>().TriggerExit -= OnTriggerExit;
        }

        public override void OnFixedUpdate()
        {
            if (_isPlayerInside)
            {
                // Rotate directional light
                var euler = Light.EulerAngles;
                euler.Y += RotateSpeed * Time.DeltaTime * 20;
                Light.EulerAngles = euler;
            }
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (Light && collider is CharacterController)
            {
                _isPlayerInside = true;
            }
        }

        private void OnTriggerExit(Collider collider)
        {
            if (Light && collider is CharacterController)
            {
                _isPlayerInside = false;
                Light.Orientation = _originalRotation;
            }
        }
    }
}