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
        public UIControl OptionsMainMenuButton;
        public UIControl QuitButton;
        public Actor MainMenuText;

        public UIControl BlurPanel;
        public Actor PostProcessingDisabler;

        public UIControl FullscreenButton;
        public UIControl GraphicsSettingButton;
        public UIControl AudioButton;
        public UIControl OptionsBackButton;
        public Actor OptionsMenuText;

        public SceneReference GameScene;

        //--- Private variables ---\\
        private bool _audioEnabled;
        private bool _highGrapicsPreset;
        private bool _optionsMenuActive;
        private bool _fullscreen;


        public override void OnEnable()
        {
            PlayButton.Get<Button>().ButtonClicked += PlayButtonFunction;
            OptionsMainMenuButton.Get<Button>().ButtonClicked += ToggleOptionsMenu;
            OptionsBackButton.Get<Button>().ButtonClicked += ToggleOptionsMenu;
            FullscreenButton.Get<Button>().ButtonClicked += ToggleFullscreen;
            GraphicsSettingButton.Get<Button>().ButtonClicked += ModifyGraphicsSettingQuality;
            AudioButton.Get<Button>().ButtonClicked += ToggleAudio;
            QuitButton.Get<Button>().ButtonClicked += QuitButtonFunction;

            _optionsMenuActive = true;
            ToggleOptionsMenu(OptionsMainMenuButton.Get<Button>());

            bool preloadGfxBool = SettingsManager.Instance.GFX;
            bool preloadFullscreenBool = SettingsManager.Instance.FULLSCREEN;
            bool preloadSfxBool = SettingsManager.Instance.SFX;

            _highGrapicsPreset = !preloadGfxBool;
            ModifyGraphicsSettingQuality(GraphicsSettingButton.Get<Button>());

            _fullscreen = !preloadFullscreenBool;
            ToggleFullscreen(FullscreenButton.Get<Button>());

            _audioEnabled = !preloadSfxBool;
            ToggleAudio(AudioButton.Get<Button>());
        }

        public override void OnDisable()
        {
            PlayButton.Get<Button>().ButtonClicked -= PlayButtonFunction;
            OptionsMainMenuButton.Get<Button>().ButtonClicked -= ToggleOptionsMenu;
            OptionsBackButton.Get<Button>().ButtonClicked -= ToggleOptionsMenu;
            FullscreenButton.Get<Button>().ButtonClicked -= ToggleFullscreen;
            GraphicsSettingButton.Get<Button>().ButtonClicked -= ModifyGraphicsSettingQuality;
            AudioButton.Get<Button>().ButtonClicked -= ToggleAudio;
            QuitButton.Get<Button>().ButtonClicked -= QuitButtonFunction;
        }

        public void WriteSettingsFile(bool GraphicsStatus, bool FullscreenStatus, bool AudioStatus)
        {
            SettingsManager.Instance.GFX = GraphicsStatus;
            SettingsManager.Instance.FULLSCREEN = FullscreenStatus;
            SettingsManager.Instance.SFX = AudioStatus;
        }

        public void PlayButtonFunction(Button button)
        {
            Level.ChangeSceneAsync(GameScene);
        }

        public void ToggleOptionsMenu(Button button)
        {
            if (!_optionsMenuActive)
            {
                FullscreenButton.IsActive = true;
                GraphicsSettingButton.IsActive = true;
                AudioButton.IsActive = true;
                OptionsBackButton.IsActive = true;
                OptionsMenuText.IsActive = true;
                PlayButton.IsActive = false;
                OptionsMainMenuButton.IsActive = false;
                QuitButton.IsActive = false;
                MainMenuText.IsActive = false;

                _optionsMenuActive = true;
            }
            else
            {
                FullscreenButton.IsActive = false;
                GraphicsSettingButton.IsActive = false;
                AudioButton.IsActive = false;
                OptionsBackButton.IsActive = false;
                OptionsMenuText.IsActive = false;
                PlayButton.IsActive = true;
                OptionsMainMenuButton.IsActive = true;
                QuitButton.IsActive = true;
                MainMenuText.IsActive = true;

                _optionsMenuActive = false;
            }
        }

        public void ToggleFullscreen(Button button)
        {
            if (_fullscreen)
            {
                Screen.IsFullscreen = false;
                _fullscreen = false;

                FullscreenButton.Get<Button>().Text = "Fullscreen: Off";
            }
            else
            {
                Screen.IsFullscreen = true;
                _fullscreen = true;

                FullscreenButton.Get<Button>().Text = "Fullscreen: On";
            }

            WriteSettingsFile(_highGrapicsPreset, _fullscreen, _audioEnabled);
        }

        public void ToggleAudio(Button button)
        {
            if (!_audioEnabled)
            {
                Audio.MasterVolume = 1;
                AudioButton.Get<Button>().Text = "Audio: On";

                _audioEnabled = true;
            }
            else
            {
                Audio.MasterVolume = 0;
                AudioButton.Get<Button>().Text = "Audio: Off";
                _audioEnabled = false;
            }

            WriteSettingsFile(_highGrapicsPreset, _fullscreen, _audioEnabled);
        }

        public void ModifyGraphicsSettingQuality(Button button)
        {
            if (!_highGrapicsPreset)
            {
                Graphics.SSAOQuality = Quality.High;
                Graphics.SSRQuality = Quality.High;
                Graphics.VolumetricFogQuality = Quality.High;
                Graphics.ShadowMapsQuality = Quality.High;
                Graphics.ShadowsQuality = Quality.High;
                Graphics.AAQuality = Quality.High;
                BlurPanel.Get<BlurPanel>().BlurStrength = 2.5f;
                PostProcessingDisabler.IsActive = false;
                GraphicsSettingButton.Get<Button>().Text = "Graphics: High";

                _highGrapicsPreset = true;
            }
            else
            {
                Graphics.SSAOQuality = Quality.Low;
                Graphics.SSRQuality = Quality.Low;
                Graphics.VolumetricFogQuality = Quality.Low;
                Graphics.ShadowMapsQuality = Quality.Low;
                Graphics.ShadowsQuality = Quality.Low;
                Graphics.AAQuality = Quality.Low;
                BlurPanel.Get<BlurPanel>().BlurStrength = 1.5f;
                PostProcessingDisabler.IsActive = true;
                GraphicsSettingButton.Get<Button>().Text = "Graphics: Low";

                _highGrapicsPreset = false;
            }

            WriteSettingsFile(_highGrapicsPreset, _fullscreen, _audioEnabled);
        }

        public void QuitButtonFunction(Button button)
        {
            Engine.RequestExit();
        }

    }
}
