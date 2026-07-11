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
            //EpamPom.SelectCountryFromDropdown("Poland");
            EpamPom.FindAndTypeIntoRoleOrKeywordSearch("Java");
            EpamPom.ClickRemoteButton();
            EpamPom.ClickTheSearchButton();
            Assert.Pass();
        }

        [TearDown]
        public void Teardown()
        {
            EpamPom.Driver.Quit();
        }
    }
}
