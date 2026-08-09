using Locators_for_Web_Elements.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Locators_for_Web_Elements.Business
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<EpamConfig>(configuration);

            services.AddTransient<HomePage>();
            services.AddTransient<CareerPage>();
            services.AddTransient<SearchPage>();
            services.AddTransient<InsightsPage>();
            services.AddTransient<ArticlePage>();
            services.AddTransient<HomeCarousel>();
            services.AddTransient<ServicesPage>();

            return services;
        }

        // Single composition root for "everything a Selenium test suite needs" —
        // Core (driver/config/logging/wait) + Business (Page Objects). Both the
        // NUnit suite (Tests/TestsBase.cs) and the Reqnroll suite
        // (Tests.BDD/Support/Hooks.cs) call this instead of each re-declaring the
        // same two calls, so there is one place that defines the test environment.
        public static IServiceCollection AddSeleniumTestServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddCoreTafServices(configuration)
                .AddBusinessServices(configuration);

            return services;
        }
    }
}
