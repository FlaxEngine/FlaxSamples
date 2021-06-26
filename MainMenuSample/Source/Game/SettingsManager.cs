using System;
using System.Collections.Generic;
using System.IO;
using FlaxEngine.Json;
using FlaxEngine;

namespace MainMenuSample
{
    public class SettingsManager
    {
        private static SettingsManager _instance;

        [NoSerialize]
        [HideInEditor]
        public static SettingsManager Instance
        {
            get
            {
                if (_instance == null)
                {
                   // Create a settings file if there is none.
                   if (!File.Exists(StringUtils.CombinePaths(Globals.ProductLocalFolder, "GameSettings.FlaxConfig")))
                   {
                        _instance = new SettingsManager();
                        using (var stream = File.CreateText(StringUtils.CombinePaths(Globals.ProductLocalFolder, "GameSettings.FlaxConfig")))
                        {
                            stream.WriteAsync(JsonSerializer.Serialize(_instance));
                        }
                    }
                    else
                    {
                        _instance = JsonSerializer.Deserialize<SettingsManager>(File.ReadAllText(StringUtils.CombinePaths(Globals.ProductLocalFolder, "GameSettings.FlaxConfig")));
                    }
                }

                return _instance;
            }
        }

        // Here you assign the defaults...
        public bool GFX = true;
        public bool FULLSCREEN = false;
        public bool SFX = true;
    }
}
