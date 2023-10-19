using FlaxEngine;

namespace PhysicsFeaturesTour
{
    public class CollisionSample : Script
    {
        private float timeout;

        public StaticModel ModelToControl;

        public MaterialBase MaterialOn;
        public MaterialBase MaterialOff;

        /// <inheritdoc />
        public override void OnStart()
        {
            timeout = 0;
        }

        /// <inheritdoc />
        public override void OnEnable()
        {
            Actor.As<Collider>().CollisionEnter += OnCollisionEnter;
        }

        /// <inheritdoc />
        public override void OnDisable()
        {
            Actor.As<Collider>().CollisionEnter -= OnCollisionEnter;
        }

        private void OnCollisionEnter(Collision collision)
        {
            timeout = 0.5f;
        }

        /// <inheritdoc />
        public override void OnUpdate()
        {
            bool isOn = false;
            if (timeout > 0)
            {
                timeout -= Time.UnscaledDeltaTime;
                isOn = timeout > 0;
            }

            if (ModelToControl)
                ModelToControl.SetMaterial(0, isOn ? MaterialOn : MaterialOff);
        }
    }
}
