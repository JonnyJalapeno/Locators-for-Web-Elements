using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Locators_for_Web_Elements
{
    public abstract class BaseComponent<TSelf> where TSelf : BaseComponent<TSelf>
    {
        protected readonly IWebDriver Driver;
        protected readonly WebDriverWait Wait;

        private readonly By PrivacyBanner = By.CssSelector("div[role='dialog'][aria-label='Privacy']");
        private readonly By CookiesAcceptanceLocator = By.Id("onetrust-accept-btn-handler");

        protected BaseComponent(IWebDriver driver, WebDriverWait wait)
        {
            Driver = driver;
            Wait = wait;
        }

        protected TSelf FindAndClick(By locator)
        {
            FindElementByLocator(locator).Click();
            return (TSelf)this;
        }

        protected TPage FindAndClick<TPage>(By locator)
         where TPage : BaseComponent<TPage>
        {
            FindElementByLocator(locator).Click();

            return (TPage)Activator.CreateInstance(typeof(TPage), Driver, Wait)!;
        }

        protected TSelf FindAndType(By locator, string text)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(text);
            FindElementByLocator(locator).SendKeys(text);
            return (TSelf)this;
        }

        protected IWebElement FindElementByLocator(By locator)
        {
            return Wait.Until(driver =>
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

        protected IEnumerable<IWebElement> FindElementsByLocator(By locator, IWebElement scope)
        {
            return Wait.Until(driver =>
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

        protected IEnumerable<IWebElement> FindElementsByLocator(By locator)
        {
            return Wait.Until(driver =>
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

        protected void WaitForUrlToContain(string phrase)
        {
            Wait.Until(driver =>
                driver.Url.Contains(phrase, StringComparison.OrdinalIgnoreCase));
        }

        protected static bool ElementContainsPhrase(IWebElement element, string phrase)
        {
            return element.Text.Contains(phrase, StringComparison.OrdinalIgnoreCase);
        }

        public TSelf AcceptCookies()
        {
            try
            {
                var button = Wait.Until(driver =>
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

                Wait.Until(driver =>
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
            return (TSelf)this;
        }
    }
}
