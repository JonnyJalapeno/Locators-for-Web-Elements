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
            page.SetCDPValues(_downloadDirectory);

            bool downloaded = await CodeOfConduct.ValidateFileDownloadedAsync(
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