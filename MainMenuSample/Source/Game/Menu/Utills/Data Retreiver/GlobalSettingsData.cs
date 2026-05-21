using FlaxEngine;

namespace MainMenuSample
{
    public static class GlobalSettingsData
    {
        public static bool IsLowGraphics => (Graphics.ShadowsQuality == Quality.Low);
        public static bool IsFullscreen => (Screen.IsFullscreen);
        public static bool IsAudioEnabled => (Audio.MasterVolume > 0);
    }
}
