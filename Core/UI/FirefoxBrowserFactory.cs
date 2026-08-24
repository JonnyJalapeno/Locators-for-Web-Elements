using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace Locators_for_Web_Elements.Core.UI
{
    public class FirefoxBrowserFactory : IBrowserFactory
    {
        private readonly TafConfig _config;
        private readonly ILogger<FirefoxBrowserFactory> _logger;

        public FirefoxBrowserFactory(IOptions<TafConfig> config, ILogger<FirefoxBrowserFactory> logger)
        {
            _config = config.Value;
            _logger = logger;
        }

        public IWebDriver Create()
        {
            var options = new FirefoxOptions();

            if (_config.Headless)
            {
                options.AddArgument("-headless"); // same Cloudflare caveat as Chrome applies here
            }

            var downloadDirectory = Path.Combine(Directory.GetCurrentDirectory(), _config.DownloadDirectory);
            Directory.CreateDirectory(downloadDirectory);

            options.SetPreference("browser.download.folderList", 2);
            options.SetPreference("browser.download.dir", downloadDirectory);
            options.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/pdf");
            options.SetPreference("pdfjs.disabled", true);

            _logger.LogInformation(
                "Launching Firefox browser (headless: {Headless}, downloadDirectory: {DownloadDirectory})",
                _config.Headless, downloadDirectory);

            return new FirefoxDriver(options);
        }
    }
}
