using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;

namespace Locators_for_Web_Elements
{
    public class CareerPageTests
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
            Services.Dispose();
        }
    }
}
