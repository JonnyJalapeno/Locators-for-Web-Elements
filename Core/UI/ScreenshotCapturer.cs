using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;

namespace Locators_for_Web_Elements.Core.UI
{
    public class ScreenshotCapturer : IScreenshotCapturer
    {
        // Test names for TestCase-parameterized tests look like
        // Foo("bar","baz") - fine on Linux, but actions/upload-artifact
        // rejects a fixed set of characters regardless of the runner OS
        // (", :, <, >, |, *, ?, CR, LF), so Path.GetInvalidFileNameChars()
        // (which is OS-dependent and near-empty on Linux) isn't enough here.
        private static readonly Regex UnsafeFileNameChars = new(
            "[\"\\\\/:<>|*?\\r\\n]",
            RegexOptions.Compiled);

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

            var safeName = UnsafeFileNameChars.Replace(testName, "_");
            var fileName = $"{safeName}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";
            var filePath = Path.Combine(directory, fileName);

            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile(filePath);

            _logger.LogError("Test {TestName} failed. Screenshot saved to {FilePath}", testName, filePath);

            return filePath;
        }
    }
}
