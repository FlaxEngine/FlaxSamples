using FlaxEngine;

namespace MaterialsFeaturesTour
{
    public class ExitOnEsc : Script
    {
        /// <inheritdoc />
        public override void OnUpdate()
        {
            if (Input.GetKeyUp(KeyboardKeys.Escape))
                Engine.RequestExit();
        }
    }
}
