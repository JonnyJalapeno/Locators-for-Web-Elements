using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;

namespace Locators_for_Web_Elements.Core
{
    public class ScreenshotCapturer : IScreenshotCapturer
    {
        private readonly TafConfig _config;
        private readonly ILogger<ScreenshotCapturer> _logger;

        public ScreenshotCapturer(IOptions<TafConfig> config, ILogger<ScreenshotCapturer> logger)
        {
            _config = config.Value;
            _logger = logger;
        }

        public string Capture(IWebDriver driver, string testName)
        {
            var directory = Path.Combine(Directory.GetCurrentDirectory(), _config.ScreenshotDirectory);
            Directory.CreateDirectory(directory);

            var safeName = string.Join("_", testName.Split(Path.GetInvalidFileNameChars()));
            var fileName = $"{safeName}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";
            var filePath = Path.Combine(directory, fileName);

            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile(filePath);

            _logger.LogError("Test {TestName} failed. Screenshot saved to {FilePath}", testName, filePath);

            return filePath;
        }
    }
}
