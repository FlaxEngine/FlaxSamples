using System;
using System.Collections.Generic;
using FlaxEngine;

namespace MainMenuSample
{
    public class PostProcessingDisabler : Script
    {
        bool WasLowGraphicsLastFrame;

        public override void OnUpdate()
        {
            bool IsLowGraphicsThisFrame = GlobalSettingsData.IsLowGraphics;

            if (IsLowGraphicsThisFrame != WasLowGraphicsLastFrame)
            {
                Actor.As<PostFxVolume>().BlendWeight = (IsLowGraphicsThisFrame ? 1f : 0f);
            }

            WasLowGraphicsLastFrame = IsLowGraphicsThisFrame;
        }
    }
}
