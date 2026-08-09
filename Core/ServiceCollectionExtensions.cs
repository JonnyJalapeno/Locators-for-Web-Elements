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

            // Scoped (not Singleton): Reqnroll's native DI integration
            // (Reqnroll.Microsoft.Extensions.DependencyInjection) builds ONE root
            // container per test session and resolves a fresh scope per scenario.
            // Scoped here means each scenario gets its own browser/wait/page-factory
            // instance. This is also safe for the NUnit suite (TestsBase.cs), which
            // builds a brand-new root ServiceProvider per test: resolving a Scoped
            // service directly from a root provider (no explicit child scope)
            // behaves identically to Singleton for that root's lifetime.
            services.AddScoped<IWebDriver>(sp => sp.GetRequiredService<IBrowserFactory>().Create());

            services.AddScoped(sp =>
            {
                var waitSeconds = sp.GetRequiredService<IOptions<TafConfig>>().Value.ExplicitWaitSeconds;
                return new WebDriverWait(sp.GetRequiredService<IWebDriver>(), TimeSpan.FromSeconds(waitSeconds));
            });

            services.AddScoped<IPageFactory, PageFactory>();
            services.AddScoped<IElementInteractor, ElementInteractor>();
            services.AddSingleton<IScreenshotCapturer, ScreenshotCapturer>();

            return services;
        }
    }
}
