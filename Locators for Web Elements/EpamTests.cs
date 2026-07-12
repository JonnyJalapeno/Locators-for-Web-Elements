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

        [Ignore("Not implemented yet")]
        [TestCase("blockchain", "Serbia")]
        [TestCase("python", "Uzbekistan")]
        public void Task1(string keyword, string country)
        {
            EpamPom.AcceptCookies()
            .FindAndClickCareers()
            .FindAndClickSearchCareers()
            .AcceptCookies()
            .SelectCountryFromDropdown(country)
            .ClickRemoteButton()
            .FindAndTypeIntoRoleOrKeywordSearch(keyword)
            .ClickTheSearchButton()
            .ExpandJobDescription();
            //Thread.Sleep(5000);
            Assert.That(EpamPom.JobDescriptionContainsKeyword(keyword));
        }

        //[Ignore("Not implemented yet")]
        [TestCase("BLOCKCHAIN")]
        [TestCase("Cloud")]
        [TestCase("Automation")]
        public void Task2(string s1)
        {
            EpamPom.AcceptCookies()
            .ClickMagnifierSearch()
            .InputPhraseIntoMagnifierSearch(s1)
            .ClickFindButton();
            Thread.Sleep(5000);
            Assert.That(EpamPom.CheckLinksForSearchTerm(s1));
        }

        [TearDown]
        public void Teardown()
        {
            EpamPom.Driver.Quit();
        }
    }
}
