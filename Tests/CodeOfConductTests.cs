namespace Locators_for_Web_Elements.Tests
{
    public class CodeOfConductTests : TestsBase
    {
        private static void CleanUp(string filePath)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(filePath);
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

        [Test]
        public async Task CodeOfEthicalConductPdf_DownloadsWithExpectedFileName()
        {
            var page = HomePage.AcceptCookies()
            .ClickCodeOfConduct();

            bool downloaded = await ValidateFileDownloadedAsync(
                DownloadDirectory, "Code-Of-Conduct_01_26.pdf");

            Assert.That(downloaded, Is.True, "Expected PDF was not downloaded within the timeout.");
        }
    }
}
