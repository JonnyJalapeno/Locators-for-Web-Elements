using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Locators_for_Web_Elements
{
    public class CareerPageTests
    {
        private HomePage HomePage { get; set; }
        private IWebDriver Driver { get; set; }

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

        [TestCase("blockchain", "Serbia")]
        //[TestCase("python", "Uzbekistan")]
        public void Task1(string keyword, string country)
        {
            var page = HomePage.AcceptCookies()
            .ClickCareers()
            .ClickSearchCareers()
            .AcceptCookies()
            .SelectCountryFromDropdown(country)
            .ClickRemoteButton()
            .TypeIntoRoleOrKeywordSearch(keyword)
            .ClickTheSearchButton()
            .ExpandJobDescription();
            Assert.That(page.JobDescriptionContainsKeyword(keyword));
        }

        [TearDown]
        public void TearDown()
        {
            Driver.Quit();
            Driver.Dispose();
        }
    }
}
