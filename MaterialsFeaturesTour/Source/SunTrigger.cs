using FlaxEngine;

namespace MaterialsFeaturesTour
{
    public class SunTrigger : Script
    {
        public DirectionalLight Light;

        void OnTriggerEnter(Collider c)
        {
            if (c is CharacterController && Light)
            {
                Light.IsActive = false;
            }
        }

        void OnTriggerExit(Collider c)
        {
            if (c is CharacterController && Light)
            {
                Light.IsActive = true;
            }
        }
    }
}
