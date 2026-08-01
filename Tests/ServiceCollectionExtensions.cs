using Locators_for_Web_Elements.Business;
using Locators_for_Web_Elements.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Locators_for_Web_Elements.Tests
{
    public static class ServiceCollectionExtensions
    {
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
