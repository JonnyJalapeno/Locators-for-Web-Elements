using Locators_for_Web_Elements.Business;
using Locators_for_Web_Elements.Business.Api;
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
    //
    // The DI container (and the IRestClient it hands out) is built ONCE per
    // fixture in [OneTimeSetUp], not per test - IRestClient is backed by
    // IHttpClientFactory, which is meant to be reused across many calls, not
    // rebuilt for every single request.
    [Category("API")]
    [Parallelizable(ParallelScope.Self)]
    public abstract class ApiTestsBase
    {
        protected UsersApiClient UsersApiClient { get; private set; } = null!;
        protected ApiResourceClient ApiResourceClient { get; private set; } = null!;
        private ServiceProvider _services = null!;
        private ILogger<ApiTestsBase> _logger = null!;

        [OneTimeSetUp]
        public void BaseOneTimeSetUp()
        {
            var configuration = ConfigurationFactory.Build();

            _services = new ServiceCollection()
                .AddApiCoreServices(configuration)
                .AddBusinessApiServices(configuration)
                .BuildServiceProvider();

            UsersApiClient = _services.GetRequiredService<UsersApiClient>();
            ApiResourceClient = _services.GetRequiredService<ApiResourceClient>();
            _logger = _services.GetRequiredService<ILogger<ApiTestsBase>>();
        }

        [OneTimeTearDown]
        public void BaseOneTimeTearDown()
        {
            _services.Dispose();
        }

        [SetUp]
        public void BaseSetUp()
        {
            _logger.LogInformation("Starting API test: {TestName}", TestContext.CurrentContext.Test.FullName);
        }

        [TearDown]
        public void BaseTearDown()
        {
            var testResult = TestContext.CurrentContext.Result.Outcome.Status;

            _logger.LogInformation(
                "Finished API test: {TestName} with result {Result}",
                TestContext.CurrentContext.Test.FullName, testResult);
        }
    }
}
