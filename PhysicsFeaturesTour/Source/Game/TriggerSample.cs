using FlaxEngine;

namespace PhysicsFeaturesTour
{
    public class TriggerSample : Script
    {
        [Serialize] private bool _lightOn;

        public Light LightToControl;

        [NoSerialize]
        public bool LightOn
        {
            get => _lightOn;
            set
            {
                _lightOn = value;
                if (LightToControl)
                    LightToControl.Color = value ? Color.Green : Color.Red;
            }
        }

        /// <inheritdoc />
        public override void OnStart()
        {
            // Restore state
            LightOn = _lightOn;
        }

        /// <inheritdoc />
        public override void OnEnable()
        {
            // Register for trigger events
            Actor.As<Collider>().TriggerEnter += OnTriggerEnter;
            Actor.As<Collider>().TriggerExit += OnTriggerExit;
        }

        /// <inheritdoc />
        public override void OnDisable()
        {
            // Unregister from trigger events
            Actor.As<Collider>().TriggerEnter -= OnTriggerEnter;
            Actor.As<Collider>().TriggerExit -= OnTriggerExit;
        }

        private void OnTriggerEnter(PhysicsColliderActor collider)
        {
            // Check player
            if (collider is CharacterController)
            {
                LightOn = true;
            }
        }

        private void OnTriggerExit(PhysicsColliderActor collider)
        {
            // Check player
            if (collider is CharacterController)
            {
                LightOn = false;
            }
        }
    }
}
