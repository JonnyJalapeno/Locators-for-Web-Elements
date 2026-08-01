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
                builder.AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: false);
            }

            builder.AddEnvironmentVariables();

            return builder.Build();
        }
    }
}
