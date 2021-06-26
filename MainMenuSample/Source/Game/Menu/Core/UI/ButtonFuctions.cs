using System;
using System.Collections.Generic;
using FlaxEngine;
using FlaxEngine.GUI;
using System.IO;

namespace MainMenuSample
{
    public class ButtonFuctions : Script
    {
        public UIControl PlayButton;
        public UIControl OptionsButton;
        public UIControl QuitButton;

        public UIControl BlurPanel;
        public UIControl FullscreenButton;
        public UIControl GFXSettingsButton;
        public UIControl AudioButton;
        public UIControl OptionsBackButton;
        public Actor OptionsMenuActor;

        public SceneReference GameScene;

        public Actor MainMenuActor;


        public override void OnEnable()
        {
            PlayButton.Get<Button>().ButtonClicked += PlayButtonFunction;
            OptionsButton.Get<Button>().ButtonClicked += ToggleOptionsMenu;
            OptionsBackButton.Get<Button>().ButtonClicked += ToggleOptionsMenu;
            FullscreenButton.Get<Button>().ButtonClicked += ToggleFullscreen;
            GFXSettingsButton.Get<Button>().ButtonClicked += ModifyGraphicsSettingQuality;
            AudioButton.Get<Button>().ButtonClicked += ToggleAudio;
            QuitButton.Get<Button>().ButtonClicked += QuitButtonFunction;

            Debug.LogWarning("Fullscreen will always be false in the editor.");

            // Graphics loading...
            Quality TargetQuality = ((SettingsManager.Instance.GFX) ? Quality.Ultra : Quality.Low);

            Graphics.SSAOQuality = TargetQuality;
            Graphics.SSRQuality = TargetQuality;
            Graphics.VolumetricFogQuality = TargetQuality;
            Graphics.ShadowMapsQuality = TargetQuality;
            Graphics.ShadowsQuality = TargetQuality;
            Graphics.AAQuality = TargetQuality;
            BlurPanel.Get<BlurPanel>().BlurStrength = ((TargetQuality == Quality.Ultra) ? 7.5f : 0.75f);

            // Fullscreen loading...
            Screen.IsFullscreen = SettingsManager.Instance.FULLSCREEN;

            // Audio loading...
            Audio.MasterVolume = (SettingsManager.Instance.SFX) ? 1 : 0;

            // Updating the UI
            UpdateUI();
        }

        public override void OnDisable()
        {
            PlayButton.Get<Button>().ButtonClicked -= PlayButtonFunction;
            OptionsButton.Get<Button>().ButtonClicked -= ToggleOptionsMenu;
            OptionsBackButton.Get<Button>().ButtonClicked -= ToggleOptionsMenu;
            FullscreenButton.Get<Button>().ButtonClicked -= ToggleFullscreen;
            GFXSettingsButton.Get<Button>().ButtonClicked -= ModifyGraphicsSettingQuality;
            AudioButton.Get<Button>().ButtonClicked -= ToggleAudio;
            QuitButton.Get<Button>().ButtonClicked -= QuitButtonFunction;
        }

        public void WriteSettingsFile()
        {
            SettingsManager.Instance.GFX = (Graphics.ShadowsQuality != Quality.Low);
            SettingsManager.Instance.FULLSCREEN = (Screen.IsFullscreen);
            SettingsManager.Instance.SFX = (Audio.MasterVolume > 0);

            SettingsManager.SaveAsync();
        }

        public void PlayButtonFunction(Button button)
        {
            Level.ChangeSceneAsync(GameScene);
        }

        public void ToggleOptionsMenu(Button button)
        {
            OptionsMenuActor.IsActive = !OptionsMenuActor.IsActive;
            MainMenuActor.IsActive = !MainMenuActor.IsActive;
        }

        public void ToggleFullscreen(Button button)
        {
            Screen.IsFullscreen = !Screen.IsFullscreen;
            UpdateUI();

            WriteSettingsFile();
        }

        public void ToggleAudio(Button button)
        {
            Audio.MasterVolume = (Audio.MasterVolume == 0) ? 1 : 0;
            UpdateUI();

            WriteSettingsFile();
        }

        public void ModifyGraphicsSettingQuality(Button button)
        {
            Quality TargetQuality = ((Graphics.ShadowsQuality == Quality.Ultra) ? Quality.Low : Quality.Ultra); // It flips the quality.

            Graphics.SSAOQuality = TargetQuality;
            Graphics.SSRQuality = TargetQuality;
            Graphics.VolumetricFogQuality = TargetQuality;
            Graphics.ShadowMapsQuality = TargetQuality;
            Graphics.ShadowsQuality = TargetQuality;
            Graphics.AAQuality = TargetQuality;
            BlurPanel.Get<BlurPanel>().BlurStrength = ((TargetQuality == Quality.Ultra) ? 7.5f : 0.75f);

            UpdateUI();

            WriteSettingsFile();
        }

        public void UpdateUI()
        {
            GFXSettingsButton.Get<Button>().Text = $"Graphics: {(Graphics.ShadowsQuality == Quality.Low ? "Low" : "High")}";
            FullscreenButton.Get<Button>().Text = $"Fullscreen: {ToOnOff(Screen.IsFullscreen)}";
            AudioButton.Get<Button>().Text = $"Audio: {ToOnOff(Audio.MasterVolume > 0)}";
        }

        public string ToOnOff(bool value) => (value ? "On" : "Off");

        public void QuitButtonFunction(Button button)
        {
            Engine.RequestExit();
        }

    }
}
