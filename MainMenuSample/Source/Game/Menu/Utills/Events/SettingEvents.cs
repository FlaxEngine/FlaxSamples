using System;
using System.Collections.Generic;
using FlaxEngine;

namespace MainMenuSample
{
    public class SettingEvents : Script
    {
        public event EventHandler OnSettingsChanged;

        bool WasLowGraphicsLastFrame;
        bool WasFullscreenLastFrame;
        bool WasAudioOnLastFrame;

        public override void OnUpdate()
        {
            bool IsLowGraphicsThisFrame = GlobalSettingsData.IsLowGraphics;
            bool IsFullscreenThisFrame = GlobalSettingsData.IsFullscreen;
            bool IsAudioOnThisFrame = GlobalSettingsData.IsAudioEnabled;

            if (IsLowGraphicsThisFrame != WasLowGraphicsLastFrame || IsFullscreenThisFrame != WasFullscreenLastFrame || IsAudioOnThisFrame != WasAudioOnLastFrame)
            {
                OnSettingsChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
