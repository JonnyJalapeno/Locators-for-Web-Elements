using EpamTests.PageObjects;
using EpamTests.PageObjects.Components;
using Locators_for_Web_Elements.Shared_Classes;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace EpamTests.Tests
{
    [TestFixture]
    public class CarouselArticleTitleTests
    {
        private ServiceProvider _provider;

        [SetUp]
        public void SetUp()
        {
            var services = new ServiceCollection();
            services.AddScoped<IWebDriver>(_ =>
            {
                var driver = new ChromeDriver();
                driver.Manage().Window.Maximize();
                return driver;
            });
            services.AddScoped(sp => new WebDriverWait(sp.GetRequiredService<IWebDriver>(), TimeSpan.FromSeconds(15)));
            services.AddScoped<HomePage>();
            services.AddScoped<HomeCarousel>();
            services.AddScoped<ArticlePage>();
            services.AddScoped<IPageFactory, PageFactory>();
            _provider = services.BuildServiceProvider();
        }

        [Test]
        public void ArticleTitle_MatchesCarouselSlideTitle_AfterSwiping()
        {
            var homePage = _provider.GetRequiredService<HomePage>();
            var carousel = homePage.GoTo().AcceptCookies().SelectInsights();

            carousel.Swipe(2);
            var expectedTitle = carousel.GetActiveSlideTitle();

            carousel.ClickReadMoreOnActiveSlide();

            var actualTitle = _provider.GetRequiredService<ArticlePage>().GetTitle();

            Assert.That(actualTitle, Is.EqualTo(expectedTitle));
        }

        [TearDown]
        public void TearDown()
        {
            _provider.GetRequiredService<IWebDriver>().Quit();
            _provider.Dispose();
        }
    }
}