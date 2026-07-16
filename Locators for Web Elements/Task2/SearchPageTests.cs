

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Locators_for_Web_Elements
{
    public class SearchPageTests
    {
        private IWebDriver Driver { get; set; }
        private HomePage HomePage { get; set; }

        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            Driver = new ChromeDriver(options);
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            HomePage = new HomePage(Driver, wait);
            HomePage.NavigateToHome();
        }

        [TestCase("BLOCKCHAIN")]
        //[TestCase("Cloud")]
        //[TestCase("Automation")]
        public void Task2(string s1)
        {
            var page = HomePage.AcceptCookies()
            .ClickMagnifierSearch()
            .InputPhraseIntoMagnifierSearch(s1)
            .ClickFindButton();
            Assert.That(page.CheckAllLinksForSearchTerm(s1));
        }

        [TearDown]
        public void Teardown()
        {
            Driver.Quit();
            Driver.Dispose();
        }
    }
}
