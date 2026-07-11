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
            this.EpamPom.ExplicitWait.Until(d =>
            ((IJavaScriptExecutor)d)
            .ExecuteScript("return document.readyState")
            .Equals("complete"));
            //Console.WriteLine(EpamPom.WebUrl);
        }

        [Test]
        public void Test1()
        {
            EpamPom.AcceptCookies();
            EpamPom.FindAndClickCareers();
            EpamPom.FindAndClickSearchCareers();
            EpamPom.AcceptCookies();
            EpamPom.FindAndTypeIntoRoleOrKeywordSearch("blockchain");
            EpamPom.SelectCountryFromDropdown("Serbia");
            EpamPom.ClickRemoteButton();
            EpamPom.ClickTheSearchButton();
            EpamPom.ExpandJobDescription();
            Assert.That(EpamPom.JobDescriptionContainsKeyword("blockchain"));
        }

        [TearDown]
        public void Teardown()
        {
            EpamPom.Driver.Quit();
        }
    }
}
