using Locators_for_Web_Elements.Core;
using OpenQA.Selenium;

namespace Locators_for_Web_Elements.Business
{
    public class CareerPage
    {
        private readonly IElementInteractor Interactor;

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

        public CareerPage(IElementInteractor interactor)
        {
            Interactor = interactor;
        }

        public CareerPage ClickSearchCareers()
        {
            Interactor.ClickElement(SearchCareersLocator);
            return this;
        }

        public CareerPage TypeIntoRoleOrKeywordSearch(string phrase)
        {
            Interactor.TypeIntoElement(SearchRoleOrKeyword, phrase);
            return this;
        }

        public CareerPage ClickTheSearchButton()
        {
            Interactor.ClickElementSafely(SearchButtonCareerPage);
            return this;
        }

        public CareerPage SelectCountryFromDropdown(string countryName)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(countryName);
            var input = Interactor.FindElementByLocator(CountryDropdownButton);
            input.Click();
            input.SendKeys(countryName + Keys.Enter);
            Interactor.WaitForUrlToContain(countryName);

            return this;
        }

        public CareerPage ClickRemoteButton()
        {
            Interactor.ClickElement(RemoteCheckbox);
            return this;
        }

        public CareerPage ExpandJobDescription()
        {
            Interactor.ClickElementSafely(ExpandJobButton);
            return this;
        }

        public bool JobDescriptionContainsKeyword(string phrase)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(phrase);
            var container = Interactor.FindElementByLocator(JobDescriptionContainer);
            var paragraphs = container.FindElements(JobDescriptionParagraphs);
            return paragraphs.Any(p => Interactor.ElementContainsPhrase(p, phrase));
        }

        public CareerPage AcceptCookies()
        {
            Interactor.AcceptCookies(CommonLocators.CookiesAcceptButton, CommonLocators.PrivacyDialog);
            return this;
        }
    }
}
