using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Text.Json;

namespace Locators_for_Web_Elements
{
    public class Task3and4
    {
        private struct EpamConfig
        {
            public string WebUrl { get; set; }
        }
        private string WebUrl { get; set; }
        public IWebDriver Driver { get; init; }
        private WebDriverWait ExplicitWait { get; init; }

        private readonly By Policies = By.XPath("//div[contains(@class, 'policies-links-wrapper')]");
        private readonly By CodeOfConduct =
    By.XPath("//a[contains(normalize-space(.), 'Ethical Conduct')]");
        private readonly By PrivacyBanner = By.CssSelector("div[role='dialog'][aria-label='Privacy']");
        private readonly By CookiesAcceptanceLocator = By.Id("onetrust-accept-btn-handler");

        public Task3and4(IWebDriver driver, string downloadDirectory)
        {
            EpamConfig config = DeserializeAppSettings();
            LoadJsonValues(config);
            Driver = driver;
            ExplicitWait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));

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

        private void LoadJsonValues(EpamConfig config)
        {
            WebUrl = config.WebUrl;
        }
        private static EpamConfig DeserializeAppSettings()
        {
            string fileName = "appsettings.json";
            try
            {
                using FileStream openStream = File.OpenRead(fileName);
                EpamConfig config = JsonSerializer.Deserialize<EpamConfig>(openStream);
                return config;
            }
            catch (FileNotFoundException ex)
            {
                throw new InvalidOperationException($"Could not load config file '{fileName}'.", ex);
            }
            catch (ArgumentNullException ex)
            {
                throw new InvalidOperationException("WebUrl was not set in the config file.", ex);
            }
        }

        public Task3and4 NavigateToHome()
        {
            Driver.Navigate().GoToUrl(WebUrl);
            return this;
        }

        public void Quit() => Driver.Quit();

        public Task3and4 ClickCodeofEthicalConduct()
        {
            //var policies = Driver.FindElement(Policies);
            //new Actions(Driver).ScrollToElement(policies).Perform();
            var element = Driver.FindElement(CodeOfConduct);
            new Actions(Driver)
        .MoveToElement(element)
        .Pause(TimeSpan.FromMilliseconds(200))
        .Click()
        .Perform();
            Console.WriteLine(element.GetAttribute("href"));
            Console.WriteLine($"Window handles BEFORE click: {Driver.WindowHandles.Count}");

            element.Click();

            Console.WriteLine($"Window handles AFTER click: {Driver.WindowHandles.Count}");
            Console.WriteLine("URL after click: " + Driver.Url);
            return this;
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

        public Task3and4 AcceptCookies()
        {
            try
            {
                var button = ExplicitWait.Until(driver =>
                {
                    IWebElement element;
                    try { element = driver.FindElement(CookiesAcceptanceLocator); }
                    catch (NoSuchElementException) { return null; }

                    if (!element.Displayed || !element.Enabled) return null;

                    // Confirm it's actually on top / not covered
                    var js = (IJavaScriptExecutor)driver;
                    var isClickable = (bool)js.ExecuteScript(@"
                        var rect = arguments[0].getBoundingClientRect();
                        var x = rect.left + rect.width/2, y = rect.top + rect.height/2;
                        var el = document.elementFromPoint(x, y);
                        return arguments[0].contains(el);", element);

                    return isClickable ? element : null;
                });

                button.Click();

                ExplicitWait.Until(driver =>
                {
                    var dialogs = driver.FindElements(PrivacyBanner);

                    if (dialogs.Count == 0)
                        return true;

                    return !dialogs[0].Displayed;
                });
            }
            catch (WebDriverTimeoutException)
            {
                // Cookie banner not present
            }
            return this;
        }
    }
}
