using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace Locators_for_Web_Elements
{
    public class Epam
    {
        private struct EpamConfig
        {
            public string WebUrl { get; set; }
        }
        private string WebUrl { get; set; }
        public IWebDriver Driver { get; init; }
        private WebDriverWait ExplicitWait { get; init; }

        private readonly By CookiesAcceptanceLocator = By.Id("onetrust-accept-btn-handler");
        private readonly By CareerLocator = By.LinkText("Careers"); //LinkText locator
        private readonly By SearchCareersLocator = By.XPath("//div[@data-gtm-category='job_search_redirect']/descendant::a"); //XPath locator with axes
        private readonly By SearchRoleOrKeyword = By.Name("search"); //Name locator
        private readonly By SearchButtonCareerPage = By.XPath("//button[@name='submit_search_box_button' and @type='submit']"); //XPath locator with operator[and]
        private readonly By CountryDropdownButton = By.CssSelector("input[aria-label='Choose your country']");
        private readonly By SelectCountryListbox = By.XPath("//div[@role='listbox']");
        private By CountryOption(string country) => By.XPath($"//div[@role='option' and .//span[normalize-space(.)='{country}']]");
        private readonly By RemoteCheckbox = By.XPath("//fieldset[@aria-labelledby='Workplace type-filter-title']//label[.//span[text()='Remote']]");
        private readonly By ExpandJobButton = By.XPath("//div[contains(@class, 'JobCard')]//span[@data-testid='accordion-section-header-icon-container']");
        private readonly By JobDescriptionContainer = By.XPath("//div[@data-testid='categories-container']");
        private readonly By JobDescriptionParagraphs = By.XPath("//div[@data-testid='rich-text']");
        private readonly By SearchButtonMainPage = By.XPath("//button[contains(@class, 'header-search__button')]");
        private readonly By SearchInputMainPage = By.Id("new_form_search");
        private readonly By FindButton = By.XPath("//div[contains(@class, 'search-results__action-section')]//button");
        private readonly By ArticleLinks = By.XPath("//a[contains(@class,'search-results__title-link')]");
        private readonly By ArticleParagraphs = By.XPath("//p[contains(@class,'search-results__description')]");
        private readonly By SearchResultContainer = By.XPath("//div[contains(@class, 'search-results__items')]");
        private readonly By SearchResultMore = By.XPath("//a[contains(@class,'search-results__view-more') and not(contains(concat(' ', normalize-space(@class), ' '), ' hidden '))]");

        public Epam(IWebDriver driver)
        {
            LoadAndInitializeUrl();
            Driver = driver;
            ExplicitWait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
        }

        public Epam NavigateToHome()
        {
            Driver.Navigate().GoToUrl(WebUrl);
            return this;
        }

        public void Quit() => Driver.Quit();

        private Epam Click(By locator)
        {
            FindElementByLocator(locator).Click();
            return this;
        }

        private Epam Type(By locator, string text)
        {
            FindElementByLocator(locator).SendKeys(text);
            return this;
        }

        private IWebElement? TryFindElement(By locator)
        {
            var elements = Driver.FindElements(locator);
            return elements.FirstOrDefault();
        }

        private IWebElement FindElementByLocator(By locator)
        {
            return ExplicitWait.Until(driver =>
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

        private IEnumerable<IWebElement> FindElementsByLocator(By locator)
        {
            return ExplicitWait.Until(driver =>
            {
                try
                {
                    var elements = driver.FindElements(locator);
                    return elements.All(d => d.Displayed) && elements.All(d=>d.Enabled) ? elements : null;
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

        private IEnumerable<IWebElement> FindElementsByLocator(By locator, IWebElement scope) =>
        ExplicitWait.Until(driver =>
        {
            try
            {
                var elements = scope.FindElements(locator);
                return elements.Any() && elements.All(d => d.Displayed) && elements.All(d => d.Enabled) ? elements : null;
            }
            catch (StaleElementReferenceException) { return null; }
        });

        private void LoadAndInitializeUrl()
        {
            string fileName = "appsettings.json";
            try
            {
                using FileStream openStream = File.OpenRead(fileName);
                EpamConfig config = JsonSerializer.Deserialize<EpamConfig>(openStream);
                WebUrl = config.WebUrl;
            }
            catch (FileNotFoundException ex)
            {
                throw new InvalidOperationException($"Could not load config file '{fileName}'.", ex);
            }
            catch (ArgumentNullException ex)
            {
                throw new InvalidOperationException("WebUrl was not set in the config file.", ex);
            }
        }

        //We can't just click accept and wait,
        //banner has animation and obscures
        //other elements and intercepting the clicks
        public Epam AcceptCookies()
        {
            try
            {
                var button = ExplicitWait.Until(driver =>
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

                ExplicitWait.Until(driver =>
                {
                    var dialogs = driver.FindElements(
                        By.CssSelector("div[role='dialog'][aria-label='Privacy']")
                    );

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

        public Epam ClickCareers() => Click(CareerLocator);

        public Epam ClickSearchCareers() => Click(SearchCareersLocator);

        public Epam TypeIntoRoleOrKeywordSearch(string phrase = "") => Type(SearchRoleOrKeyword, phrase);

        public Epam ClickTheSearchButton() => Click(SearchButtonCareerPage);

        public Epam SelectCountryFromDropdown(string countryName)
        {
            var input = FindElementByLocator(CountryDropdownButton);
            input.Click();
            input.SendKeys(countryName + Keys.Enter);
            ExplicitWait.Until(driver =>
                driver.Url.Contains(countryName, StringComparison.OrdinalIgnoreCase));

            return this;
        }

        public Epam ClickRemoteButton() => Click(RemoteCheckbox);

        public Epam ExpandJobDescription() => Click(ExpandJobButton);

        public bool JobDescriptionContainsKeyword(string phrase = "")
        {
            var container = FindElementByLocator(JobDescriptionContainer);
            var paragraphs = container.FindElements(JobDescriptionParagraphs);
            return paragraphs.Any(p => p.Text.Contains(
                phrase,
                StringComparison.OrdinalIgnoreCase
            ));
        }

        public Epam ClickMagnifierSearch() => Click(SearchButtonMainPage);

        public Epam InputPhraseIntoMagnifierSearch(string phrase = "") => Type(SearchInputMainPage, phrase);

        public Epam ClickFindButton() => Click(FindButton);

        public bool CheckLinksForSearchTerm(string phrase = "")
        {
            var links = FindElementsByLocator(ArticleLinks);
            return links.All(p => p.Text.Contains(
                phrase,
                StringComparison.OrdinalIgnoreCase
            ));
        }

        public bool CheckAllLinksForSearchTerm(string phrase = "")
        {
            var footer = Driver.FindElement(By.XPath("//footer[contains(@class,'search-results__footer')]"));

            ((IJavaScriptExecutor)Driver).ExecuteScript(@"
                const el = arguments[0];
                const rect = el.getBoundingClientRect();
                window.scrollTo({
                    top: rect.bottom + window.scrollY - window.innerHeight,
                    behavior: 'instant'
                });
            ", footer);

            while (true)
            {
                var searchButton = TryFindElement(SearchResultMore);

                if (searchButton == null)
                {
                    try
                    {
                        ExplicitWait.Until(driver =>
                            TryFindElement(SearchResultMore) != null
                        );

                        continue;
                    }
                    catch (WebDriverTimeoutException)
                    {
                        break;
                    }
                }

                searchButton.Click();

                ExplicitWait.Until(driver =>
                {
                    var button = TryFindElement(SearchResultMore);
                    return button == null || button.Displayed;
                });
            }

            var links = FindElementByLocator(SearchResultContainer);
            var elements = links.FindElements(ArticleParagraphs);

            return elements.All(p => p.Text.Contains(
                phrase,
                StringComparison.OrdinalIgnoreCase
            ));
        }
    }
}
