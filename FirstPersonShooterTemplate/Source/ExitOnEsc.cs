using System;
using System.Collections.Generic;
using FlaxEngine;

namespace FirstPersonShooterTemplate
{
    public class ExitOnEsc : Script
    {
        private void Update()
        {
            if (Input.GetKeyUp(Keys.Escape))
                Application.Exit();
        }
    }
}
