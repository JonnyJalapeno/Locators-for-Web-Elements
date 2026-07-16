using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Locators_for_Web_Elements
{
    public class CareerPage : BaseComponent<CareerPage>
    {

        private readonly By SearchCareersLocator = 
            By.XPath("//div[@data-gtm-category='job_search_redirect']/descendant::a"); //XPath locator with axes
        private readonly By SearchRoleOrKeyword = 
            By.Name("search"); //Name locator
        private readonly By SearchButtonCareerPage = 
            By.XPath("//button[@name='submit_search_box_button' and @type='submit']"); //XPath locator with operator[and]
        private readonly By CountryDropdownButton = 
            By.CssSelector("input[aria-label='Choose your country']");
        private readonly By RemoteCheckbox = 
            By.XPath("//fieldset[@aria-labelledby='Workplace type-filter-title']//label[.//span[text()='Remote']]");
        private readonly By ExpandJobButton = 
            By.XPath("//div[contains(@class, 'JobCard')]//span[@data-testid='accordion-section-header-icon-container']");
        private readonly By JobDescriptionContainer = 
            By.XPath("//div[@data-testid='categories-container']");
        private readonly By JobDescriptionParagraphs = 
            By.XPath("//div[@data-testid='rich-text']");

        public CareerPage(IWebDriver driver, WebDriverWait wait) : base(driver, wait)
        {
        }

        public CareerPage ClickSearchCareers() => FindAndClick(SearchCareersLocator);
        public CareerPage TypeIntoRoleOrKeywordSearch(string phrase) => FindAndType(SearchRoleOrKeyword, phrase);
        public CareerPage ClickTheSearchButton() => FindAndClick(SearchButtonCareerPage);

        public CareerPage SelectCountryFromDropdown(string countryName)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(countryName);
            var input = FindElementByLocator(CountryDropdownButton);
            input.Click();
            input.SendKeys(countryName + Keys.Enter);
            WaitForUrlToContain(countryName);

            return this;
        }

        public CareerPage ClickRemoteButton() => FindAndClick(RemoteCheckbox);

        public CareerPage ExpandJobDescription() => FindAndClick(ExpandJobButton);

        public bool JobDescriptionContainsKeyword(string phrase)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(phrase);
            var container = FindElementByLocator(JobDescriptionContainer);
            var paragraphs = container.FindElements(JobDescriptionParagraphs);
            return paragraphs.Any(p => ElementContainsPhrase(p, phrase));
        }
    }
}