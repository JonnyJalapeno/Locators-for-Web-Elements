using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Text.Json;

namespace Locators_for_Web_Elements
{
    public class Task1and2Refactored
    {
        private struct EpamConfig
        {
            public string WebUrl { get; set; }
        }
        private string WebUrl { get; set; }
        public IWebDriver Driver { get; init; }
        private WebDriverWait ExplicitWait { get; init; }

        private readonly By PrivacyBanner = By.CssSelector("div[role='dialog'][aria-label='Privacy']");
        private readonly By CookiesAcceptanceLocator = By.Id("onetrust-accept-btn-handler");
        private readonly By CareerLocator = By.LinkText("Careers"); //LinkText locator
        private readonly By SearchCareersLocator = By.XPath("//div[@data-gtm-category='job_search_redirect']/descendant::a"); //XPath locator with axes
        private readonly By SearchRoleOrKeyword = By.Name("search"); //Name locator
        private readonly By SearchButtonCareerPage = By.XPath("//button[@name='submit_search_box_button' and @type='submit']"); //XPath locator with operator[and]
        private readonly By CountryDropdownButton = By.CssSelector("input[aria-label='Choose your country']");
        private readonly By RemoteCheckbox = By.XPath("//fieldset[@aria-labelledby='Workplace type-filter-title']//label[.//span[text()='Remote']]");
        private readonly By ExpandJobButton = By.XPath("//div[contains(@class, 'JobCard')]//span[@data-testid='accordion-section-header-icon-container']");
        private readonly By JobDescriptionContainer = By.XPath("//div[@data-testid='categories-container']");
        private readonly By JobDescriptionParagraphs = By.XPath("//div[@data-testid='rich-text']");
        private readonly By SearchButtonMainPage = By.XPath("//button[contains(@class, 'header-search__button')]");
        private readonly By SearchInputMainPage = By.Id("new_form_search");
        private readonly By FindButton = By.XPath("//div[contains(@class, 'search-results__action-section')]//button");
        private readonly By ArticleLinks = By.TagName("a");
        private readonly By ArticleParagraphs = By.TagName("p");
        private readonly By SearchResultContainer = By.XPath("//div[contains(@class, 'search-results__items')]");
        private readonly By SearchResultMore = By.XPath("//a[contains(@class,'search-results__view-more') and not(contains(concat(' ', normalize-space(@class), ' '), ' hidden '))]");
        private readonly By Footer = By.XPath("//footer[contains(@class,'search-results__footer')]");
        private readonly By Article = By.XPath("//article[contains(@class, 'search-results__item')]");

        public Task1and2Refactored(IWebDriver driver)
        {
            EpamConfig config = DeserializeAppSettings();
            LoadJsonValues(config);
            Driver = driver;
            ExplicitWait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
        }

        private void LoadJsonValues(EpamConfig config)
        {
            WebUrl = config.WebUrl;
        }
        private static EpamConfig DeserializeAppSettings()
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

        public Task1and2Refactored NavigateToHome()
        {
            Driver.Navigate().GoToUrl(WebUrl);
            return this;
        }

        public void Quit() => Driver.Quit();

        private Task1and2Refactored FindAndClick(By locator)
        {
            FindElementByLocator(locator).Click();
            return this;
        }

        private Task1and2Refactored FindAndType(By locator, string text)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(text);
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

        private IEnumerable<IWebElement> FindElementsByLocator(By locator, IWebElement scope)
        {
            return ExplicitWait.Until(driver =>
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


        //We can't just click accept and wait,
        //banner has animation and obscures
        //other elements and intercepting the clicks
        public Task1and2Refactored AcceptCookies()
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

        public Task1and2Refactored ClickCareers() => FindAndClick(CareerLocator);

        public Task1and2Refactored ClickSearchCareers() => FindAndClick(SearchCareersLocator);

        public Task1and2Refactored TypeIntoRoleOrKeywordSearch(string phrase) => FindAndType(SearchRoleOrKeyword, phrase);

        public Task1and2Refactored ClickTheSearchButton() => FindAndClick(SearchButtonCareerPage);

        public Task1and2Refactored SelectCountryFromDropdown(string countryName)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(countryName);
            var input = FindElementByLocator(CountryDropdownButton);
            input.Click();
            input.SendKeys(countryName + Keys.Enter);
            WaitForUrlToContain(countryName);

            return this;
        }

        private void WaitForUrlToContain(string phrase)
        {
            ExplicitWait.Until(driver =>
                driver.Url.Contains(phrase, StringComparison.OrdinalIgnoreCase));
        }

        public Task1and2Refactored ClickRemoteButton() => FindAndClick(RemoteCheckbox);

        public Task1and2Refactored ExpandJobDescription() => FindAndClick(ExpandJobButton);

        public bool JobDescriptionContainsKeyword(string phrase)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(phrase);
            var container = FindElementByLocator(JobDescriptionContainer);
            var paragraphs = container.FindElements(JobDescriptionParagraphs);
            return paragraphs.Any(p => ElementContainsPhrase(p, phrase));
        }

        public Task1and2Refactored ClickMagnifierSearch() => FindAndClick(SearchButtonMainPage);

        public Task1and2Refactored InputPhraseIntoMagnifierSearch(string phrase) => FindAndType(SearchInputMainPage, phrase);

        public Task1and2Refactored ClickFindButton() => FindAndClick(FindButton);

        public bool CheckLinksForSearchTerm(string phrase)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(phrase);
            var links = ExplicitWait.Until(driver =>
            {
                var container = FindElementByLocator(SearchResultContainer);
                var links = FindElementsByLocator(ArticleLinks, container).ToList();
                return links.Count > 0 ? links : null;
            });
            return links.All(element => ElementContainsPhrase(element, phrase));
        }

        private static bool ElementContainsPhrase(IWebElement element, string phrase)
        {
            return element.Text.Contains(phrase, StringComparison.OrdinalIgnoreCase);
        }

        public bool CheckAllLinksForSearchTerm(string phrase)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(phrase);
            var footer = Driver.FindElement(Footer);

            new Actions(Driver).ScrollToElement(footer).Perform();

            Driver.FindElement(SearchResultMore).Click();

            var articles = FetchArticles();

            return AllArticlesContainPhrase(articles, phrase);
        }

        private List<IWebElement> FetchArticles()
        { 
            return ExplicitWait.Until(driver =>
            {
                var container = FindElementByLocator(SearchResultContainer);
                var articles = FindElementsByLocator(Article, container).ToList();
                return articles.Count > 0 ? articles : null;
            });
        }

        private bool AllArticlesContainPhrase(IEnumerable<IWebElement> articles, string phrase)
        {
            return articles.All(element =>
            {
                var linkText = element.FindElement(ArticleLinks);
                var paragraphText = element.FindElement(ArticleParagraphs);

                return ElementContainsPhrase(linkText, phrase) ||
                       ElementContainsPhrase(paragraphText, phrase);
            });
        }
    }
}
