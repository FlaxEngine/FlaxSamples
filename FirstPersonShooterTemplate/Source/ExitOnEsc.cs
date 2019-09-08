using FlaxEngine;

namespace FirstPersonShooterTemplate
{
    public class ExitOnEsc : Script
    {
        public override void OnUpdate()
        {
            if (Input.GetKeyUp(Keys.Escape))
                Platform.Exit();
        }
    }
}