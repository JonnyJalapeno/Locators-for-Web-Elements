using Locators_for_Web_Elements.Business;
using Locators_for_Web_Elements.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using NUnit.Framework.Interfaces;
using Locators_for_Web_Elements.Business.UI;
using Locators_for_Web_Elements.Core.UI;

namespace Locators_for_Web_Elements.Tests
{
    // Holds everything that is identical from test to test: spinning up the DI
    // container, resolving the driver/HomePage, navigating home, tearing the
    // browser+container down again, logging test start/end, and capturing a
    // screenshot on failure. Concrete test fixtures only contain their own
    // [Test]/[TestCase] methods.
    public abstract class TestsBase
    {
        protected ServiceProvider Services { get; private set; } = null!;
        protected IWebDriver Driver { get; private set; } = null!;
        protected HomePage HomePage { get; private set; } = null!;
        protected TafConfig TafConfig { get; private set; } = null!;

        protected string DownloadDirectory =>
            Path.Combine(Directory.GetCurrentDirectory(), TafConfig.DownloadDirectory);

        private ILogger<TestsBase> _logger = null!;

        [SetUp]
        public void BaseSetUp()
        {
            var configuration = ConfigurationFactory.Build();

            Services = new ServiceCollection()
                .AddCoreTafServices(configuration)
                .AddBusinessServices(configuration)
                .BuildServiceProvider();

            Driver = Services.GetRequiredService<IWebDriver>();
            HomePage = Services.GetRequiredService<HomePage>();
            TafConfig = Services.GetRequiredService<IOptions<TafConfig>>().Value;
            _logger = Services.GetRequiredService<ILogger<TestsBase>>();

            _logger.LogInformation("Starting test: {TestName}", TestContext.CurrentContext.Test.FullName);

            HomePage.NavigateToHome();
        }

        [TearDown]
        public void BaseTearDown()
        {
            var testResult = TestContext.CurrentContext.Result.Outcome.Status;

            if (testResult == TestStatus.Failed)
            {
                var screenshotCapturer = Services.GetRequiredService<IScreenshotCapturer>();
                screenshotCapturer.Capture(Driver, TestContext.CurrentContext.Test.Name);
            }

            _logger.LogInformation(
                "Finished test: {TestName} with result {Result}",
                TestContext.CurrentContext.Test.FullName, testResult);

            Driver.Quit();
            Driver.Dispose();
            Services.Dispose();
        }
    }
}
