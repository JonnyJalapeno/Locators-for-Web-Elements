using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Locators_for_Web_Elements
{
    public class Task1And2RefactoredTests
    {
        private Task1and2Refactored EpamPom { get; set; }

        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            IWebDriver driver = new ChromeDriver(options);
            EpamPom = new Task1and2Refactored(driver);
            EpamPom.NavigateToHome();
        }

        [Ignore("Not implemented yet")]
        [TestCase("blockchain", "Serbia")]
        [TestCase("python", "Uzbekistan")]
        public void Task1(string keyword, string country)
        {
            EpamPom.AcceptCookies()
            .ClickCareers()
            .ClickSearchCareers()
            .AcceptCookies()
            .SelectCountryFromDropdown(country)
            .ClickRemoteButton()
            .TypeIntoRoleOrKeywordSearch(keyword)
            .ClickTheSearchButton()
            .ExpandJobDescription();
            Assert.That(EpamPom.JobDescriptionContainsKeyword(keyword));
        }

        [Ignore("Not implemented yet")]
        [TestCase("BLOCKCHAIN")]
        [TestCase("Cloud")]
        [TestCase("Automation")]
        public void Task2(string s1)
        {
            EpamPom.AcceptCookies()
            .ClickMagnifierSearch()
            .InputPhraseIntoMagnifierSearch(s1)
            .ClickFindButton();
            Assert.That(EpamPom.CheckLinksForSearchTerm(s1));
        }

        [TearDown]
        public void Teardown()
        {
            EpamPom.Quit();
        }
    }
}
