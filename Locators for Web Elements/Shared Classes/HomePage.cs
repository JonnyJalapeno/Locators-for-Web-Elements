using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Text.Json;

namespace Locators_for_Web_Elements
{
    public class HomePage :  BaseComponent<HomePage>
    {
        private struct EpamConfig
        {
            public string WebUrl { get; set; }
        }
        private string WebUrl { get; set; }


        private readonly By Insights = By.XPath("//nav//a[normalize-space()='Insights']");    
        private readonly By Career = By.LinkText("Careers"); //LinkText locator
        private readonly By MangifierGlass = By.XPath("//button[contains(@class, 'header-search__button')]");
        private readonly By SearchInput = By.Id("new_form_search");
        private readonly By FindButton = By.XPath("//div[contains(@class, 'search-results__action-section')]//button");
        private readonly By CodeOfConductLink =
            By.XPath("//a[contains(normalize-space(.), 'Ethical Conduct')]");

        public HomePage(IWebDriver driver, WebDriverWait wait) : base(driver, wait)
        {
            EpamConfig config = DeserializeAppSettings();
            LoadJsonValues(config);
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

        public CodeOfConduct ClickCodeOfConduct()
        {
            var element = Driver.FindElement(CodeOfConductLink);
            new Actions(Driver)
        .MoveToElement(element)
        .Pause(TimeSpan.FromMilliseconds(200))
        .Click()
        .Perform();

            element.Click();
            return new CodeOfConduct(Driver, Wait);
        }
    }
}