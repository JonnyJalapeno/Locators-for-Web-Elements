using Locators_for_Web_Elements.Business.UI;
using Locators_for_Web_Elements.Core.UI;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using Reqnroll;

namespace Locators_for_Web_Elements.Tests.BDD.Support
{
    // Per-scenario lifecycle glue — BDD counterpart of Tests/TestsBase.cs.
    // Driver/HomePage/etc. are constructor-injected by Reqnroll's native DI
    // integration (see TestDependencies.CreateServices) from a fresh scope per
    // scenario, since IWebDriver and its dependents are registered Scoped in
    // Core. That gives the same "fresh browser per run" isolation TestsBase.cs
    // gets by building a whole new root ServiceProvider per NUnit test.
    [Binding]
    public class Hooks
    {
        private readonly IWebDriver _driver;
        private readonly HomePage _homePage;
        private readonly IScreenshotCapturer _screenshotCapturer;
        private readonly ILogger<Hooks> _logger;

        public Hooks(
            IWebDriver driver,
            HomePage homePage,
            IScreenshotCapturer screenshotCapturer,
            ILogger<Hooks> logger)
        {
            _driver = driver;
            _homePage = homePage;
            _screenshotCapturer = screenshotCapturer;
            _logger = logger;
        }

        [BeforeScenario]
        public void BeforeScenario(ScenarioContext scenarioContext)
        {
            _logger.LogInformation("Starting scenario: {ScenarioTitle}", scenarioContext.ScenarioInfo.Title);
            _homePage.NavigateToHome();
        }

        [AfterScenario]
        public void AfterScenario(ScenarioContext scenarioContext)
        {
            if (scenarioContext.TestError != null)
            {
                _screenshotCapturer.Capture(_driver, scenarioContext.ScenarioInfo.Title);
            }

            _logger.LogInformation(
                "Finished scenario: {ScenarioTitle} with result {Result}",
                scenarioContext.ScenarioInfo.Title,
                scenarioContext.TestError == null ? "Passed" : "Failed");

            _driver.Quit();
            _driver.Dispose();
        }
    }
}
