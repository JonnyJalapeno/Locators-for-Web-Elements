using Microsoft.Extensions.Configuration;

namespace Locators_for_Web_Elements.Core
{
    public static class ConfigurationFactory
    {
        private const string EnvironmentVariableName = "TEST_ENVIRONMENT";

        public static IConfiguration Build()
        {
            var environmentName = Environment.GetEnvironmentVariable(EnvironmentVariableName);

            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

            if (!string.IsNullOrWhiteSpace(environmentName))
            {
                var overrideFile = FindConfigFileCaseInsensitive(environmentName);

                // Falls back to the literal expected name if nothing on disk
                // matches, so AddJsonFile's own "optional: true" behavior
                // (silently skip if missing) still applies as before -
                // this only changes casing resolution, not whether a
                // missing/misspelled environment name is still tolerated.
                builder.AddJsonFile(
                    overrideFile ?? $"appsettings.{environmentName}.json",
                    optional: true,
                    reloadOnChange: false);
            }

            builder.AddEnvironmentVariables();

            return builder.Build();
        }

        // appsettings.<Environment>.json lookups are case-sensitive on
        // Linux CI runners but case-insensitive on Windows. A TEST_ENVIRONMENT
        // value like "staging" silently fails to match a file named
        // "appsettings.Staging.json" on Linux, with no error, since
        // AddJsonFile is called with optional: true - it just quietly skips
        // the override. Resolving the actual on-disk filename here, by a
        // case-insensitive match against files present in the current
        // directory, makes TEST_ENVIRONMENT's casing irrelevant on any OS.
        private static string? FindConfigFileCaseInsensitive(string environmentName)
        {
            var targetFileName = $"appsettings.{environmentName}.json";
            var directory = Directory.GetCurrentDirectory();

            return Directory.EnumerateFiles(directory, "appsettings.*.json")
                .Select(Path.GetFileName)
                .FirstOrDefault(fileName =>
                    string.Equals(fileName, targetFileName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
