using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Text.Json;

namespace Locators_for_Web_Elements
{
    public class CodeOfConduct : BaseComponent<CodeOfConduct>
    {      

        public CodeOfConduct(IWebDriver driver,WebDriverWait wait) : base(driver, wait)
        {      
        }

        public void SetCDPValues(string downloadDirectory)
        {
            if (Driver is ChromeDriver chromeDriver)
            {
                try
                {
                    var result = chromeDriver.ExecuteCdpCommand("Browser.setDownloadBehavior", new Dictionary<string, object>
                    {
                        ["behavior"] = "allow",
                        ["downloadPath"] = downloadDirectory,
                        ["eventsEnabled"] = true
                    });
                    Console.WriteLine($"CDP setDownloadBehavior result: {result}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"CDP setDownloadBehavior FAILED: {ex}");
                }
            }
        }

        

        public static async Task<bool> ValidateFileDownloadedAsync(
        string downloadDirectory,
        string expectedFileName,
        int timeoutSeconds = 30,
        int pollIntervalMilliseconds = 500)
        {
            var targetFilePath = Path.Combine(downloadDirectory, expectedFileName);
            var partialFilePath = targetFilePath + ".crdownload";
            var deadline = DateTime.UtcNow.AddSeconds(timeoutSeconds);

            while (DateTime.UtcNow < deadline)
            {
                bool targetExists = File.Exists(targetFilePath);
                bool partialExists = File.Exists(partialFilePath);

                if (targetExists && !partialExists && await IsFileSizeStableAsync(targetFilePath, pollIntervalMilliseconds))
                {
                    return true;
                }

                await Task.Delay(pollIntervalMilliseconds);
            }

            return false;
        }
        private static async Task<bool> IsFileSizeStableAsync(string filePath, int checkIntervalMilliseconds)
        {
            try
            {
                long sizeBefore = new FileInfo(filePath).Length;
                await Task.Delay(checkIntervalMilliseconds);
                long sizeAfter = new FileInfo(filePath).Length;
                return sizeBefore == sizeAfter && sizeAfter > 0;
            }
            catch (IOException)
            {
                return false;
            }
        }
    }
}
