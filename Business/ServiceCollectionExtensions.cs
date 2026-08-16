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

        // Registers the API-side Business layer (domain models + API
        // clients). Independent from AddBusinessServices/AddCoreTafServices
        // above so that pure API test projects don't have to pull in Selenium.
        public static IServiceCollection AddBusinessApiServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddTransient<UsersApiClient>();

            return services;
        }
    }
}
