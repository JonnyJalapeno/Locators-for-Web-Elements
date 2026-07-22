using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Locators_for_Web_Elements
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSeleniumTestServices(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<ChromeOptions>? configureChromeOptions = null)
        {
            services.Configure<EpamConfig>(configuration);

            services.AddSingleton<IWebDriver>(_ =>
            {
                var options = new ChromeOptions();
                options.AddArgument("--start-maximized");
                configureChromeOptions?.Invoke(options);
                return new ChromeDriver(options);
            });

            services.AddSingleton(sp =>
                new WebDriverWait(sp.GetRequiredService<IWebDriver>(), TimeSpan.FromSeconds(10)));

            services.AddSingleton<IPageFactory, PageFactory>();
            services.AddSingleton<IElementInteractor, ElementInteractor>();

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
