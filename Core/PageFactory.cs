using Microsoft.Extensions.DependencyInjection;

namespace Locators_for_Web_Elements.Core
{
    public class PageFactory : IPageFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public PageFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public TPage Create<TPage>() where TPage : notnull =>
            _serviceProvider.GetRequiredService<TPage>();
    }
}
