using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;

namespace Locators_for_Web_Elements
{
    [TestFixture]
    public class CarouselArticleTitleTests
    {
        private ServiceProvider Services { get; set; }
        private HomePage HomePage { get; set; }
        private IWebDriver Driver { get; set; }

        [SetUp]
        public void Setup()
        {
            Services = new ServiceCollection()
                .AddSeleniumTestServices(ConfigurationFactory.Build())
                .BuildServiceProvider();

            Driver = Services.GetRequiredService<IWebDriver>();
            HomePage = Services.GetRequiredService<HomePage>();
            HomePage.NavigateToHome();
        }

        [Test]
        public void ArticleTitle_MatchesCarouselSlideTitle_AfterSwiping()
        {
            var insightPage = HomePage.AcceptCookies().ClickInsights();

            insightPage.Carousel.Swipe(2);
            var expectedTitle = insightPage.Carousel.GetActiveSlideTitle();

            var actualTitle = insightPage.Carousel.ClickReadMoreOnActiveSlide().GetTitle();

            Assert.That(actualTitle, Is.EqualTo(expectedTitle));
        }

        [TearDown]
        public void TearDown()
        {
            Driver.Quit();
            Driver.Dispose();
            Services.Dispose();
        }

    }
}