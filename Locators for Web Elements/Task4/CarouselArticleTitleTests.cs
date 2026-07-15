using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using EpamTests.PageObjects;

namespace EpamTests.Tests
{
    [TestFixture]
    public class CarouselArticleTitleTests
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;

        [SetUp]
        public void SetUp()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Window.Maximize();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
        }

        [Test]
        public void ArticleTitle_MatchesCarouselSlideTitle_AfterSwiping()
        {
            var home = new HomePage(_driver, _wait).GoTo().AcceptCookies().SelectInsights();

            home.Carousel.Swipe(2);
            var expectedTitle = home.Carousel.GetActiveSlideTitle();

            home.Carousel.ClickReadMoreOnActiveSlide();

            var actualTitle = new ArticlePage(_driver, _wait).GetTitle();

            Assert.That(actualTitle, Is.EqualTo(expectedTitle));
        }

        [TearDown]
        public void TearDown()
        {
            _driver?.Quit();
            _driver.Dispose();
        }

    }
}