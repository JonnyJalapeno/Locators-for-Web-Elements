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

        public By CareerLocator = By.LinkText("Careers"); //LinkText locator
        public By SearchCareersLocator = By.XPath("//div[@data-gtm-category='job_search_redirect']/descendant::a"); //XPath locator with axes
        public By SearchRoleOrKeyword = By.Name("search"); //Name locator
        public By SearchButton = By.XPath("//button[@name='submit_search_box_button' and @type='submit']"); //XPath locator with operator[and]
        public By CountryDropdownButton = By.CssSelector("input[id*='react-select']"); //CSS locator
        public By SelectCountryListbox = By.XPath("//div[@role='listbox']");
        //public By CountryDiv = By.XPath("//div[contains(@id,'react-select') and .//span]");
        public By CountryDiv(string country) =>
    By.XPath($"//div[@role='option' and span[normalize-space(.)='{country}']]");

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

                    return element.Displayed && element.Enabled
                        ? element
                        : null;
                }
                catch (NoSuchElementException)
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
            input.SendKeys(Keys.ArrowDown);
            var country = this.Driver.FindElement(CountryDiv(countryName));
            country.Click();


            return this;
        }
    }
}
