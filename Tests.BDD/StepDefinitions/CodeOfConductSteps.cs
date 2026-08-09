using Locators_for_Web_Elements.Business;
using Locators_for_Web_Elements.Core;
using Reqnroll;

namespace Locators_for_Web_Elements.Tests.BDD.StepDefinitions
{
    [Binding]
    public class CodeOfConductSteps
    {
        private readonly HomePage _homePage;
        private readonly TafConfig _tafConfig;

        public CodeOfConductSteps(HomePage homePage, TafConfig tafConfig)
        {
            _homePage = homePage;
            _tafConfig = tafConfig;
        }

        private string DownloadDirectory =>
            Path.Combine(Directory.GetCurrentDirectory(), _tafConfig.DownloadDirectory);

        [When(@"I click the ""Ethical Conduct"" link")]
        public void WhenIClickTheEthicalConductLink()
        {
            _homePage.ClickCodeOfConduct();
        }

        [Then(@"the file ""(.*)"" should be downloaded")]
        public async Task ThenTheFileShouldBeDownloaded(string expectedFileName)
        {
            bool downloaded = await ValidateFileDownloadedAsync(DownloadDirectory, expectedFileName);
            Assert.That(downloaded, Is.True, "Expected PDF was not downloaded within the timeout.");
        }

        // Mirrors the download-polling helper kept local to CodeOfConductTests.cs in
        // the NUnit suite (it's test-orchestration logic, not page-object behavior,
        // so it stays out of the Business/ POM layer in both suites).
        private static void CleanUp(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private static async Task<bool> ValidateFileDownloadedAsync(
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
    }
}
