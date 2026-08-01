using Locators_for_Web_Elements.Core;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Locators_for_Web_Elements.Business
{
    public class HomePage
    {
        private readonly IWebDriver Driver;
        private readonly IPageFactory PageFactory;
        private readonly IElementInteractor Interactor;

        private string WebUrl { get; set; }

        private readonly By Insights =
            By.XPath("//nav//a[contains(@class,'top-navigation__item-link') and normalize-space()='Insights']");
        private readonly By Career = By.LinkText("Careers"); //LinkText locator
        private readonly By MangifierGlass = By.XPath("//button[contains(@class, 'header-search__button')]");
        private readonly By SearchInput = By.Id("new_form_search");
        private readonly By FindButton = By.XPath("//div[contains(@class, 'search-results__action-section')]//button");
        private readonly By CodeOfConductLink =
            By.XPath("//a[contains(normalize-space(.), 'Ethical Conduct')]");

        public HomePage(IWebDriver driver, IPageFactory pageFactory, IElementInteractor interactor, IOptions<EpamConfig> config)
        {
            Driver = driver;
            PageFactory = pageFactory;
            Interactor = interactor;
            WebUrl = config.Value.WebUrl;

            if (string.IsNullOrWhiteSpace(WebUrl))
            {
                throw new InvalidOperationException("WebUrl was not set in the config file.");
            }
        }

        public HomePage NavigateToHome()
        {
            Driver.Navigate().GoToUrl(WebUrl);
            return this;
        }

        public void Quit() => Driver.Quit();

        public InsightsPage ClickInsights()
        {
            Interactor.ClickElement(Insights);
            return PageFactory.Create<InsightsPage>();
        }

        public CareerPage ClickCareers()
        {
            Interactor.ClickElement(Career);
            return PageFactory.Create<CareerPage>();
        }

        public HomePage ClickMagnifierSearch()
        {
            Interactor.ClickElement(MangifierGlass);
            return this;
        }

        public HomePage InputPhraseIntoMagnifierSearch(string phrase)
        {
            Interactor.TypeIntoElement(SearchInput, phrase);
            return this;
        }

        public SearchPage ClickFindButton()
        {
            Interactor.ClickElement(FindButton);
            return PageFactory.Create<SearchPage>();
        }

        public HomePage ClickCodeOfConduct()
        {
            var element = Driver.FindElement(CodeOfConductLink);
            new Actions(Driver)
                .MoveToElement(element)
                .Pause(TimeSpan.FromMilliseconds(200))
                .Click()
                .Perform();

            element.Click();
            return this;
        }

        public HomePage AcceptCookies()
        {
            Interactor.AcceptCookies(CommonLocators.CookiesAcceptButton, CommonLocators.PrivacyDialog);
            return this;
        }
    }
}
