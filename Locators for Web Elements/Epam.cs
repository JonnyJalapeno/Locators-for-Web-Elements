using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text;
using System.Text.Json;

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
        public By CountryDropdownButton = By.CssSelector("div[class*='Dropdown_defaultOption']"); //CSS locator
        public By SelectCountryListbox = By.XPath("//div[@role='listbox']");
        public By CountryDiv = By.CssSelector("div[class*='SingleOption']");
        public Epam(IWebDriver driver)
        {
            LoadAndInitializeUrl();
            this.Driver = driver;
            this.Driver.Url = this.WebUrl;
            this.ExplicitWait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(10));
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
            IWebElement careers = this.ExplicitWait.Until(driver =>
            {
                var element = this.Driver.FindElement(this.CareerLocator);
                return element.Displayed && element.Enabled ? element : null;
            });
            careers.Click();
            return this;
        }

        public Epam FindAndClickSearchCareers()
        {
            IWebElement searchbutton = this.ExplicitWait.Until(driver =>
            {
                var element = this.Driver.FindElement(this.SearchCareersLocator);
                return element.Displayed && element.Enabled ? element : null;
            });
            searchbutton.Click();
            return this;
        }

        public Epam FindAndTypeIntoRoleOrKeywordSearch(string phrase = "")
        {
            IWebElement searchField = this.ExplicitWait.Until(driver =>
            {
                var element = this.Driver.FindElement(this.SearchRoleOrKeyword);
                return element.Displayed && element.Enabled ? element : null;
            });
            searchField.SendKeys(phrase);
            return this;
        }

        public Epam ClickTheSearchButton()
        {
            IWebElement searchButton = this.ExplicitWait.Until(driver =>
            {
                var element = this.Driver.FindElement(this.SearchButton);
                return element.Displayed && element.Enabled ? element : null;
            });
            searchButton.Click();
            return this;
        }

        public Epam SelectCountryFromDropdown(string countryName)
        {
            IWebElement countryDropdownButton = this.ExplicitWait.Until(driver =>
            {
                var element = this.Driver.FindElement(this.CountryDropdownButton);
                return element.Displayed && element.Enabled ? element : null;
            });
            countryDropdownButton.Click();

            IWebElement listboxWithCountries = this.ExplicitWait.Until(driver =>
            {
                var element = this.Driver.FindElement(this.SelectCountryListbox);
                return element.Displayed && element.Enabled ? element : null;
            });

            IEnumerable<IWebElement> listboxCountries = this.ExplicitWait.Until(driver =>
            {
                var countries = listboxWithCountries.FindElements(this.CountryDiv);
                return countries.Count > 0 ? countries : null;
            });

            //option.Click();
            return this;
        }
    }
}
