using FlaxEngine;

namespace GraphicsFeaturesTour
{
    public class FogControl : Script
    {
        public ExponentialHeightFog Fog;
        public Actor[] ActorsToToggle;

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

        private void SetActive(bool value)
        {
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

        private void OnTriggerEnter(Collider collider)
        {
            if (Fog && collider is CharacterController)
            {
                SetActive(true);
            }
        }

        private void OnTriggerExit(Collider collider)
        {
            if (Fog && collider is CharacterController)
            {
                SetActive(false);
            }
        }
    }
}