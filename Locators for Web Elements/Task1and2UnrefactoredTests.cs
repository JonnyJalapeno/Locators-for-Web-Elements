using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Locators_for_Web_Elements
{
    public class Task1and2UnrefactoredTests
    {
        private struct EpamConfig
        {
            public string WebUrl { get; set; }
        }

        private IWebDriver Driver { get; set; }
        private WebDriverWait ExplicitWait { get; set; }
        private string WebUrl { get; set; }

        private readonly By PrivacyBanner = By.CssSelector("div[role='dialog'][aria-label='Privacy']");
        private readonly By CookiesAcceptanceLocator = By.Id("onetrust-accept-btn-handler"); //Id locator
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

        [SetUp]
        public void Setup()
        {
            LoadJsonValues(DeserializeAppSettings());

            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            Driver = new ChromeDriver(options);

            ExplicitWait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));

            Driver.Navigate().GoToUrl(WebUrl);
        }

        [TearDown]
        public void Teardown()
        {
            Driver.Quit();
            Driver.Dispose();
        }

        private EpamConfig DeserializeAppSettings()
        {
            string fileName = "appsettings.json";
            try
            {
                using FileStream openStream = File.OpenRead(fileName);
                EpamConfig config = JsonSerializer.Deserialize<EpamConfig>(openStream);
                return config;
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

        private void LoadJsonValues(EpamConfig config)
        {
            WebUrl = config.WebUrl;
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
                catch (NoSuchElementException) { return null; }
                catch (StaleElementReferenceException) { return null; }
            });
        }

        private IEnumerable<IWebElement> FindElementsByLocator(By locator, IWebElement scope)
        {
            return ExplicitWait.Until(driver =>
            {
                try
                {
                    var elements = scope.FindElements(locator);
                    return elements.All(d => d.Displayed) && elements.All(d => d.Enabled) ? elements : null;
                }
                catch (NoSuchElementException) { return null; }
                catch (StaleElementReferenceException) { return null; }
            });
        }

        //We can't just click accept and wait,
        //banner has animation and obscures
        //other elements and intercepting the clicks
        private void AcceptCookies()
        {
            try
            {
                var button = ExplicitWait.Until(driver =>
                {
                    IWebElement element;
                    try { element = driver.FindElement(CookiesAcceptanceLocator); }
                    catch (NoSuchElementException) { return null; }

                    if (!element.Displayed || !element.Enabled) return null;

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

        private void SelectCountryFromDropdown(string countryName)
        {
            var input = FindElementByLocator(CountryDropdownButton);
            input.Click();
            input.SendKeys(countryName + Keys.Enter);
            ExplicitWait.Until(driver =>
                driver.Url.Contains(countryName, StringComparison.OrdinalIgnoreCase));
        }

        private bool ElementContainsPhrase(IWebElement element, string phrase)
        {
            return element.Text.Contains(phrase, StringComparison.OrdinalIgnoreCase);
        }

        private bool JobDescriptionContainsKeyword(string phrase)
        {
            var container = FindElementByLocator(JobDescriptionContainer);
            var paragraphs = container.FindElements(JobDescriptionParagraphs);
            return paragraphs.Any(p => ElementContainsPhrase(p, phrase));
        }

        private bool CheckLinksForSearchTerm(string phrase)
        {
            var links = ExplicitWait.Until(driver =>
            {
                var container = FindElementByLocator(SearchResultContainer);
                var found = FindElementsByLocator(ArticleLinks, container).ToList();
                return found.Count > 0 ? found : null;
            });
            return links.All(element => ElementContainsPhrase(element, phrase));
        }

        [TestCase("blockchain", "Serbia")]
        [TestCase("python", "Uzbekistan")]
        public void Task1(string keyword, string country)
        {
            AcceptCookies();

            FindElementByLocator(CareerLocator).Click();

            FindElementByLocator(SearchCareersLocator).Click();

            AcceptCookies();

            SelectCountryFromDropdown(country);

            FindElementByLocator(RemoteCheckbox).Click();

            FindElementByLocator(SearchRoleOrKeyword).SendKeys(keyword);

            FindElementByLocator(SearchButtonCareerPage).Click();

            FindElementByLocator(ExpandJobButton).Click();

            Assert.That(JobDescriptionContainsKeyword(keyword));
        }

        [TestCase("BLOCKCHAIN")]
        [TestCase("Cloud")]
        [TestCase("Automation")]
        public void Task2(string phrase)
        {
            AcceptCookies();

            FindElementByLocator(SearchButtonMainPage).Click();

            FindElementByLocator(SearchInputMainPage).SendKeys(phrase);

            FindElementByLocator(FindButton).Click();

            Assert.That(CheckLinksForSearchTerm(phrase));
        }
    }
}