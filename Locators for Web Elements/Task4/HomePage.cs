using EpamTests.PageObjects.Components;
using Locators_for_Web_Elements;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace EpamTests.PageObjects
{
    public class HomePage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        private readonly By _insightsMenuItem = By.XPath("//nav//a[normalize-space()='Insights']");
        private readonly By PrivacyBanner = By.CssSelector("div[role='dialog'][aria-label='Privacy']");
        private readonly By CookiesAcceptanceLocator = By.Id("onetrust-accept-btn-handler");

        public HomeCarousel Carousel { get; }

        public HomePage(IWebDriver driver, WebDriverWait wait)
        {
            _driver = driver;
            _wait = wait;
            Carousel = new HomeCarousel(driver, wait);
        }

        public HomePage GoTo()
        {
            _driver.Navigate().GoToUrl("https://www.epam.com/");
            return this;
        }

        public HomePage SelectInsights()
        {
            var insightsLink = _wait.Until(d =>
                d.FindElements(_insightsMenuItem).FirstOrDefault(e => e.Displayed && e.Enabled));

            if (insightsLink == null)
                throw new NoSuchElementException("No visible 'Insights' nav link found.");

            insightsLink.Click();
            return this;
        }

        public HomePage AcceptCookies()
        {
            try
            {
                var button = _wait.Until(driver =>
                {
                    IWebElement element;
                    try { element = driver.FindElement(CookiesAcceptanceLocator); }
                    catch (NoSuchElementException) { return null; }

                    if (!element.Displayed || !element.Enabled) return null;

                    // Confirm it's actually on top / not covered
                    var js = (IJavaScriptExecutor)driver;
                    var isClickable = (bool)js.ExecuteScript(@"
                        var rect = arguments[0].getBoundingClientRect();
                        var x = rect.left + rect.width/2, y = rect.top + rect.height/2;
                        var el = document.elementFromPoint(x, y);
                        return arguments[0].contains(el);", element);

                    return isClickable ? element : null;
                });

                button.Click();

                _wait.Until(driver =>
                {
                    var dialogs = driver.FindElements(PrivacyBanner);

                    if (dialogs.Count == 0)
                        return true;

                    return !dialogs[0].Displayed;
                });
            }
            catch (WebDriverTimeoutException)
            {
                // Cookie banner not present
            }
            return this;
        }
    }
}