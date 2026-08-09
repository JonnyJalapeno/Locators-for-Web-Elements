using Locators_for_Web_Elements.Business;
using Locators_for_Web_Elements.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using Reqnroll;
using Reqnroll.BoDi;

namespace Locators_for_Web_Elements.Tests.BDD.Support
{
    // BDD counterpart of Tests/TestsBase.cs: spins up the same Core+Business DI
    // container per scenario, resolves the shared Page Objects, navigates home,
    // and tears the browser+container down again. Everything resolved here is
    // registered into Reqnroll's scenario-scoped IObjectContainer so step
    // definition classes can simply constructor-inject HomePage, IWebDriver, etc.
    [Binding]
    public class Hooks
    {
        private readonly IObjectContainer _objectContainer;
        private ServiceProvider? _services;
        private IWebDriver? _driver;
        private ILogger<Hooks>? _logger;

        public Hooks(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        [BeforeScenario]
        public void BeforeScenario(ScenarioContext scenarioContext)
        {
            var configuration = ConfigurationFactory.Build();

            _services = new ServiceCollection()
                .AddCoreTafServices(configuration)
                .AddBusinessServices(configuration)
                .BuildServiceProvider();

            _driver = _services.GetRequiredService<IWebDriver>();
            _logger = _services.GetRequiredService<ILogger<Hooks>>();

            var homePage = _services.GetRequiredService<HomePage>();

            _objectContainer.RegisterInstanceAs(_driver);
            _objectContainer.RegisterInstanceAs(homePage);
            _objectContainer.RegisterInstanceAs(_services.GetRequiredService<IPageFactory>());
            _objectContainer.RegisterInstanceAs(_services.GetRequiredService<IElementInteractor>());
            _objectContainer.RegisterInstanceAs(_services.GetRequiredService<IOptions<TafConfig>>().Value);

            _logger.LogInformation("Starting scenario: {ScenarioTitle}", scenarioContext.ScenarioInfo.Title);

            homePage.NavigateToHome();
        }

        [AfterScenario]
        public void AfterScenario(ScenarioContext scenarioContext)
        {
            if (scenarioContext.TestError != null && _driver != null && _services != null)
            {
                var screenshotCapturer = _services.GetRequiredService<IScreenshotCapturer>();
                screenshotCapturer.Capture(_driver, scenarioContext.ScenarioInfo.Title);
            }

            _logger?.LogInformation(
                "Finished scenario: {ScenarioTitle} with result {Result}",
                scenarioContext.ScenarioInfo.Title,
                scenarioContext.TestError == null ? "Passed" : "Failed");

            _driver?.Quit();
            _driver?.Dispose();
            _services?.Dispose();
        }
    }
}
