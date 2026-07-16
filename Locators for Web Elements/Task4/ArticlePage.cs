using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using EpamTests.PageObjects.Components;

namespace EpamTests.PageObjects
{
    public class ArticlePage
    {
        private readonly WebDriverWait _wait;
        private readonly By _heading = By.CssSelector("h1"); // verify once on real article page
        private readonly IWebDriver _driver;

        public ArticlePage(IWebDriver driver, WebDriverWait wait)
        {
            _driver = driver;
            _wait = wait;
        }

        public string GetTitle()
        {
            var el = _wait.Until(d => d.FindElement(_heading));
            return HomeCarousel.NormalizeText(el.Text);
        }
    }
}