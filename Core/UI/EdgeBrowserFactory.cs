using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace Locators_for_Web_Elements.Core.UI
{
    public class EdgeBrowserFactory : IBrowserFactory
    {
        private readonly TafConfig _config;
        private readonly ILogger<EdgeBrowserFactory> _logger;

        public EdgeBrowserFactory(IOptions<TafConfig> config, ILogger<EdgeBrowserFactory> logger)
        {
            _config = config.Value;
            _logger = logger;
        }

        public IWebDriver Create()
        {
            var options = new EdgeOptions();
            options.AddArgument("--start-maximized");

            if (_config.Headless)
            {
                options.AddArgument("--headless=new"); // same Cloudflare caveat as Chrome applies here
            }

            var downloadDirectory = Path.Combine(Directory.GetCurrentDirectory(), _config.DownloadDirectory);
            Directory.CreateDirectory(downloadDirectory);

            options.AddUserProfilePreference("download.default_directory", downloadDirectory);
            options.AddUserProfilePreference("download.prompt_for_download", false);
            options.AddUserProfilePreference("download.directory_upgrade", true);

            _logger.LogInformation(
                "Launching Edge browser (headless: {Headless}, downloadDirectory: {DownloadDirectory})",
                _config.Headless, downloadDirectory);

            return new EdgeDriver(options);
        }
    }
}
