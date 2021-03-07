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

        public Actor BlurPanel;
        public Actor PostProcessingDisabler;

        public string GFX_Preset = "GFX";
        public string FullscreenPreset = "FULLSCREEN";
        public string SFX_Preset = "SFX";

        [Tooltip("The dot is placed automatically.")]
            public string SettingsFileExtension = "FlaxConfig";
            public string SettingsFileName = "GameSettings";


        [Tooltip("Used in the settings file.")]
            public string SettingsFileTrueTerm = "True";
        [Tooltip("Used in the settings file.")]
            public string SettingsFileFalseTerm = "False";

        [Tooltip("Writes a comment on the bottom of the settings file.")]
            public bool EnableSettingsFileComments;


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

        private string _dataPath;
        private string _directoryPath;
        private string[] _lastSettingsReadValue;


        public override void OnEnable()
        {
            PlayButton.Get<Button>().ButtonClicked += PlayButtonFunction;
            OptionsMainMenuButton.Get<Button>().ButtonClicked += ToggleOptionsMenu;
            OptionsBackButton.Get<Button>().ButtonClicked += ToggleOptionsMenu;
            FullscreenButton.Get<Button>().ButtonClicked += ToggleFullscreen;
            GraphicsSettingButton.Get<Button>().ButtonClicked += ModifyGraphicsSettingQuality;
            AudioButton.Get<Button>().ButtonClicked += ToggleAudio;
            QuitButton.Get<Button>().ButtonClicked += QuitButtonFunction;

            // Generates the path to the settings file.
            string fileName = SettingsFileName + "." + SettingsFileExtension;
            
            _dataPath = StringUtils.CombinePaths(Globals.ProductLocalFolder, fileName);
            Debug.Log("Settings file location: " + _dataPath);

            if (!File.Exists(_dataPath))
            {
                if (!Directory.Exists(Globals.ProductLocalFolder))
                {
                    Directory.CreateDirectory(Globals.ProductLocalFolder);
                }
                WriteSettingsFile(true, false, true);
            }

            _optionsMenuActive = true;
            ToggleOptionsMenu(OptionsMainMenuButton.Get<Button>());

            ReadSettingsFile();

            string lastRead_Formed = "";
            for (var readLine = 0; readLine < _lastSettingsReadValue.Length; readLine++)
            {
                lastRead_Formed = lastRead_Formed + Environment.NewLine + _lastSettingsReadValue[readLine];
            }

            bool gfxBool;
            if (lastRead_Formed.Contains(GFX_Preset + " = " + SettingsFileTrueTerm + ";"))
            {
                gfxBool = true;
            }else
            {
                gfxBool = false;
            }

            bool fullscreenBool;
            if (lastRead_Formed.Contains(FullscreenPreset + " = " + SettingsFileTrueTerm + ";"))
            {
                fullscreenBool = true;
            }else
            {
                fullscreenBool = false;
            }

            bool sfxBool;
            if (lastRead_Formed.Contains(SFX_Preset + " = " + SettingsFileTrueTerm + ";"))
            {
                sfxBool = true;
            }else
            {
                sfxBool = false;
            }

            _highGrapicsPreset = !gfxBool;
            ModifyGraphicsSettingQuality(GraphicsSettingButton.Get<Button>());

            _fullscreen = !fullscreenBool;
            ToggleFullscreen(FullscreenButton.Get<Button>());

            _audioEnabled = !sfxBool;
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

        public string[] ReadSettingsFile()
        {
            _lastSettingsReadValue = File.ReadAllLines(_dataPath);
            return _lastSettingsReadValue;
        }

        public void WriteSettingsFile(bool GraphicsStatus, bool FullscreenStatus, bool AudioStatus)
        {
            string _writtenSettingsValue;
            string _retreivedSettings;

            if (GraphicsStatus)
            {
                _retreivedSettings = GFX_Preset + " = " + SettingsFileTrueTerm + ";";
            }else
            {
                _retreivedSettings = GFX_Preset + " = " + SettingsFileFalseTerm + ";";
            }

            if (FullscreenStatus)
            {
                _retreivedSettings = _retreivedSettings + Environment.NewLine + FullscreenPreset + " = " + SettingsFileTrueTerm + ";";
            }else
            {
                _retreivedSettings = _retreivedSettings + Environment.NewLine + FullscreenPreset + " = " + SettingsFileFalseTerm + ";";
            }

            if (AudioStatus)
            {
                _retreivedSettings = _retreivedSettings + Environment.NewLine + SFX_Preset + " = " + SettingsFileTrueTerm + ";";
            }else
            {
                _retreivedSettings = _retreivedSettings + Environment.NewLine + SFX_Preset + " = " + SettingsFileFalseTerm + ";";
            }



            if (EnableSettingsFileComments)
            {
                string _settingsComment = "// These values represent the in-game settings." +
                 Environment.NewLine + "// The values match as following:" + Environment.NewLine + "// Setting enabled -> " + SettingsFileTrueTerm + 
                 Environment.NewLine + "// Setting disabled -> " + SettingsFileFalseTerm + Environment.NewLine + "// Non-matching values will result in 'False' value.";

                _writtenSettingsValue = _retreivedSettings + Environment.NewLine + Environment.NewLine + _settingsComment; // This is where the 2 bonus newlines come from.
            }else
            {
                _writtenSettingsValue = _retreivedSettings;
            }

            File.WriteAllText(_dataPath, _writtenSettingsValue);
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
            }else
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
            }else
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
            }else
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
                BlurPanel.IsActive = true;
                PostProcessingDisabler.IsActive = false;
                GraphicsSettingButton.Get<Button>().Text = "Graphics: High";

                _highGrapicsPreset = true;
            }else
            {
                Graphics.SSAOQuality = Quality.Low;
                Graphics.SSRQuality = Quality.Low;
                Graphics.VolumetricFogQuality = Quality.Low;
                Graphics.ShadowMapsQuality = Quality.Low;
                Graphics.ShadowsQuality = Quality.Low;
                Graphics.AAQuality = Quality.Low;
                BlurPanel.IsActive = false;
                PostProcessingDisabler.IsActive = true;
                GraphicsSettingButton.Get<Button>().Text = "Graphics: Low";

                _highGrapicsPreset = false;
            }

            WriteSettingsFile(_highGrapicsPreset, _fullscreen, _audioEnabled);
        }

        public void QuitButtonFunction(Button button)
        {
            Engine.RequestExit();
            Debug.LogError("Crash data could not be read.");
        }

    }
}
