using System;
using System.Collections.Generic;
using FlaxEngine;

namespace FpsTemplate
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
