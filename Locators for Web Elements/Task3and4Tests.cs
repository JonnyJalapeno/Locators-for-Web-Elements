using Locators_for_Web_Elements;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Locators_for_Web_Elements
{
    public class Task3and4Tests
    {
        private Task3and4 EpamPom { get; set; }
        private string _downloadDirectory;

        [SetUp]
        public void SetUp()
        {
            _downloadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "TestDownloads");
            Directory.CreateDirectory(_downloadDirectory);

            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            options.AddUserProfilePreference("download.default_directory", _downloadDirectory);
            options.AddUserProfilePreference("download.prompt_for_download", false);
            options.AddUserProfilePreference("download.directory_upgrade", true);
            options.AddUserProfilePreference("download.open_pdf_in_system_reader", true);
            options.AddUserProfilePreference("plugins.always_open_pdf_externally", true);

            IWebDriver driver = new ChromeDriver(options);
            EpamPom = new Task3and4(driver, _downloadDirectory); // pass it here
            EpamPom.NavigateToHome();
        }

        [Test]
        public async Task CodeOfEthicalConductPdf_DownloadsWithExpectedFileName()
        {
            EpamPom.AcceptCookies();
            EpamPom.ClickCodeofEthicalConduct();
            Console.WriteLine($"Download dir exists: {Directory.Exists(_downloadDirectory)}");
            if (Directory.Exists(_downloadDirectory))
            {
                var files = Directory.GetFiles(_downloadDirectory);
                Console.WriteLine($"Files in download dir ({files.Length}):");
                foreach (var f in files) Console.WriteLine($"  {f}");
            }

            // Ask Chrome itself what it thinks downloaded, via chrome://downloads internals
            ((IJavaScriptExecutor)EpamPom.Driver).ExecuteScript(
                "window.open('chrome://downloads/', '_blank');");

            bool downloaded = await Task3and4.ValidateFileDownloadedAsync(
                _downloadDirectory, "Code-Of-Conduct_01_26.pdf");

            Assert.That(downloaded, Is.True, "Expected PDF was not downloaded within the timeout.");
        }

        [TearDown]
        public void TearDown()
        {
            EpamPom.Quit();
        }
    }
}