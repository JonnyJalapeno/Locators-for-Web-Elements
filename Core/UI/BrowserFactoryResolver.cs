using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;

namespace Locators_for_Web_Elements.Core.UI
{
    // CI trigger test
    // Picks the concrete IBrowserFactory based on TafConfig.Browser (bound from
    // "Taf:Browser", overridable via the TAF__BROWSER env var so CI can select
    // it per run) and delegates to it. Registered in place of a single
    // hardcoded ChromeBrowserFactory in ServiceCollectionExtensions.
    public class BrowserFactoryResolver : IBrowserFactory
    {
        private readonly IBrowserFactory _inner;

        public BrowserFactoryResolver(IOptions<TafConfig> config, IServiceProvider serviceProvider)
        {
            _inner = config.Value.Browser switch
            {
                BrowserType.Firefox => ActivatorUtilities.CreateInstance<FirefoxBrowserFactory>(serviceProvider),
                BrowserType.Edge => ActivatorUtilities.CreateInstance<EdgeBrowserFactory>(serviceProvider),
                _ => ActivatorUtilities.CreateInstance<ChromeBrowserFactory>(serviceProvider),
            };
        }

        public IWebDriver Create() => _inner.Create();
    }
}
