using Locators_for_Web_Elements.Core.Api;
using Locators_for_Web_Elements.Core.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using RestSharp;
using RestSharp.Extensions.DependencyInjection;

namespace Locators_for_Web_Elements.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCoreTafServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var tafConfig = ConfigureLogging(services, configuration);

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

        // Registers everything the API side of the TAF needs: NLog-backed
        // logging (console + file, min level driven by "Taf:Logging:MinLevel",
        // same as the UI TAF), a RestSharp IRestClient (backed by
        // IHttpClientFactory) wired up via
        // RestSharp.Extensions.DependencyInjection with the base URL/timeout
        // taken from the "Api" configuration section, and the base ApiClient
        // that all Business-layer API clients depend on.
        public static IServiceCollection AddApiCoreServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            ConfigureLogging(services, configuration);

            services.Configure<TafConfig>(configuration.GetSection("Taf"));

            var apiConfig = configuration.GetSection("Api").Get<ApiConfig>() ?? new ApiConfig();
            services.Configure<ApiConfig>(configuration.GetSection("Api"));

            if (string.IsNullOrWhiteSpace(apiConfig.BaseUrl))
            {
                throw new InvalidOperationException("Api:BaseUrl was not set in the config file.");
            }

            services.AddRestClient(options =>
            {
                options.BaseUrl = new Uri(apiConfig.BaseUrl);
                options.Timeout = TimeSpan.FromSeconds(apiConfig.TimeoutSeconds);
                options.ThrowOnAnyError = false;
            });

            services.AddSingleton<IApiClient, ApiClient>();

            return services;
        }

        // Shared by both the UI and API registration paths so that log
        // targets/levels/format stay identical no matter which part of the
        // TAF is running.
        private static TafConfig ConfigureLogging(IServiceCollection services, IConfiguration configuration)
        {
            var tafConfig = configuration.GetSection("Taf").Get<TafConfig>() ?? new TafConfig();
            LoggingConfigurator.Configure(tafConfig);

            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                builder.AddNLog();
            });

            return tafConfig;
        }
    }
}
