using EpamTests.PageObjects.Components;
using Microsoft.Extensions.DependencyInjection; // <-- fixed
using System;

namespace Locators_for_Web_Elements.Shared_Classes
{
    public class PageFactory : IPageFactory
    {
        private readonly IServiceProvider _services;

        public PageFactory(IServiceProvider services) => _services = services;

        public HomeCarousel CreateInsightsPage() => _services.GetRequiredService<HomeCarousel>();
    }
}