using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Locators_for_Web_Elements.Core.UI
{
    public class ChromeBrowserFactory : IBrowserFactory
    {
        private readonly TafConfig _config;
        private readonly ILogger<ChromeBrowserFactory> _logger;

        public ChromeBrowserFactory(IOptions<TafConfig> config, ILogger<ChromeBrowserFactory> logger)
        {
            _config = config.Value;
            _logger = logger;
        }

        public IWebDriver Create()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");

            if (_config.Headless)
            {
                options.AddArgument("--headless=new"); //this won't work with EPAM site due to cloudlfare protection
            }

            var downloadDirectory = Path.Combine(Directory.GetCurrentDirectory(), _config.DownloadDirectory);
            Directory.CreateDirectory(downloadDirectory);

            options.AddUserProfilePreference("download.default_directory", downloadDirectory);
            options.AddUserProfilePreference("download.prompt_for_download", false);
            options.AddUserProfilePreference("download.directory_upgrade", true);
            options.AddUserProfilePreference("download.open_pdf_in_system_reader", false);
            options.AddUserProfilePreference("plugins.always_open_pdf_externally", true);

            _logger.LogInformation(
                "Launching Chrome browser (headless: {Headless}, downloadDirectory: {DownloadDirectory})",
                _config.Headless, downloadDirectory);

            return new ChromeDriver(options);
        }
    }
}
