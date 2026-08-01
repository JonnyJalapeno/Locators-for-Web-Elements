namespace Locators_for_Web_Elements.Core
{
    public class TafConfig
    {
        public bool Headless { get; set; }
        public int ExplicitWaitSeconds { get; set; } = 10;
        public string DownloadDirectory { get; set; } = "TestDownloads";
        public string ScreenshotDirectory { get; set; } = "Screenshots";
        public LoggingConfig Logging { get; set; } = new();
    }

    public class LoggingConfig
    {
        public string MinLevel { get; set; } = "Info";
    }
}
