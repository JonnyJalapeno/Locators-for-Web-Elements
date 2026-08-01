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

            return services;
        }
    }
}
