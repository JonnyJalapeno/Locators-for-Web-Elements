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
            options.AddExcludedArgument("enable-automation");
            options.AddAdditionalOption("useAutomationExtension", false);
            IWebDriver driver = new ChromeDriver(options);
            this.EpamPom = new Epam(driver);
            //Console.WriteLine(EpamPom.WebUrl);
        }

        [Test]
        public void Test1()
        {
            EpamPom.FindAndClickCareers();
            EpamPom.FindAndClickSearchCareers();
            //EpamPom.FindAndTypeIntoRoleOrKeywordSearch("Java");
            EpamPom.SelectCountryFromDropdown("Poland");
            //EpamPom.ClickTheSearchButton();
            Assert.Pass();
        }
    }
}
