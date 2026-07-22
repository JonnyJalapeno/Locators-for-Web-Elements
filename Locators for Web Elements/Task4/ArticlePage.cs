using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Locators_for_Web_Elements
{
    public class ArticlePage
    {
        private readonly WebDriverWait Wait;

        private readonly By _heading = By.CssSelector("h1"); // verify once on real article page

        public ArticlePage(WebDriverWait wait)
        {
            Wait = wait;
        }

        public string GetTitle()
        {
            var el = Wait.Until(d => d.FindElement(_heading));
            return HomeCarousel.NormalizeText(el.Text);
        }
    }
}