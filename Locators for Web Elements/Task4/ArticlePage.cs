using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Locators_for_Web_Elements
{
    public class ArticlePage : BaseComponent<ArticlePage>
    {
        private readonly By _heading = By.CssSelector("h1"); // verify once on real article page

        public ArticlePage(IWebDriver driver, WebDriverWait wait) : base(driver, wait)
        {
        }

        public string GetTitle()
        {
            var el = Wait.Until(d => d.FindElement(_heading));
            return HomeCarousel.NormalizeText(el.Text);
        }
    }
}