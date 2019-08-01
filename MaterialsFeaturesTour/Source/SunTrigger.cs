using FlaxEngine;

namespace MaterialsFeaturesTour
{
    public class SunTrigger : Script
    {
        public DirectionalLight Light;

        public override void OnEnable()
        {
            Actor.As<Collider>().TriggerEnter += OnTriggerEnter;
            Actor.As<Collider>().TriggerExit += OnTriggerExit;
        }

        public override void OnDisable()
        {
            Actor.As<Collider>().TriggerEnter -= OnTriggerEnter;
            Actor.As<Collider>().TriggerExit -= OnTriggerExit;
        }

        void OnTriggerEnter(Collider collider)
        {
            if (collider is CharacterController && Light)
            {
                Light.IsActive = false;
            }
        }

        void OnTriggerExit(Collider collider)
        {
            if (collider is CharacterController && Light)
            {
                Light.IsActive = true;
            }
        }
    }
}
