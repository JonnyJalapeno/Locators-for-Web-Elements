using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Locators_for_Web_Elements.Core
{
    public class ElementInteractor : IElementInteractor
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        private readonly ILogger<ElementInteractor> _logger;

        public ElementInteractor(IWebDriver driver, WebDriverWait wait, ILogger<ElementInteractor> logger)
        {
            _driver = driver;
            _wait = wait;
            _logger = logger;
        }

        public void ClickElement(By locator)
        {
            _logger.LogInformation("Clicking element {Locator}", locator);
            FindElementByLocator(locator).Click();
        }

        public void ClickElementSafely(By locator)
        {
            _logger.LogInformation("Safely clicking element {Locator}", locator);

            var element = _wait.Until(driver =>
            {
                IWebElement candidate;
                try
                {
                    candidate = driver.FindElement(locator);
                }
                catch (NoSuchElementException) { return null; }
                catch (StaleElementReferenceException) { return null; }

                if (!candidate.Displayed || !candidate.Enabled) return null;

                return IsElementOnTop(driver, candidate) ? candidate : null;
            });

            element.Click();
        }

        public void TypeIntoElement(By locator, string text)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(text);
            _logger.LogInformation("Typing '{Text}' into element {Locator}", text, locator);
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

        public void AcceptCookies(By acceptButtonLocator, By privacyDialogLocator)
        {
            try
            {
                var button = _wait.Until(driver =>
                {
                    IWebElement element;
                    try { element = driver.FindElement(acceptButtonLocator); }
                    catch (NoSuchElementException) { return null; }

                    if (!element.Displayed || !element.Enabled) return null;

                    // Confirm it's actually on top / not covered
                    return IsElementOnTop(driver, element) ? element : null;
                });

                button.Click();
                _logger.LogInformation("Cookie banner accepted");

                _wait.Until(driver =>
                {
                    var dialogs = driver.FindElements(privacyDialogLocator);

                    if (dialogs.Count == 0)
                        return true;

                    return !dialogs[0].Displayed;
                });
            }
            catch (WebDriverTimeoutException)
            {
                _logger.LogInformation("Cookie banner not present");
            }
        }

        public bool IsElementOnTop(IWebDriver driver, IWebElement element)
        {
            var js = (IJavaScriptExecutor)driver;
            return (bool)js.ExecuteScript(@"
                var rect = arguments[0].getBoundingClientRect();
                var x = rect.left + rect.width/2, y = rect.top + rect.height/2;
                var el = document.elementFromPoint(x, y);
                return arguments[0].contains(el);", element);
        }

        public List<IWebElement> FindContainerAndReturnItsElements(By containerLocator, By elementLocator)
        {
            return _wait.Until(driver =>
            {
                var container = FindElementByLocator(containerLocator);
                var articles = FindElementsByLocator(elementLocator, container).ToList();
                return articles.Count > 0 ? articles : null;
            });
        }

        public void WaitForPreloaderToDisappear(By preloaderLocator)
        {
            _wait.Until(driver =>
            {
                var preloader = FindPresentElementByLocator(preloaderLocator);
                return preloader.GetAttribute("class").Contains("hidden");
            });
        }
    }
}
