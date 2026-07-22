using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Locators_for_Web_Elements
{
    public class ElementInteractor : IElementInteractor
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        private readonly By PrivacyBanner = By.CssSelector("div[role='dialog'][aria-label='Privacy']");
        private readonly By CookiesAcceptanceLocator = By.Id("onetrust-accept-btn-handler");

        public ElementInteractor(IWebDriver driver, WebDriverWait wait)
        {
            _driver = driver;
            _wait = wait;
        }

        public void ClickElement(By locator)
        {
            FindElementByLocator(locator).Click();
        }

        public void TypeIntoElement(By locator, string text)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(text);
            FindElementByLocator(locator).SendKeys(text);
        }

        public IWebElement FindElementByLocator(By locator)
        {
            return _wait.Until(driver =>
            {
                try
                {
                    var element = driver.FindElement(locator);
                    return element.Displayed && element.Enabled ? element : null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            });
        }

        public IWebElement FindPresentElementByLocator(By locator)
        {
            return _wait.Until(driver =>
            {
                try
                {
                    return driver.FindElement(locator);
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            });
        }

        public IEnumerable<IWebElement> FindElementsByLocator(By locator, IWebElement scope)
        {
            return _wait.Until(driver =>
            {
                try
                {
                    var elements = scope.FindElements(locator);
                    return elements.All(d => d.Displayed) && elements.All(d => d.Enabled) ? elements : null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            });
        }

        public IEnumerable<IWebElement> FindElementsByLocator(By locator)
        {
            return _wait.Until(driver =>
            {
                try
                {
                    var elements = driver.FindElements(locator);
                    return elements.All(d => d.Displayed) && elements.All(d => d.Enabled) ? elements : null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
            });
        }

        public void WaitForUrlToContain(string phrase)
        {
            _wait.Until(driver =>
                driver.Url.Contains(phrase, StringComparison.OrdinalIgnoreCase));
        }

        public bool ElementContainsPhrase(IWebElement element, string phrase)
        {
            return element.Text.Contains(phrase, StringComparison.OrdinalIgnoreCase);
        }

        public void AcceptCookies()
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
        }
    }
}