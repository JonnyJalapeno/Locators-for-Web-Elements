using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using Microsoft.Extensions.Options;


namespace Locators_for_Web_Elements
{
    public class HomePage :  BaseComponent<HomePage>
    {
        private string WebUrl { get; set; }

        private readonly By Insights =
            By.XPath("//nav//a[contains(@class,'top-navigation__item-link') and normalize-space()='Insights']");
        private readonly By Career = By.LinkText("Careers"); //LinkText locator
        private readonly By MangifierGlass = By.XPath("//button[contains(@class, 'header-search__button')]");
        private readonly By SearchInput = By.Id("new_form_search");
        private readonly By FindButton = By.XPath("//div[contains(@class, 'search-results__action-section')]//button");
        private readonly By CodeOfConductLink =
            By.XPath("//a[contains(normalize-space(.), 'Ethical Conduct')]");

        public HomePage(IWebDriver driver, WebDriverWait wait, IPageFactory pageFactory, IOptions<EpamConfig> config)
            : base(driver, wait, pageFactory)
        {
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

        public InsightsPage ClickInsights() => FindAndClick<InsightsPage>(Insights);

        public CareerPage ClickCareers() => FindAndClick<CareerPage>(Career);

        public HomePage ClickMagnifierSearch() => FindAndClick(MangifierGlass);

        public HomePage InputPhraseIntoMagnifierSearch(string phrase) => FindAndType(SearchInput, phrase);

        public SearchPage ClickFindButton() => FindAndClick<SearchPage>(FindButton);

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
    }
}