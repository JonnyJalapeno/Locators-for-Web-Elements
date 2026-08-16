using Locators_for_Web_Elements.Business;
using Locators_for_Web_Elements.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework.Interfaces;

namespace Locators_for_Web_Elements.Tests.API
{
    // Mirrors Tests/TestsBase.cs: every concrete fixture only contains its
    // [Test] methods, while spinning up/tearing down the DI container and
    // logging test start/end lives here in one place. Marked with the "API"
    // category so every derived fixture/test automatically inherits it.
    [Category("API")]
    [Parallelizable(ParallelScope.Self)]
    public abstract class ApiTestsBase
    {
        protected ServiceProvider Services { get; private set; } = null!;
        protected UsersApiClient UsersApiClient { get; private set; } = null!;

        private ILogger<ApiTestsBase> _logger = null!;

        [SetUp]
        public void BaseSetUp()
        {
            var configuration = ConfigurationFactory.Build();

            Services = new ServiceCollection()
                .AddApiCoreServices(configuration)
                .AddBusinessApiServices(configuration)
                .BuildServiceProvider();

            UsersApiClient = Services.GetRequiredService<UsersApiClient>();
            _logger = Services.GetRequiredService<ILogger<ApiTestsBase>>();

            _logger.LogInformation("Starting API test: {TestName}", TestContext.CurrentContext.Test.FullName);
        }

        [TearDown]
        public void BaseTearDown()
        {
            var testResult = TestContext.CurrentContext.Result.Outcome.Status;

            _logger.LogInformation(
                "Finished API test: {TestName} with result {Result}",
                TestContext.CurrentContext.Test.FullName, testResult);

            Services.Dispose();
        }
    }
}
