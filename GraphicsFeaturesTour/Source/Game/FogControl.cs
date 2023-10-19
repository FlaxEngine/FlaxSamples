using FlaxEngine;

namespace GraphicsFeaturesTour
{
    public class FogControl : Script
    {
        public ExponentialHeightFog Fog;
        public Actor[] ActorsToToggle;

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

        private void SetActive(bool value)
        {
            Fog.FogDensity = value ? 0.6f : 0.02f;
            Fog.VolumetricFogEnable = value;

            if (ActorsToToggle != null)
            {
                for (int i = 0; i < ActorsToToggle.Length; i++)
                {
                    if (ActorsToToggle[i] != null)
                        ActorsToToggle[i].IsActive = value;
                }
            }
        }

        private void OnTriggerEnter(PhysicsColliderActor collider)
        {
            if (Fog && collider is CharacterController)
            {
                SetActive(true);
            }
        }

        private void OnTriggerExit(PhysicsColliderActor collider)
        {
            if (Fog && collider is CharacterController)
            {
                SetActive(false);
            }
        }
    }
}
