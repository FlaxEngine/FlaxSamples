using FlaxEngine;

namespace MaterialsFeaturesTour
{
    public class SunTrigger : Script
    {
        public DirectionalLight Light;

        /// <inheritdoc />
        public override void OnEnable()
        {
            Actor.As<Collider>().TriggerEnter += OnTriggerEnter;
            Actor.As<Collider>().TriggerExit += OnTriggerExit;
        }

        /// <inheritdoc />
        public override void OnDisable()
        {
            Actor.As<Collider>().TriggerEnter -= OnTriggerEnter;
            Actor.As<Collider>().TriggerExit -= OnTriggerExit;
        }

        void OnTriggerEnter(PhysicsColliderActor collider)
        {
            if (collider is CharacterController && Light)
            {
                Light.IsActive = false;
            }
        }

        void OnTriggerExit(PhysicsColliderActor collider)
        {
            if (collider is CharacterController && Light)
            {
                Light.IsActive = true;
            }
        }
    }
}
