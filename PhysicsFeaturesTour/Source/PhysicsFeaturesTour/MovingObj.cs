using FlaxEngine;

namespace PhysicsFeaturesTour
{
    public class MovingObj : Script
    {
        private Vector3 _startPos;
        private float _startTime;

        public Vector3 MoveAxis = Vector3.UnitX;

        public float Speed = 1;

        public float Amplitude = 50;

        /// <inheritdoc />
        public override void OnEnable()
        {
            _startTime = Time.GameTime;
            _startPos = Actor.LocalPosition;
        }

        /// <inheritdoc />
        public override void OnDisable()
        {
            Actor.LocalPosition = _startPos;
        }

        /// <inheritdoc />
        public override void OnUpdate()
        {
            float pos = Mathf.Sin((Time.GameTime - _startTime) * Speed) * Amplitude;
            Actor.LocalPosition = _startPos + MoveAxis * pos;
        }
    }
}
