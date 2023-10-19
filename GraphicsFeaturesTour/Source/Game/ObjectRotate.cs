using FlaxEngine;

namespace GraphicsFeaturesTour
{
    public class ObjectRotate : Script
    {
        private Quaternion _originalRotation;

        [Limit(-20, 20, 0.01f), Tooltip("Object rotation speed")]
        public float RotateSpeed = 1.0f;

        /// <inheritdoc />
        public override void OnEnable()
        {
            _originalRotation = Actor.Orientation;
        }

        /// <inheritdoc />
        public override void OnDisable()
        {
            Actor.Orientation = _originalRotation;
        }

        /// <inheritdoc />
        public override void OnFixedUpdate()
        {
            var euler = Actor.LocalEulerAngles;
            euler.Y += RotateSpeed * Time.DeltaTime * 20;
            Actor.LocalEulerAngles = euler;
        }
    }
}
