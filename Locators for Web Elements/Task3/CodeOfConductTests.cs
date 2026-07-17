using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Locators_for_Web_Elements
{
    public class CodeOfConductTests
    {
        private ServiceProvider Services { get; set; }
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

        private static void BrowserOptionsSetUp(ChromeOptions options, string downloadDirectory)
        {
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

            Services = new ServiceCollection()
                .AddSeleniumTestServices(
                    ConfigurationFactory.Build(),
                    options => BrowserOptionsSetUp(options, _downloadDirectory))
                .BuildServiceProvider();

            Driver = Services.GetRequiredService<IWebDriver>();
            HomePage = Services.GetRequiredService<HomePage>();
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
            Services.Dispose();
        }
    }
}