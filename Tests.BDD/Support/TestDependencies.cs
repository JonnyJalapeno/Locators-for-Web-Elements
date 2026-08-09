using Locators_for_Web_Elements.Business;
using Locators_for_Web_Elements.Core;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll.Microsoft.Extensions.DependencyInjection;

namespace Locators_for_Web_Elements.Tests.BDD.Support
{
    // Composition root for the Reqnroll suite, wired through Reqnroll's native
    // Microsoft.Extensions.DependencyInjection integration
    // (Reqnroll.Microsoft.Extensions.DependencyInjection). Every [Binding] class
    // (hooks, step definitions) is auto-added to this container by that plugin,
    // so they can simply constructor-inject HomePage, IWebDriver, etc. directly —
    // no manual IObjectContainer.RegisterInstanceAs bridging required.
    public static class TestDependencies
    {
        [ScenarioDependencies]
        public static IServiceCollection CreateServices()
        {
            var configuration = ConfigurationFactory.Build();

            return new ServiceCollection()
                .AddCoreTafServices(configuration)
                .AddBusinessServices(configuration);
        }
    }
}
