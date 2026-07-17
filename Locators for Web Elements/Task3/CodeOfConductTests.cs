using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Locators_for_Web_Elements
{
    public class CodeOfConductTests
    {
        private HomePage HomePage { get; set; }
        private string _downloadDirectory;
        private IWebDriver Driver { get; set; }

        public static void CleanUp(string filePath)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(filePath);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public static async Task<bool> ValidateFileDownloadedAsync(
        string downloadDirectory,
        string expectedFileName,
        int timeoutSeconds = 60,
        int pollIntervalMilliseconds = 500)
        {
            var targetFilePath = Path.Combine(downloadDirectory, expectedFileName);
            var partialFilePath = targetFilePath + ".crdownload";
            var deadline = DateTime.UtcNow.AddSeconds(timeoutSeconds);

            while (DateTime.UtcNow < deadline)
            {
                bool targetExists = File.Exists(targetFilePath);
                bool partialExists = File.Exists(partialFilePath);

                if (targetExists && !partialExists)
                {
                    CleanUp(targetFilePath);
                    return true;
                }

                await Task.Delay(pollIntervalMilliseconds);
            }

            return false;
        }

        public void BrowserOptionsSetUp(ChromeOptions options, string downloadDirectory)
        {
            options.AddArgument("--start-maximized");
            options.AddUserProfilePreference("download.default_directory", downloadDirectory);
            options.AddUserProfilePreference("download.prompt_for_download", false);
            options.AddUserProfilePreference("download.directory_upgrade", true);
            options.AddUserProfilePreference("download.open_pdf_in_system_reader", false);
            options.AddUserProfilePreference("plugins.always_open_pdf_externally", true);
        }

        public void AssignDownloadFolder(string path)
        {
            _downloadDirectory = Path.Combine(Directory.GetCurrentDirectory(), path);
            
        }

        public void CreateDownloadFolder(string path)
        {
            Directory.CreateDirectory(_downloadDirectory);
        }

        [SetUp]
        public void SetUp()
        {
            AssignDownloadFolder("TestDownloads");
            CreateDownloadFolder(_downloadDirectory);

            var options = new ChromeOptions();
            BrowserOptionsSetUp(options, _downloadDirectory);

            Driver = new ChromeDriver(options);
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            HomePage = new HomePage(Driver, wait);
            HomePage.NavigateToHome();
        }

        [Test]
        public async Task CodeOfEthicalConductPdf_DownloadsWithExpectedFileName()
        {
            var page = HomePage.AcceptCookies()
            .ClickCodeOfConduct();

            bool downloaded = await ValidateFileDownloadedAsync(
                _downloadDirectory, "Code-Of-Conduct_01_26.pdf");

            Assert.That(downloaded, Is.True, "Expected PDF was not downloaded within the timeout.");
        }

        [TearDown]
        public void TearDown()
        {
            Driver.Quit();
            Driver.Dispose();
        }
    }
}