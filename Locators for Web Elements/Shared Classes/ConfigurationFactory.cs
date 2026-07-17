using Microsoft.Extensions.Configuration;

namespace Locators_for_Web_Elements
{
    public static class ConfigurationFactory
    {
        public static IConfiguration Build() =>
            new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
    }
}
