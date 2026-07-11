using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Locators_for_Web_Elements
{
    public class Tests
    {
        private Epam EpamPom { get; set; }

        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            IWebDriver driver = new ChromeDriver(options);
            this.EpamPom = new Epam(driver);
            this.EpamPom.Driver.Navigate().GoToUrl(EpamPom.WebUrl);
        }

        [TestCase("blockchain", "Serbia")]
        [TestCase("java", "Poland")]
        public void Task1(string keyword, string country)
        {
            EpamPom.AcceptCookies()
            .FindAndClickCareers()
            .FindAndClickSearchCareers()
            .AcceptCookies()
            .FindAndTypeIntoRoleOrKeywordSearch(keyword)
            .SelectCountryFromDropdown(country)
            .ClickRemoteButton()
            .ClickTheSearchButton()
            .ExpandJobDescription();
            Assert.That(EpamPom.JobDescriptionContainsKeyword(keyword));
        }

        [TearDown]
        public void Teardown()
        {
            EpamPom.Driver.Quit();
        }
    }
}
