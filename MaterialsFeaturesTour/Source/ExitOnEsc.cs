using FlaxEngine;

namespace MaterialsFeaturesTour
{
    public class ExitOnEsc : Script
    {
        public override void OnUpdate()
        {
            if (Input.GetKeyUp(Keys.Escape))
                Application.Exit();
        }
    }
}