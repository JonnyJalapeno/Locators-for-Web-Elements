

using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;

namespace Locators_for_Web_Elements
{
    public class SearchPageTests
    {
        private ServiceProvider Services { get; set; }
        private IWebDriver Driver { get; set; }
        private HomePage HomePage { get; set; }

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
            Services.Dispose();
        }
    }
}
