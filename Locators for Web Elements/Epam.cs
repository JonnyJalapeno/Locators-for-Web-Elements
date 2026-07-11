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
        public string? WebUrl { get; private set; }
        public IWebDriver Driver { get; init; }
        public WebDriverWait ExplicitWait { get; init; }

        /*
         Locators to cover in these tasks:
        • ID locator
        • Name locator
        • ClassName locator
        • TagName locator
        • LinkText locator
        • PartialLinkText locator
        • CSS locator (if possible, use pseudo-classes)
        • XPath locator (Relative path)
        • XPath locator with any operator
        • XPath locator with axes
         */

        public By CookiesAcceptanceLocator = By.Id("onetrust-accept-btn-handler");
        public By CareerLocator = By.LinkText("Careers"); //LinkText locator
        public By SearchCareersLocator = By.XPath("//div[@data-gtm-category='job_search_redirect']/descendant::a"); //XPath locator with axes
        public By SearchRoleOrKeyword = By.Name("search"); //Name locator
        public By SearchButton = By.XPath("//button[@name='submit_search_box_button' and @type='submit']"); //XPath locator with operator[and]
        //public By CountryDropdownButton = By.CssSelector("input[id*='react-select']"); //CSS locator
        //public By CountryDropdownButton = By.CssSelector("[data-testid='dropdown-value']");

        public By CountryDropdownButton =
    By.CssSelector("input[aria-label='Choose your country']");
        public By SelectCountryListbox = By.XPath("//div[@role='listbox']");
        public By CountryDiv(string country) =>
            By.XPath($"//div[@role='option' and span[normalize-space(.)='{country}']]");
        public By RemoteCheckbox = By.XPath("//fieldset[@aria-labelledby='Workplace type-filter-title']//label[.//span[text()='Remote']]");

        public Epam(IWebDriver driver)
        {
            LoadAndInitializeUrl();
            this.Driver = driver;
            this.ExplicitWait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(10));
        }

        public IWebElement FindElementByLocator(By locator)
        {
            return this.ExplicitWait.Until(driver =>
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

        public void LoadAndInitializeUrl()
        {
            string fileName = "appsettings.json";
            try
            {
                using FileStream openStream = File.OpenRead(fileName);
                EpamConfig config = JsonSerializer.Deserialize<EpamConfig>(openStream);
                this.WebUrl = config.WebUrl;
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"File was not found: {ex.FileName}");
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"WebUrl value in json file was not assigned");
            }
        }

        public void AcceptCookies()
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
        }

        public Epam FindAndClickCareers()
        {
            IWebElement careers = FindElementByLocator(CareerLocator);
            careers.Click();
            return this;
        }

        public Epam FindAndClickSearchCareers()
        {
            IWebElement searchbutton = FindElementByLocator(SearchCareersLocator);
            searchbutton.Click();
            return this;
        }

        public Epam FindAndTypeIntoRoleOrKeywordSearch(string phrase = "")
        {
            IWebElement searchField = FindElementByLocator(SearchRoleOrKeyword);
            searchField.SendKeys(phrase);
            return this;
        }

        public Epam ClickTheSearchButton()
        {
            IWebElement searchButton = FindElementByLocator(SearchButton);
            searchButton.Click();
            return this;
        }

        public Epam SelectCountryFromDropdown(string countryName)
        {
            var input = FindElementByLocator(CountryDropdownButton);
            input.Click();
            input.SendKeys(countryName);
            input.SendKeys(Keys.Enter);
            //var country = this.Driver.FindElement(CountryDiv(countryName));
            //var country = FindElementByLocator(CountryDiv(countryName));
            //country.Click();

            return this;
        }

        public Epam ClickRemoteButton()
        {
            var element = FindElementByLocator(RemoteCheckbox);

            element.Click();
            
            return this;
        }
    }
}
