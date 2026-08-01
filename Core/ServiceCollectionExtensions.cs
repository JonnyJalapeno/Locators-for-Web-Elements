using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Locators_for_Web_Elements.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCoreTafServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var tafConfig = configuration.GetSection("Taf").Get<TafConfig>() ?? new TafConfig();
            LoggingConfigurator.Configure(tafConfig);

            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                builder.AddNLog();
            });

            services.Configure<TafConfig>(configuration.GetSection("Taf"));

            services.AddSingleton<IBrowserFactory, ChromeBrowserFactory>();
            services.AddSingleton<IWebDriver>(sp => sp.GetRequiredService<IBrowserFactory>().Create());

            services.AddSingleton(sp =>
            {
                var waitSeconds = sp.GetRequiredService<IOptions<TafConfig>>().Value.ExplicitWaitSeconds;
                return new WebDriverWait(sp.GetRequiredService<IWebDriver>(), TimeSpan.FromSeconds(waitSeconds));
            });

            services.AddSingleton<IPageFactory, PageFactory>();
            services.AddSingleton<IElementInteractor, ElementInteractor>();
            services.AddSingleton<IScreenshotCapturer, ScreenshotCapturer>();

            return services;
        }
    }
}
