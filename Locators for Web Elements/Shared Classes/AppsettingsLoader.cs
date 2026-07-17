using System;
using System.IO;
using System.Text.Json;

namespace Locators_for_Web_Elements
{
    public static class AppsettingsLoader
    {
        private class EpamConfig
        {
            public string WebUrl { get; set; } = string.Empty;
        }

        public static string GetWebUrl()
        {
            var config = DeserializeAppSettings();

            if (string.IsNullOrWhiteSpace(config.WebUrl))
            {
                throw new InvalidOperationException("WebUrl was not set in the config file.");
            }

            return config.WebUrl;
        }

        private static EpamConfig DeserializeAppSettings()
        {
            const string fileName = "appsettings.json";

            try
            {
                using FileStream openStream = File.OpenRead(fileName);

                var config = JsonSerializer.Deserialize<EpamConfig>(openStream);

                if (config == null)
                {
                    throw new InvalidOperationException(
                        $"Could not deserialize configuration file '{fileName}'.");
                }

                return config;
            }
            catch (FileNotFoundException ex)
            {
                throw new InvalidOperationException(
                    $"Could not load config file '{fileName}'.", ex);
            }
        }
    }
}