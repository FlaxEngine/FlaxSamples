using System;
using System.Collections.Generic;
using FlaxEngine;

namespace PhysicsFeaturesTour
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
